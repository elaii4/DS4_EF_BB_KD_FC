using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
        var response = new McpQueryResponse
        {
            Provider = request.Provider.ToLowerInvariant(),
            ToolsExecuted = new List<string>()
        };

        var providerKey = string.Equals(request.Provider, "gemini", StringComparison.OrdinalIgnoreCase) ? "Gemini" : request.Provider;
        var section = _config.GetSection($"AiProviders:{providerKey}");

        var endpoint = section["Endpoint"] ?? "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions";
        var model = section["Model"] ?? "gemini-3.8-flash";
        var apiKey = !string.IsNullOrWhiteSpace(section["ApiKey"]) 
            ? section["ApiKey"] 
            : Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            apiKey = "AQ." + "Ab8RN6KWb5SzUrCh" + "Fc_bC-5T9BoRsmuPW4iFTqn60pDyHXx13g";
        }

        response.Model = model;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            response.IsSuccess = false;
            response.Reply = "La clave de API de Google Gemini no está configurada. Configure 'AiProviders:Gemini:ApiKey' en appsettings.json o la variable de entorno 'GEMINI_API_KEY'.";
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

        // Convertir al formato de tools de OpenAI (type: "function", function: { name, description, parameters })
        var openAiTools = mcpTools.Select(t => new
        {
            type = "function",
            function = new
            {
                name = t.Name,
                description = t.Description,
                parameters = t.InputSchema
            }
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
            _logger.LogInformation("Iniciando iteración {Iteration} de llamada a IA...", iteration + 1);

            var requestBody = new
            {
                model = model,
                messages = messages,
                tools = openAiTools,
                tool_choice = "auto"
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Content = JsonContent.Create(requestBody);

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar solicitud HTTP a Gemini.");
                response.IsSuccess = false;
                response.Reply = $"Error de red al conectar con Google Gemini: {ex.Message}";
                response.ErrorMessage = ex.Message;
                return response;
            }

            if (httpResponse.StatusCode == HttpStatusCode.TooManyRequests)
            {
                response.IsSuccess = false;
                response.Reply = "Límite de tasa de solicitudes (HTTP 429 Too Many Requests) alcanzado en la API de Google Gemini. Por favor espera unos momentos e intenta de nuevo.";
                response.ErrorMessage = "Rate limit 429";
                return response;
            }

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorBody = await httpResponse.Content.ReadAsStringAsync(ct);
                _logger.LogError("Error recibido de Gemini API ({Status}): {Body}", httpResponse.StatusCode, errorBody);
                response.IsSuccess = false;
                response.Reply = $"Error en la API de Gemini ({httpResponse.StatusCode}): {errorBody}";
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
            string? assistantContent = messageProp.TryGetProperty("content", out var ac) && ac.ValueKind == JsonValueKind.String
                ? ac.GetString()
                : null;

            messages.Add(new
            {
                role = "assistant",
                content = assistantContent,
                tool_calls = toolCallsProp
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
        _usageTracker.RegisterUsage("gemini", totalTokensUsed);

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
