using System;
using Mcp.Client.Interfaces;
using Mcp.Client.Models;
using Mcp.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mcp.Client.Extensions;

// ServiceCollectionExtensions
// Métodos de extensión para registrar la Capa 3 (Cliente MCP) en el contenedor IoC de .NET.
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMcpClient(
        this IServiceCollection services,
        Action<McpClientOptions>? configureOptions = null)
    {
        var options = new McpClientOptions();
        configureOptions?.Invoke(options);

        services.AddSingleton(options);

        services.AddHttpClient<IMcpClient, McpHttpClient>(client =>
        {
            if (!string.IsNullOrWhiteSpace(options.ServerUrl))
            {
                client.BaseAddress = new Uri(options.ServerUrl.EndsWith('/') ? options.ServerUrl : options.ServerUrl + "/");
            }
            if (options.Timeout > TimeSpan.Zero)
            {
                client.Timeout = options.Timeout;
            }
        });

        return services;
    }
}
