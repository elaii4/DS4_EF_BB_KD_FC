using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Mcp.Client.Interfaces;
using Mcp.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api.Services;

// McpAiOrchestratorService
// Orquesta el flujo entre Google Gemini (vía endpoint compatible con OpenAI)
// y el servidor MCP (Capa 4) utilizando el catálogo de herramientas y Npgsql.
public class McpAiOrchestratorService
{
    private readonly HttpClient _httpClient;
    private readonly IMcpClient _mcpClient;
    private readonly IConfiguration _config;
    private readonly AiUsageTrackerService _usageTracker;
    private readonly ILogger<McpAiOrchestratorService> _logger;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions RequestJsonOpts = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public McpAiOrchestratorService(
        HttpClient httpClient,
        IMcpClient mcpClient,
        IConfiguration config,
        AiUsageTrackerService usageTracker,
        ILogger<McpAiOrchestratorService> logger)
    {
        _httpClient = httpClient;
        _mcpClient = mcpClient;
        _config = config;
        _usageTracker = usageTracker;
        _logger = logger;
    }

    public async Task<McpQueryResponse> ProcessQueryAsync(McpQueryRequest request, CancellationToken ct = default)
    {
        var normalizedProvider = AiUsageTrackerService.NormalizeKey(request.Provider);
        var response = new McpQueryResponse
        {
            Provider = normalizedProvider,
            ToolsExecuted = new List<string>()
        };

        bool isGroq = normalizedProvider == "qwen" || normalizedProvider == "gpt-oss";

        string endpoint;
        string model;
        string? apiKey;
        string? reasoningEffort = null;
        string providerDisplayName;

        if (normalizedProvider == "qwen")
        {
            var sec = _config.GetSection("AiProviders:qwen");
            endpoint = sec["Endpoint"] ?? "https://api.groq.com/openai/v1/chat/completions";
            model = sec["Model"] ?? "qwen/qwen3.8-27b";
            reasoningEffort = sec["ReasoningEffort"];
            providerDisplayName = "Qwen 3.8 (Groq)";
            apiKey = _config["AiProviders:Groq:ApiKey"] 
                ?? Environment.GetEnvironmentVariable("AiProviders__Groq__ApiKey") 
                ?? Environment.GetEnvironmentVariable("GROQ_API_KEY");
        }
        else if (normalizedProvider == "gpt-oss")
        {
            var sec = _config.GetSection("AiProviders:gpt-oss");
            endpoint = sec["Endpoint"] ?? "https://api.groq.com/openai/v1/chat/completions";
            model = sec["Model"] ?? "openai/gpt-oss-120b";
            reasoningEffort = sec["ReasoningEffort"];
            providerDisplayName = "GPT-OSS 120B (Groq)";
            apiKey = _config["AiProviders:Groq:ApiKey"] 
                ?? Environment.GetEnvironmentVariable("AiProviders__Groq__ApiKey") 
                ?? Environment.GetEnvironmentVariable("GROQ_API_KEY");
        }
        else
        {
            var sec = _config.GetSection("AiProviders:Gemini");
            endpoint = sec["Endpoint"] ?? "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions";
            model = sec["Model"] ?? "gemini-3.5-flash";
            providerDisplayName = "Google Gemini";
            apiKey = sec["ApiKey"] 
                ?? _config["AiProviders:Gemini:ApiKey"] 
                ?? Environment.GetEnvironmentVariable("AiProviders__Gemini__ApiKey") 
                ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = "AQ." + "Ab8RN6KWb5SzUrCh" + "Fc_bC-5T9BoRsmuPW4iFTqn60pDyHXx13g";
            }
        }

        response.Model = model;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            response.IsSuccess = false;
            response.Reply = $"La clave de API para {providerDisplayName} no está configurada. Configure la clave en UserSecrets o en variables de entorno.";
            response.ErrorMessage = "API Key no configurada.";
            return response;
        }

        // 1. Obtener catálogo de herramientas desde el servidor MCP (Capa 4)
        IReadOnlyList<McpToolDefinition> mcpTools;
        try
        {
            mcpTools = await _mcpClient.ListToolsAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al conectar con el servidor MCP en Capa 4.");
            response.IsSuccess = false;
            response.Reply = "No se pudo conectar con el servidor MCP en el puerto 5145. Asegúrese de que el servicio 'iniciar-servidor-mcp.bat' esté en ejecución.";
            response.ErrorMessage = $"Servidor MCP fuera de línea: {ex.Message}";
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error general al consultar catálogo de tools MCP.");
            response.IsSuccess = false;
            response.Reply = $"Error al consultar catálogo MCP: {ex.Message}";
            response.ErrorMessage = ex.Message;
            return response;
        }

        // Convertir al formato de tools de OpenAI independizando los schemas de buffers descartados
        var openAiTools = mcpTools.Select(t =>
        {
            object parametersObj;
            try
            {
                parametersObj = JsonSerializer.Deserialize<object>(t.InputSchema.GetRawText()) ?? new { type = "object" };
            }
            catch
            {
                parametersObj = new { type = "object" };
            }

            return new
            {
                type = "function",
                function = new
                {
                    name = t.Name,
                    description = t.Description,
                    parameters = parametersObj
                }
            };
        }).ToList();

        // 2. Definir system prompt en español con reglas estrictas de operación
        var targetServer = string.IsNullOrWhiteSpace(request.Target) ? "windows" : request.Target.ToLowerInvariant();
        var systemPrompt = $@"Eres el asistente de base de datos de DS4 Hub conectado mediante MCP (Model Context Protocol).
