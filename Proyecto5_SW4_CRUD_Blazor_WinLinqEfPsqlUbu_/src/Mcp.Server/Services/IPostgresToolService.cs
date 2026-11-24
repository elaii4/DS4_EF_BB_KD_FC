using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mcp.Client.Models;

namespace Mcp.Server.Services;

// IPostgresToolService
// Contrato de la Capa 4 (MCP Server) para ejecutar operaciones SQL sobre PostgreSQL (Capa 5).
// Maneja herramientas SQL, validación de permisos y ejecución con Npgsql.
public interface IPostgresToolService
{
    Task<List<McpPersonDto>> ListPeopleAsync(string target, string? search, int limit = 100, CancellationToken ct = default);
    Task<McpPersonDto?> GetPersonByIdAsync(string target, int id, CancellationToken ct = default);
    Task<McpPersonDto> CreatePersonAsync(string target, string name, int age, CancellationToken ct = default);
    Task<bool> UpdatePersonAsync(string target, int id, string name, int age, CancellationToken ct = default);
    Task<bool> DeletePersonAsync(string target, int id, CancellationToken ct = default);
    Task<McpDatabaseStatsDto> GetDatabaseStatsAsync(string target, CancellationToken ct = default);
    Task<string> ExecuteQueryAsync(string target, string sqlQuery, CancellationToken ct = default);
}
