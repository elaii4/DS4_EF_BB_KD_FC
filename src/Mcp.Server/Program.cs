using System;
using System.Threading;
using Mcp.Client.Models;
using Mcp.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuración de URL predeterminada en puerto 5145 si no se especifica otra
builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:5145");

// Servicios de la Capa 4 (MCP Server)
builder.Services.AddScoped<IPostgresToolService, PostgresToolService>();
builder.Services.AddScoped<McpProtocolService>();

// Habilitar CORS para permitir invocaciones desde Blazor / API local y túneles
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

// Endpoint de estado y salud
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    layer = "CAPA 4 · SERVIDOR MCP",
    server = "PostgreSQL Model Context Protocol Server (.NET 8)",
    protocolVersion = "2024-11-05",
    timestamp = DateTime.UtcNow
}));

// Inspección rápida del catálogo de herramientas disponibles
app.MapGet("/tools", (McpProtocolService protocol) =>
{
    return Results.Ok(protocol.GetRegisteredTools());
});

// Endpoint principal MCP (JSON-RPC 2.0 sobre HTTP)
app.MapPost("/mcp", async (McpJsonRpcRequest request, McpProtocolService protocol, CancellationToken ct) =>
{
    var response = await protocol.ProcessRequestAsync(request, ct);
    return Results.Ok(response);
});

// Mensaje de bienvenida
app.MapGet("/", () => Results.Content(@"
<html>
<head><title>Servidor MCP - Capa 4</title></head>
<body style='font-family: sans-serif; padding: 2rem; background: #0f172a; color: #f8fafc;'>
    <h1>🚀 Servidor MCP Activo (Capa 4)</h1>
    <p>Servidor compatible con Model Context Protocol (MCP) y PostgreSQL (Capa 5).</p>
    <ul>
        <li><strong>Endpoint JSON-RPC MCP:</strong> <code>POST /mcp</code></li>
        <li><strong>Catálogo de Tools:</strong> <a href='/tools' style='color: #38bdf8;'>GET /tools</a></li>
        <li><strong>Salud:</strong> <a href='/health' style='color: #38bdf8;'>GET /health</a></li>
    </ul>
</body>
</html>", "text/html"));

app.Run();
