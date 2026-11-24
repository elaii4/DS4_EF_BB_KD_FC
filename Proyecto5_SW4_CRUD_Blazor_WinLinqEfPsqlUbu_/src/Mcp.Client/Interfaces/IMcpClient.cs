using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mcp.Client.Models;

namespace Mcp.Client.Interfaces;

// IMcpClient
// Contrato de integración para la Capa 3 (Cliente MCP .NET).
// Permite que la Capa 2 (ASP.NET Core Web API) u otros consumidores invoquen
// herramientas en el servidor MCP de forma tipada y desacoplada.
public interface IMcpClient
{
    // Realiza el handshake de inicialización con el servidor MCP
    Task<McpInitializeResult?> InitializeAsync(CancellationToken cancellationToken = default);

    // Consulta el catálogo de herramientas registradas en el servidor MCP
    Task<IReadOnlyList<McpToolDefinition>> ListToolsAsync(CancellationToken cancellationToken = default);

    // Invoca una herramienta MCP genérica pasando nombre y argumentos
    Task<McpToolCallResult> CallToolAsync(string toolName, Dictionary<string, object?> arguments, CancellationToken cancellationToken = default);

    // Comprueba si el servidor MCP está respondiendo
    Task<bool> PingAsync(CancellationToken cancellationToken = default);

    // --- MÉTODOS DE ALTO NIVEL TIPADOS PARA OPERACIONES SQL ---
    Task<List<McpPersonDto>> ListPeopleAsync(string target = "windows", string? search = null, CancellationToken cancellationToken = default);
    Task<McpPersonDto?> CreatePersonAsync(string name, int age, string target = "windows", CancellationToken cancellationToken = default);
    Task<bool> UpdatePersonAsync(int id, string name, int age, string target = "windows", CancellationToken cancellationToken = default);
    Task<bool> DeletePersonAsync(int id, string target = "windows", CancellationToken cancellationToken = default);
    Task<McpDatabaseStatsDto?> GetStatsAsync(string target = "windows", CancellationToken cancellationToken = default);
}
