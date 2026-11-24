using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Mcp.Client.Interfaces;
using Mcp.Client.Models;

namespace Mcp.Client.Services;

// McpHttpClient
// Implementación cliente de la especificación Model Context Protocol sobre transporte HTTP / JSON-RPC 2.0.
public class McpHttpClient : IMcpClient
{
    private readonly HttpClient _http;
    private readonly McpClientOptions _options;
    private int _requestId = 0;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public McpHttpClient(HttpClient http, McpClientOptions? options = null)
    {
        _http = http;
        _options = options ?? new McpClientOptions();

        if (_http.BaseAddress == null && !string.IsNullOrWhiteSpace(_options.ServerUrl))
        {
            _http.BaseAddress = new Uri(_options.ServerUrl);
        }
        if (_options.Timeout > TimeSpan.Zero)
        {
            _http.Timeout = _options.Timeout;
        }
    }

    public async Task<McpInitializeResult?> InitializeAsync(CancellationToken cancellationToken = default)
    {
        var request = new McpJsonRpcRequest
        {
            Id = Interlocked.Increment(ref _requestId),
            Method = "initialize",
            Params = new
            {
                protocolVersion = "2024-11-05",
                capabilities = new { tools = new { } },
                clientInfo = new { name = "DotNet-McpClient", version = "1.0.0" }
            }
        };

        var response = await SendJsonRpcAsync<McpInitializeResult>(request, cancellationToken);
        return response?.Result;
    }

    public async Task<IReadOnlyList<McpToolDefinition>> ListToolsAsync(CancellationToken cancellationToken = default)
    {
        var request = new McpJsonRpcRequest
        {
            Id = Interlocked.Increment(ref _requestId),
            Method = "tools/list",
            Params = new { }
        };

        var response = await SendJsonRpcAsync<McpToolsListResult>(request, cancellationToken);
        return response?.Result?.Tools ?? (IReadOnlyList<McpToolDefinition>)Array.Empty<McpToolDefinition>();
    }

    public async Task<McpToolCallResult> CallToolAsync(string toolName, Dictionary<string, object?> arguments, CancellationToken cancellationToken = default)
    {
        var request = new McpJsonRpcRequest
        {
            Id = Interlocked.Increment(ref _requestId),
            Method = "tools/call",
            Params = new
            {
                name = toolName,
                arguments = arguments
            }
        };

        var response = await SendJsonRpcAsync<McpToolCallResult>(request, cancellationToken);
        if (response?.Error != null)
        {
            return new McpToolCallResult
            {
                IsError = true,
                Content = new List<McpContentItem>
                {
                    new() { Type = "text", Text = $"Error {response.Error.Code}: {response.Error.Message}" }
                }
            };
        }

        return response?.Result ?? new McpToolCallResult
        {
            IsError = true,
            Content = new List<McpContentItem>
            {
                new() { Type = "text", Text = "No se recibió respuesta del servidor MCP." }
            }
        };
    }

    public async Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.GetAsync("health", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // --- OPERACIONES SQL TIPADAS DE ALTO NIVEL ---

    public async Task<List<McpPersonDto>> ListPeopleAsync(string target = "windows", string? search = null, CancellationToken cancellationToken = default)
    {
        var args = new Dictionary<string, object?>
        {
            ["target"] = target
        };
        if (!string.IsNullOrWhiteSpace(search))
        {
            args["search"] = search;
        }

        var result = await CallToolAsync("sql_list_people", args, cancellationToken);
        if (result.IsError) return new List<McpPersonDto>();

        try
        {
            var text = result.GetText();
            return JsonSerializer.Deserialize<List<McpPersonDto>>(text, JsonOptions) ?? new List<McpPersonDto>();
        }
        catch
        {
            return new List<McpPersonDto>();
        }
    }

    public async Task<McpPersonDto?> CreatePersonAsync(string name, int age, string target = "windows", CancellationToken cancellationToken = default)
    {
        var args = new Dictionary<string, object?>
        {
            ["name"] = name,
            ["age"] = age,
            ["target"] = target
        };

        var result = await CallToolAsync("sql_create_person", args, cancellationToken);
        if (result.IsError) return null;

        try
        {
            var text = result.GetText();
            return JsonSerializer.Deserialize<McpPersonDto>(text, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdatePersonAsync(int id, string name, int age, string target = "windows", CancellationToken cancellationToken = default)
    {
        var args = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["name"] = name,
            ["age"] = age,
            ["target"] = target
        };

        var result = await CallToolAsync("sql_update_person", args, cancellationToken);
        return !result.IsError;
    }

    public async Task<bool> DeletePersonAsync(int id, string target = "windows", CancellationToken cancellationToken = default)
    {
        var args = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["target"] = target
        };

        var result = await CallToolAsync("sql_delete_person", args, cancellationToken);
        return !result.IsError;
    }

    public async Task<McpDatabaseStatsDto?> GetStatsAsync(string target = "windows", CancellationToken cancellationToken = default)
    {
        var args = new Dictionary<string, object?>
        {
            ["target"] = target
        };

        var result = await CallToolAsync("sql_get_stats", args, cancellationToken);
        if (result.IsError) return null;

        try
        {
            var text = result.GetText();
            return JsonSerializer.Deserialize<McpDatabaseStatsDto>(text, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private async Task<McpJsonRpcResponse<T>?> SendJsonRpcAsync<T>(McpJsonRpcRequest request, CancellationToken cancellationToken)
    {
        var response = await _http.PostAsJsonAsync("mcp", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<McpJsonRpcResponse<T>>(JsonOptions, cancellationToken);
    }
}
