using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Mcp.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class McpController : ControllerBase
{
    private readonly McpAiOrchestratorService _orchestrator;
    private readonly AiUsageTrackerService _usageTracker;
    private readonly IMcpClient _mcpClient;

    public McpController(
        McpAiOrchestratorService orchestrator,
        AiUsageTrackerService usageTracker,
        IMcpClient mcpClient)
    {
        _orchestrator = orchestrator;
        _usageTracker = usageTracker;
        _mcpClient = mcpClient;
    }

    /// <summary>
    /// Procesa una instrucción de lenguaje natural conectando Gemini con herramientas MCP.
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<McpQueryResponse>> Query([FromBody] McpQueryRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest(new McpQueryResponse
            {
                IsSuccess = false,
                Reply = "La consulta no puede estar vacía.",
                ErrorMessage = "El prompt es obligatorio."
            });
        }

        var result = await _orchestrator.ProcessQueryAsync(request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Consulta el estado del servidor MCP y las métricas de consumo de tokens en tiempo real.
    /// </summary>
    [HttpGet("usage")]
    public async Task<ActionResult<AiUsageStatusDto>> GetUsage([FromQuery] string provider = "gemini", CancellationToken ct = default)
    {
        var isOnline = await _mcpClient.PingAsync(ct);
        var status = _usageTracker.GetStatus(provider, isOnline);
        return Ok(status);
    }
}
