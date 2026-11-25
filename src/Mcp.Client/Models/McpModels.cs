using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mcp.Client.Models;

// McpJsonRpcRequest
// Estructura base para mensajes JSON-RPC 2.0 según la especificación Model Context Protocol (MCP).
public class McpJsonRpcRequest
{
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; set; } = "2.0";

    [JsonPropertyName("id")]
    public object Id { get; set; } = 1;

    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;

    [JsonPropertyName("params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Params { get; set; }
}

// McpJsonRpcResponse
// Respuesta estándar JSON-RPC 2.0 emitida por el servidor MCP.
public class McpJsonRpcResponse<T>
{
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; set; } = "2.0";

    [JsonPropertyName("id")]
    public object? Id { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }

    [JsonPropertyName("error")]
    public McpJsonRpcError? Error { get; set; }
}

public class McpJsonRpcError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public object? Data { get; set; }
}

// McpToolDefinition
// Metadatos y firma de una herramienta registrada en el servidor MCP.
public class McpToolDefinition
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("inputSchema")]
    public JsonElement InputSchema { get; set; }
}

public class McpToolsListResult
{
    [JsonPropertyName("tools")]
    public List<McpToolDefinition> Tools { get; set; } = new();
}

public class McpInitializeResult
{
    [JsonPropertyName("protocolVersion")]
    public string ProtocolVersion { get; set; } = string.Empty;

    [JsonPropertyName("capabilities")]
    public JsonElement Capabilities { get; set; }

    [JsonPropertyName("serverInfo")]
    public McpServerInfo ServerInfo { get; set; } = new();
}

public class McpServerInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

public class McpToolCallResult
{
    [JsonPropertyName("content")]
    public List<McpContentItem> Content { get; set; } = new();

    [JsonPropertyName("isError")]
    public bool IsError { get; set; }

    public string GetText()
    {
        if (Content == null || Content.Count == 0) return string.Empty;
        var list = new List<string>();
        foreach (var c in Content)
        {
            if (c.Type == "text" && !string.IsNullOrEmpty(c.Text))
            {
                list.Add(c.Text);
            }
        }
        return string.Join("\n", list);
    }
}

public class McpContentItem
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class McpClientOptions
{
    public string ServerUrl { get; set; } = "http://localhost:5145/";
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
}

// DTOs auxiliares para tipado fuerte al consumir herramientas SQL
public class McpPersonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class McpDatabaseStatsDto
{
    public int TotalRecords { get; set; }
    public double AverageAge { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public string TargetServer { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