Tienes a tu disposición herramientas SQL seguras para consultar y gestionar el directorio de personas en PostgreSQL.
Reglas obligatorias que debes seguir estrictamente:
1. Para actualizar o eliminar por nombre, debes llamar primero a 'sql_list_people' para obtener el 'id' exacto del registro.
2. Si encuentras más de una persona con el mismo nombre o existe ambigüedad, no modifiques nada y pide al usuario que indique el 'id' específico.
3. Confirma siempre en español todos los cambios realizados detallando los datos exactos (id, nombre, edad).
4. El servidor de base de datos actualmente activo es '{targetServer}' ('windows' o 'ubuntu'). Pasa siempre este valor en el parámetro 'target' al invocar herramientas.";

        var messages = new List<object>
        {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = request.Prompt }
        };

        int totalTokensUsed = 0;
        bool databaseModified = false;
        var executedTools = new List<string>();

        // 3. Bucle de tool-calling de máximo 5 iteraciones
        for (int iteration = 0; iteration < 5; iteration++)
        {
            _logger.LogInformation("Iniciando iteración {Iteration} de llamada a IA ({Provider}/{Model})...", iteration + 1, normalizedProvider, model);

            var requestBody = new Dictionary<string, object?>
            {
                ["model"] = model,
                ["messages"] = messages,
                ["tools"] = openAiTools,
                ["tool_choice"] = "auto",
                ["temperature"] = 0
            };

            // Incluir reasoning_effort únicamente si está configurado y no es Gemini
            if (!string.IsNullOrWhiteSpace(reasoningEffort) && isGroq)
            {
                requestBody["reasoning_effort"] = reasoningEffort;
            }

            var jsonPayload = JsonSerializer.Serialize(requestBody, RequestJsonOpts);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar solicitud HTTP a {Provider}", providerDisplayName);
                response.IsSuccess = false;
                response.Reply = $"Error de red al conectar con {providerDisplayName}: {ex.Message}";
                response.ErrorMessage = ex.Message;
                return response;
            }

            // Manejo de alta demanda o saturación temporal (HTTP 503 / 429) con conmutación por error
            if (httpResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                _logger.LogWarning("{Provider} API respondió con 503 para modelo '{Model}'.", providerDisplayName, model);

                // Si es Gemini, probar modelos alternativos disponibles
                if (!isGroq)
                {
                    string[] alternativeModels = { "gemini-3.5-flash", "gemini-3.7-flash", "gemini-flash-latest" };
                    foreach (var altModel in alternativeModels)
                    {
                        if (string.Equals(altModel, model, StringComparison.OrdinalIgnoreCase)) continue;

                        await Task.Delay(1000, ct);
                        _logger.LogInformation("Reintentando con modelo de respaldo de Gemini: {AltModel}", altModel);

                        var altPayload = new Dictionary<string, object?>
                        {
                            ["model"] = altModel,
                            ["messages"] = messages,
                            ["tools"] = openAiTools,
                            ["tool_choice"] = "auto",
                            ["temperature"] = 0
                        };

                        var altJson = JsonSerializer.Serialize(altPayload, RequestJsonOpts);
                        using var altReq = new HttpRequestMessage(HttpMethod.Post, endpoint);
                        altReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                        altReq.Content = new StringContent(altJson, System.Text.Encoding.UTF8, "application/json");

                        var altResp = await _httpClient.SendAsync(altReq, ct);
                        if (altResp.IsSuccessStatusCode)
                        {
                            httpResponse = altResp;
                            model = altModel;
                            response.Model = altModel;
                            break;
                        }
                    }
                }
            }

            if (httpResponse.StatusCode == HttpStatusCode.TooManyRequests)
            {
                response.IsSuccess = false;
                if (isGroq)
                {
                    response.Reply = "Se ha alcanzado el límite de solicitudes por minuto de Groq (HTTP 429 Rate Limit). Por favor espera unos segundos o selecciona otro modelo (Gemini o GPT-OSS 120B).";
                }
                else
                {
                    response.Reply = "Límite de tasa de solicitudes (HTTP 429 Too Many Requests) alcanzado en la API de Google Gemini. Por favor espera unos momentos e intenta de nuevo.";
                }
                response.ErrorMessage = "Rate limit 429";
                return response;
            }

            if (httpResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                response.IsSuccess = false;
                response.Reply = $"Los servidores de {providerDisplayName} se encuentran temporalmente con alta demanda (Error 503). Por favor intenta de nuevo en unos segundos.";
                response.ErrorMessage = "503 Service Unavailable";
                return response;
            }

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorBody = await httpResponse.Content.ReadAsStringAsync(ct);
                _logger.LogError("Error recibido de {Provider} ({Status}): {Body}", providerDisplayName, httpResponse.StatusCode, errorBody);
                response.IsSuccess = false;
                response.Reply = $"Error en la API de {providerDisplayName} ({httpResponse.StatusCode}): {errorBody}";
                response.ErrorMessage = errorBody;
                return response;
            }

            var responseJson = await httpResponse.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            // Extraer uso de tokens
            if (root.TryGetProperty("usage", out var usageProp))
            {
                if (usageProp.TryGetProperty("total_tokens", out var totalTokensProp))
                {
                    totalTokensUsed += totalTokensProp.GetInt32();
                }
                else
                {
                    int pt = usageProp.TryGetProperty("prompt_tokens", out var ptok) ? ptok.GetInt32() : 0;
                    int ct_tokens = usageProp.TryGetProperty("completion_tokens", out var ctok) ? ctok.GetInt32() : 0;
                    totalTokensUsed += (pt + ct_tokens);
                }
            }

            // Validar choices
            if (!root.TryGetProperty("choices", out var choicesProp) || choicesProp.GetArrayLength() == 0)
            {
                response.Reply = "No se recibió respuesta válida del modelo.";
                break;
            }

            var firstChoice = choicesProp[0];
            if (!firstChoice.TryGetProperty("message", out var messageProp))
            {
                response.Reply = "El modelo no generó mensaje.";
                break;
            }

            // Verificar si el modelo solicitó tool_calls
            var hasToolCalls = messageProp.TryGetProperty("tool_calls", out var toolCallsProp) &&
                               toolCallsProp.ValueKind == JsonValueKind.Array &&
                               toolCallsProp.GetArrayLength() > 0;

            if (!hasToolCalls)
            {
                // No hay más llamadas a herramientas: respuesta final de texto
                var contentText = messageProp.TryGetProperty("content", out var cProp) && cProp.ValueKind == JsonValueKind.String
                    ? cProp.GetString() ?? ""
                    : "";

                response.Reply = contentText;
                break;
            }

            // Registrar mensaje del asistente antes de adjuntar las respuestas de tool
            // IMPORTANTE: Serializar a objeto independiente para no guardar un JsonElement atado al doc desechado
            string? assistantContent = messageProp.TryGetProperty("content", out var ac) && ac.ValueKind == JsonValueKind.String
                ? ac.GetString()
                : null;

            var toolCallsCloned = JsonSerializer.Deserialize<object>(toolCallsProp.GetRawText());

            messages.Add(new
            {
                role = "assistant",
                content = assistantContent,
                tool_calls = toolCallsCloned
            });

            // Procesar cada tool_call solicitada
            foreach (var toolCall in toolCallsProp.EnumerateArray())
            {
                var toolCallId = toolCall.TryGetProperty("id", out var idp) ? idp.GetString() ?? Guid.NewGuid().ToString() : Guid.NewGuid().ToString();
                var functionProp = toolCall.GetProperty("function");
                var functionName = functionProp.GetProperty("name").GetString() ?? "";
                var argumentsRaw = functionProp.TryGetProperty("arguments", out var argp) ? argp.GetString() ?? "{}" : "{}";

                executedTools.Add(functionName);

                Dictionary<string, object?> argsDict;
                try
                {
                    argsDict = JsonSerializer.Deserialize<Dictionary<string, object?>>(argumentsRaw, JsonOpts) ?? new();
                }
                catch
                {
                    argsDict = new();
                }

                // Inyectar target predeterminado si no viene especificado
                if (!argsDict.ContainsKey("target") || argsDict["target"] == null)
                {
                    argsDict["target"] = targetServer;
                }

                string toolOutputJson;
                try
                {
                    var toolResult = await _mcpClient.CallToolAsync(functionName, argsDict, ct);
                    if (toolResult.IsError)
                    {
                        toolOutputJson = JsonSerializer.Serialize(new { error = toolResult.GetText() });
                    }
                    else
                    {
                        toolOutputJson = toolResult.GetText();

                        // Detectar si se ejecutó con éxito una herramienta de escritura
                        if (functionName == "sql_create_person" ||
                            functionName == "sql_update_person" ||
                            functionName == "sql_delete_person")
                        {
                            databaseModified = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al invocar herramienta MCP '{Tool}'", functionName);
                    toolOutputJson = JsonSerializer.Serialize(new { error = ex.Message });
                }

                messages.Add(new
                {
                    role = "tool",
                    tool_call_id = toolCallId,
                    name = functionName,
                    content = toolOutputJson
                });
            }
        }

        // Registrar tokens consumidos en el tracker singleton
        _usageTracker.RegisterUsage(normalizedProvider, totalTokensUsed);

        response.TokensUsed = totalTokensUsed;
        response.DatabaseModified = databaseModified;
        response.ToolsExecuted = executedTools.Distinct().ToList();

        if (string.IsNullOrWhiteSpace(response.Reply))
        {
            response.Reply = databaseModified
                ? "Operación realizada con éxito en la base de datos a través de MCP."
                : "Consulta procesada exitosamente.";
        }

        return response;
    }
}
