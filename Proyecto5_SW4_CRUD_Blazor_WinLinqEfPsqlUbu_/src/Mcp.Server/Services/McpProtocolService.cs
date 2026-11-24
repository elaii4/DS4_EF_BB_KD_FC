using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Mcp.Client.Models;
using Microsoft.Extensions.Logging;

namespace Mcp.Server.Services;

// McpProtocolService
// Orquestador del protocolo MCP (Model Context Protocol).
// Procesa solicitudes JSON-RPC 2.0, expone el catálogo de herramientas y despacha su ejecución.
public class McpProtocolService
{
    private readonly IPostgresToolService _db;
    private readonly ILogger<McpProtocolService> _logger;

    public McpProtocolService(IPostgresToolService db, ILogger<McpProtocolService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public List<McpToolDefinition> GetRegisteredTools()
    {
        return new List<McpToolDefinition>
        {
            new()
            {
                Name = "sql_list_people",
                Description = "Consulta la lista de personas desde PostgreSQL (soporta filtrado por nombre y selección de servidor target 'windows' o 'ubuntu').",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""properties"": {
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" },
                        ""search"": { ""type"": ""string"", ""description"": ""Texto opcional para filtrar por nombre"" },
                        ""limit"": { ""type"": ""integer"", ""description"": ""Límite máximo de resultados (por defecto 100)"", ""default"": 100 }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_get_person",
                Description = "Obtiene los detalles de una persona por su identificador único (ID).",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""id""],
                    ""properties"": {
                        ""id"": { ""type"": ""integer"", ""description"": ""ID único de la persona"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_create_person",
                Description = "Registra una nueva persona en la base de datos PostgreSQL.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""name"", ""age""],
                    ""properties"": {
                        ""name"": { ""type"": ""string"", ""description"": ""Nombre completo de la persona"" },
                        ""age"": { ""type"": ""integer"", ""description"": ""Edad de la persona (0 a 120)"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_update_person",
                Description = "Actualiza los datos (nombre y/o edad) de una persona existente en PostgreSQL.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""id"", ""name"", ""age""],
                    ""properties"": {
                        ""id"": { ""type"": ""integer"", ""description"": ""ID de la persona a modificar"" },
                        ""name"": { ""type"": ""string"", ""description"": ""Nuevo nombre completo"" },
                        ""age"": { ""type"": ""integer"", ""description"": ""Nueva edad"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_delete_person",
                Description = "Elimina de forma permanente una persona por su ID de la base de datos.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""id""],
                    ""properties"": {
                        ""id"": { ""type"": ""integer"", ""description"": ""ID de la persona a eliminar"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_get_stats",
                Description = "Calcula métricas y estadísticas de la base de datos (conteo total, edad promedio, mínimas y máximas).",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""properties"": {
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_execute_query",
                Description = "Ejecuta consultas SQL en PostgreSQL bajo estricta política de permisos (solo SELECT y comandos seguros, prohíbe DROP/TRUNCATE).",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""sql""],
                    ""properties"": {
                        ""sql"": { ""type"": ""string"", ""description"": ""Sentencia SQL a ejecutar"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            }
        };
    }

    public async Task<McpJsonRpcResponse<object>> ProcessRequestAsync(McpJsonRpcRequest request, CancellationToken ct)
    {
        _logger.LogInformation("MCP Request recibido: Método={Method}, Id={Id}", request.Method, request.Id);

        try
        {
            switch (request.Method)
            {
                case "initialize":
                    var initResult = new McpInitializeResult
                    {
                        ProtocolVersion = "2024-11-05",
                        ServerInfo = new McpServerInfo
                        {
                            Name = "Crud-PostgreSql-Mcp-Server",
                            Version = "1.0.0"
                        },
                        Capabilities = JsonDocument.Parse(@"{
                            ""tools"": { ""listChanged"": false },
                            ""logging"": {}
                        }").RootElement
                    };
                    return Success(request.Id, initResult);

                case "notifications/initialized":
                    return Success(request.Id, new { status = "ready" });

                case "tools/list":
                    var toolsList = new McpToolsListResult
                    {
                        Tools = GetRegisteredTools()
                    };
                    return Success(request.Id, toolsList);

                case "tools/call":
                    var callResult = await HandleToolCallAsync(request.Params, ct);
                    return Success(request.Id, callResult);

                default:
                    return Error(request.Id, -32601, $"Método no encontrado: '{request.Method}'");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar solicitud MCP");
            return Error(request.Id, -32603, $"Error interno MCP: {ex.Message}");
        }
    }

    private async Task<McpToolCallResult> HandleToolCallAsync(object? rawParams, CancellationToken ct)
    {
        if (rawParams == null)
        {
            return ToolError("Parámetros faltantes para tools/call.");
        }

        var json = JsonSerializer.Serialize(rawParams);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("name", out var nameProp))
        {
            return ToolError("El parámetro 'name' es obligatorio en tools/call.");
        }

        var toolName = nameProp.GetString() ?? string.Empty;
        var args = root.TryGetProperty("arguments", out var argsProp) ? argsProp : default;

        string target = "windows";
        if (args.ValueKind == JsonValueKind.Object && args.TryGetProperty("target", out var targetProp))
        {
            target = targetProp.GetString() ?? "windows";
        }

        try
        {
            switch (toolName)
            {
                case "sql_list_people":
                {
                    string? search = null;
                    int limit = 100;
                    if (args.ValueKind == JsonValueKind.Object)
                    {
                        if (args.TryGetProperty("search", out var sp)) search = sp.GetString();
                        if (args.TryGetProperty("limit", out var lp)) limit = lp.GetInt32();
                    }
                    var list = await _db.ListPeopleAsync(target, search, limit, ct);
                    return ToolSuccess(JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
                }

                case "sql_get_person":
                {
                    if (args.ValueKind != JsonValueKind.Object || !args.TryGetProperty("id", out var idProp))
                    {
                        return ToolError("Argumento 'id' requerido.");
                    }
                    var p = await _db.GetPersonByIdAsync(target, idProp.GetInt32(), ct);
                    return p != null 
                        ? ToolSuccess(JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true }))
                        : ToolError($"Persona con ID {idProp.GetInt32()} no encontrada.");
                }

                case "sql_create_person":
                {
                    if (args.ValueKind != JsonValueKind.Object ||
                        !args.TryGetProperty("name", out var np) ||
                        !args.TryGetProperty("age", out var ap))
                    {
                        return ToolError("Argumentos 'name' y 'age' requeridos.");
                    }
                    var created = await _db.CreatePersonAsync(target, np.GetString() ?? "", ap.GetInt32(), ct);
                    return ToolSuccess(JsonSerializer.Serialize(created, new JsonSerializerOptions { WriteIndented = true }));
                }

                case "sql_update_person":
                {
                    if (args.ValueKind != JsonValueKind.Object ||
                        !args.TryGetProperty("id", out var idProp) ||
                        !args.TryGetProperty("name", out var np) ||
                        !args.TryGetProperty("age", out var ap))
                    {
                        return ToolError("Argumentos 'id', 'name' y 'age' requeridos.");
                    }
                    var ok = await _db.UpdatePersonAsync(target, idProp.GetInt32(), np.GetString() ?? "", ap.GetInt32(), ct);
                    return ok 
                        ? ToolSuccess($"Persona ID {idProp.GetInt32()} actualizada exitosamente.")
                        : ToolError($"No se encontró la persona ID {idProp.GetInt32()} para actualizar.");
                }

                case "sql_delete_person":
                {
                    if (args.ValueKind != JsonValueKind.Object || !args.TryGetProperty("id", out var idProp))
                    {
                        return ToolError("Argumento 'id' requerido.");
                    }
                    var ok = await _db.DeletePersonAsync(target, idProp.GetInt32(), ct);
                    return ok 
                        ? ToolSuccess($"Persona ID {idProp.GetInt32()} eliminada exitosamente.")
                        : ToolError($"No se encontró la persona ID {idProp.GetInt32()} para eliminar.");
                }

                case "sql_get_stats":
                {
                    var stats = await _db.GetDatabaseStatsAsync(target, ct);
                    return ToolSuccess(JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true }));
                }

                case "sql_execute_query":
                {
                    if (args.ValueKind != JsonValueKind.Object || !args.TryGetProperty("sql", out var sqlProp))
                    {
                        return ToolError("Argumento 'sql' requerido.");
                    }
                    var queryOutput = await _db.ExecuteQueryAsync(target, sqlProp.GetString() ?? "", ct);
                    return ToolSuccess(queryOutput);
                }

                default:
                    return ToolError($"Herramienta desconocida: '{toolName}'.");
            }
        }
        catch (Exception ex)
        {
            return ToolError($"Fallo al ejecutar herramienta '{toolName}': {ex.Message}");
        }
    }

    private static McpToolCallResult ToolSuccess(string text) => new()
    {
        IsError = false,
        Content = new List<McpContentItem> { new() { Type = "text", Text = text } }
    };

    private static McpToolCallResult ToolError(string error) => new()
    {
        IsError = true,
        Content = new List<McpContentItem> { new() { Type = "text", Text = error } }
    };

    private static McpJsonRpcResponse<object> Success(object? id, object result) => new()
    {
        Id = id,
        Result = result
    };

    private static McpJsonRpcResponse<object> Error(object? id, int code, string message) => new()
    {
        Id = id,
        Error = new McpJsonRpcError { Code = code, Message = message }
    };
}
