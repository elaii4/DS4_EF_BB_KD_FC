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
                Description = "Consulta la lista de personas desde PostgreSQL. Permite filtrar por nombre opcionalmente.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""properties"": {
                        ""filtroNombre"": { ""type"": ""string"", ""description"": ""Texto opcional para filtrar por nombre de persona"" },
                        ""search"": { ""type"": ""string"", ""description"": ""Alias opcional de filtroNombre para buscar coincidencias"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" },
                        ""limit"": { ""type"": ""integer"", ""description"": ""Límite máximo de registros a retornar (por defecto 100)"", ""default"": 100 }
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
                        ""id"": { ""type"": ""integer"", ""description"": ""ID único de la persona (> 0)"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_create_person",
                Description = "Crea una nueva persona en la base de datos PostgreSQL y devuelve el registro creado con su ID.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""nombre"", ""edad""],
                    ""properties"": {
                        ""nombre"": { ""type"": ""string"", ""description"": ""Nombre completo de la persona (obligatorio, no vacío)"" },
                        ""edad"": { ""type"": ""integer"", ""description"": ""Edad de la persona (entero entre 1 y 120)"" },
                        ""name"": { ""type"": ""string"", ""description"": ""Alias de nombre"" },
                        ""age"": { ""type"": ""integer"", ""description"": ""Alias de edad"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_update_person",
                Description = "Actualiza los datos (nombre y/o edad) de una persona por su ID en PostgreSQL y devuelve el registro actualizado.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""id"", ""nombre"", ""edad""],
                    ""properties"": {
                        ""id"": { ""type"": ""integer"", ""description"": ""ID único de la persona a modificar (> 0)"" },
                        ""nombre"": { ""type"": ""string"", ""description"": ""Nuevo nombre completo (no vacío)"" },
                        ""edad"": { ""type"": ""integer"", ""description"": ""Nueva edad (entero entre 1 y 120)"" },
                        ""name"": { ""type"": ""string"", ""description"": ""Alias de nombre"" },
                        ""age"": { ""type"": ""integer"", ""description"": ""Alias de edad"" },
                        ""target"": { ""type"": ""string"", ""description"": ""Servidor destino ('windows' o 'ubuntu')"", ""default"": ""windows"" }
                    }
                }").RootElement
            },
            new()
            {
                Name = "sql_delete_person",
                Description = "Elimina de forma permanente una persona por su ID y devuelve confirmación con las filas afectadas.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""id""],
                    ""properties"": {
                        ""id"": { ""type"": ""integer"", ""description"": ""ID único de la persona a eliminar (> 0)"" },
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
                Description = "Ejecuta consultas de solo lectura (SELECT) en PostgreSQL. Prohíbe terminantemente DROP, TRUNCATE, ALTER, INSERT, UPDATE y DELETE.",
                InputSchema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""required"": [""sql""],
                    ""properties"": {
                        ""sql"": { ""type"": ""string"", ""description"": ""Sentencia SQL de solo lectura (debe iniciar con SELECT)"" },
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
                        if (args.TryGetProperty("filtroNombre", out var fp) && fp.ValueKind == JsonValueKind.String) search = fp.GetString();
                        else if (args.TryGetProperty("search", out var sp) && sp.ValueKind == JsonValueKind.String) search = sp.GetString();

                        if (args.TryGetProperty("limit", out var lp) && lp.ValueKind == JsonValueKind.Number) limit = lp.GetInt32();
                    }
                    var list = await _db.ListPeopleAsync(target, search, limit, ct);
                    return ToolSuccess(JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
                }

                case "sql_get_person":
                {
                    if (args.ValueKind != JsonValueKind.Object || !args.TryGetProperty("id", out var idProp) || idProp.ValueKind != JsonValueKind.Number)
                    {
                        return ToolError("El parámetro 'id' es requerido y debe ser numérico.");
                    }
                    var id = idProp.GetInt32();
                    if (id <= 0) return ToolError("El ID debe ser mayor a 0.");

                    var p = await _db.GetPersonByIdAsync(target, id, ct);
                    return p != null 
                        ? ToolSuccess(JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true }))
                        : ToolError($"Persona con ID {id} no encontrada.");
                }

                case "sql_create_person":
                {
                    string? nombre = null;
                    int? edad = null;

                    if (args.ValueKind == JsonValueKind.Object)
                    {
                        if (args.TryGetProperty("nombre", out var np) && np.ValueKind == JsonValueKind.String) nombre = np.GetString();
                        else if (args.TryGetProperty("name", out var nnamep) && nnamep.ValueKind == JsonValueKind.String) nombre = nnamep.GetString();

                        if (args.TryGetProperty("edad", out var ep) && ep.ValueKind == JsonValueKind.Number) edad = ep.GetInt32();
                        else if (args.TryGetProperty("age", out var eagep) && eagep.ValueKind == JsonValueKind.Number) edad = eagep.GetInt32();
                    }

                    if (string.IsNullOrWhiteSpace(nombre))
                    {
                        return ToolError("El parámetro 'nombre' es obligatorio y no puede estar vacío.");
                    }
                    if (!edad.HasValue || edad.Value < 1 || edad.Value > 120)
                    {
                        return ToolError("El parámetro 'edad' debe ser un número entero entre 1 y 120.");
                    }

                    var created = await _db.CreatePersonAsync(target, nombre, edad.Value, ct);
                    return ToolSuccess(JsonSerializer.Serialize(created, new JsonSerializerOptions { WriteIndented = true }));
                }

                case "sql_update_person":
                {
                    int? id = null;
                    string? nombre = null;
                    int? edad = null;

                    if (args.ValueKind == JsonValueKind.Object)
                    {
                        if (args.TryGetProperty("id", out var ip) && ip.ValueKind == JsonValueKind.Number) id = ip.GetInt32();
                        
                        if (args.TryGetProperty("nombre", out var np) && np.ValueKind == JsonValueKind.String) nombre = np.GetString();
                        else if (args.TryGetProperty("name", out var nnamep) && nnamep.ValueKind == JsonValueKind.String) nombre = nnamep.GetString();

                        if (args.TryGetProperty("edad", out var ep) && ep.ValueKind == JsonValueKind.Number) edad = ep.GetInt32();
                        else if (args.TryGetProperty("age", out var eagep) && eagep.ValueKind == JsonValueKind.Number) edad = eagep.GetInt32();
                    }

                    if (!id.HasValue || id.Value <= 0)
                    {
                        return ToolError("El parámetro 'id' es obligatorio y debe ser mayor a 0.");
                    }
                    if (string.IsNullOrWhiteSpace(nombre))
                    {
                        return ToolError("El parámetro 'nombre' es obligatorio y no puede estar vacío.");
                    }
                    if (!edad.HasValue || edad.Value < 1 || edad.Value > 120)
                    {
                        return ToolError("El parámetro 'edad' debe ser un número entero entre 1 y 120.");
                    }

                    var updated = await _db.UpdatePersonAsync(target, id.Value, nombre, edad.Value, ct);
                    return updated != null 
                        ? ToolSuccess(JsonSerializer.Serialize(updated, new JsonSerializerOptions { WriteIndented = true }))
                        : ToolError($"No se encontró la persona con ID {id.Value} para actualizar.");
                }

                case "sql_delete_person":
                {
                    if (args.ValueKind != JsonValueKind.Object || !args.TryGetProperty("id", out var idProp) || idProp.ValueKind != JsonValueKind.Number)
                    {
                        return ToolError("El parámetro 'id' es requerido y debe ser numérico.");
                    }
                    var id = idProp.GetInt32();
                    if (id <= 0) return ToolError("El ID debe ser mayor a 0.");

                    var rows = await _db.DeletePersonAsync(target, id, ct);
                    return rows > 0 
                        ? ToolSuccess(JsonSerializer.Serialize(new { success = true, id = id, rowsAffected = rows, message = $"Persona con ID {id} eliminada exitosamente. Filas afectadas: {rows}." }, new JsonSerializerOptions { WriteIndented = true }))
                        : ToolError($"No se encontró la persona con ID {id} para eliminar. Filas afectadas: 0.");
                }

                case "sql_get_stats":
                {
                    var stats = await _db.GetDatabaseStatsAsync(target, ct);
                    return ToolSuccess(JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true }));
                }

                case "sql_execute_query":
                {
                    if (args.ValueKind != JsonValueKind.Object || !args.TryGetProperty("sql", out var sqlProp) || sqlProp.ValueKind != JsonValueKind.String)
                    {
                        return ToolError("El parámetro 'sql' es requerido.");
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
