using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Mcp.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Mcp.Server.Services;

// PostgresToolService
// Implementa el acceso a PostgreSQL (Capa 5) mediante el driver oficial Npgsql.
// Proporciona validación de seguridad de permisos y ejecución de consultas parametrizadas.
public class PostgresToolService : IPostgresToolService
{
    private readonly IConfiguration _config;
    private readonly ILogger<PostgresToolService> _logger;

    // Palabras reservadas peligrosas restringidas por la política de permisos del servidor MCP
    private static readonly string[] DangerousSqlTokens =
    {
        "DROP", "TRUNCATE", "ALTER", "GRANT", "REVOKE", "CREATE USER", "DROP DATABASE"
    };

    public PostgresToolService(IConfiguration config, ILogger<PostgresToolService> logger)
    {
        _config = config;
        _logger = logger;
    }

    private string GetConnectionString(string target)
    {
        var isWindows = target.Equals("windows", StringComparison.OrdinalIgnoreCase);
        if (isWindows)
        {
            return _config.GetConnectionString("Windows")
                ?? _config.GetConnectionString("Default")
                ?? "Host=127.0.0.1;Port=5434;Database=crud_windows_forms_win;Username=crud_app;Password=crud_secret_dev_pass";
        }
        else
        {
            return _config.GetConnectionString("Ubuntu")
                ?? "Host=localhost;Port=5432;Database=crud_windows_forms;Username=crud_app;Password=crud_secret_dev_pass";
        }
    }

    public async Task<List<McpPersonDto>> ListPeopleAsync(string target, string? search, int limit = 100, CancellationToken ct = default)
    {
        var connStr = GetConnectionString(target);
        var list = new List<McpPersonDto>();

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        var sql = "SELECT id, name, age FROM people";
        if (!string.IsNullOrWhiteSpace(search))
        {
            sql += " WHERE name ILIKE @search";
        }
        sql += " ORDER BY id ASC LIMIT @limit";

        await using var cmd = new NpgsqlCommand(sql, conn);
        if (!string.IsNullOrWhiteSpace(search))
        {
            cmd.Parameters.AddWithValue("@search", $"%{search.Trim()}%");
        }
        cmd.Parameters.AddWithValue("@limit", Math.Clamp(limit, 1, 500));

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new McpPersonDto
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Age = reader.GetInt32(2)
            });
        }

        return list;
    }

    public async Task<McpPersonDto?> GetPersonByIdAsync(string target, int id, CancellationToken ct = default)
    {
        var connStr = GetConnectionString(target);
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        const string sql = "SELECT id, name, age FROM people WHERE id = @id LIMIT 1";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new McpPersonDto
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Age = reader.GetInt32(2)
            };
        }

        return null;
    }

    public async Task<McpPersonDto> CreatePersonAsync(string target, string name, int age, CancellationToken ct = default)
    {
        var connStr = GetConnectionString(target);
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        const string sql = "INSERT INTO people (name, age) VALUES (@name, @age) RETURNING id, name, age";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", name.Trim());
        cmd.Parameters.AddWithValue("@age", age);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new McpPersonDto
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Age = reader.GetInt32(2)
            };
        }

        throw new InvalidOperationException("No se pudo obtener el registro recién creado.");
    }

    public async Task<bool> UpdatePersonAsync(string target, int id, string name, int age, CancellationToken ct = default)
    {
        var connStr = GetConnectionString(target);
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        const string sql = "UPDATE people SET name = @name, age = @age WHERE id = @id";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@name", name.Trim());
        cmd.Parameters.AddWithValue("@age", age);

        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows > 0;
    }

    public async Task<bool> DeletePersonAsync(string target, int id, CancellationToken ct = default)
    {
        var connStr = GetConnectionString(target);
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        const string sql = "DELETE FROM people WHERE id = @id";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows > 0;
    }

    public async Task<McpDatabaseStatsDto> GetDatabaseStatsAsync(string target, CancellationToken ct = default)
    {
        var connStr = GetConnectionString(target);
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        const string sql = @"
            SELECT 
                COUNT(*) AS total,
                COALESCE(AVG(age), 0) AS avg_age,
                COALESCE(MIN(age), 0) AS min_age,
                COALESCE(MAX(age), 0) AS max_age
            FROM people";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);

        if (await reader.ReadAsync(ct))
        {
            var isWin = target.Equals("windows", StringComparison.OrdinalIgnoreCase);
            return new McpDatabaseStatsDto
            {
                TotalRecords = Convert.ToInt32(reader["total"]),
                AverageAge = Math.Round(Convert.ToDouble(reader["avg_age"]), 2),
                MinAge = Convert.ToInt32(reader["min_age"]),
                MaxAge = Convert.ToInt32(reader["max_age"]),
                TargetServer = isWin ? "Windows Server (Local)" : "Ubuntu Server (AWS Cloud)",
                DatabaseName = isWin ? "crud_windows_forms_win" : "crud_windows_forms",
                Timestamp = DateTime.UtcNow
            };
        }

        return new McpDatabaseStatsDto();
    }

    public async Task<string> ExecuteQueryAsync(string target, string sqlQuery, CancellationToken ct = default)
    {
        // 1. Control de Permisos de Seguridad
        var upper = sqlQuery.ToUpperInvariant();
        foreach (var danger in DangerousSqlTokens)
        {
            if (upper.Contains(danger))
            {
                _logger.LogWarning("Intento de ejecución de comando no autorizado en MCP: {Sql}", sqlQuery);
                throw new UnauthorizedAccessException($"Permiso denegado por política de seguridad MCP: El comando contiene '{danger}'. Solo se permiten consultas de lectura y manipulación segura.");
            }
        }

        var connStr = GetConnectionString(target);
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand(sqlQuery, conn);

        if (upper.TrimStart().StartsWith("SELECT"))
        {
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            var results = new List<Dictionary<string, object?>>();

            while (await reader.ReadAsync(ct))
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                results.Add(row);
            }

            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        else
        {
            var affected = await cmd.ExecuteNonQueryAsync(ct);
            return JsonSerializer.Serialize(new { rowsAffected = affected, success = true });
        }
    }
}
