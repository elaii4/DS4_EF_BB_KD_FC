using System;
using System.Collections.Generic;

namespace Api.Models;

public class McpQueryRequest
{
    public string Prompt { get; set; } = string.Empty;
    public string Provider { get; set; } = "gemini";
    public string Target { get; set; } = "windows";
}

public class McpQueryResponse
{
    public string Reply { get; set; } = string.Empty;
    public string Provider { get; set; } = "gemini";
    public string Model { get; set; } = string.Empty;
    public List<string> ToolsExecuted { get; set; } = new();
    public int TokensUsed { get; set; }
    public bool DatabaseModified { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string? ErrorMessage { get; set; }
}

public class AiUsageStatusDto
{
    public bool McpServerOnline { get; set; }
    public string Provider { get; set; } = "gemini";
    public string DisplayName { get; set; } = "Google Gemini";
    public string Model { get; set; } = "gemini-3.5-flash";
    public int UsedTokens { get; set; }
    public int TokenBudget { get; set; } = 100000;
    public int AvailableTokens { get; set; }
    public double UsagePercentage { get; set; }
    public int TotalQueries { get; set; }
    public bool IsConfigured { get; set; }
    public DateTime? LastQueryAt { get; set; }
}

public class McpUsageSummaryDto
{
    public bool McpServerOnline { get; set; }
    public string Provider { get; set; } = "gemini";
    public string Model { get; set; } = "gemini-3.5-flash";
    public int UsedTokens { get; set; }
    public int TokenBudget { get; set; } = 100000;
    public int AvailableTokens { get; set; }
    public double UsagePercentage { get; set; }
    public int TotalQueries { get; set; }
    public bool IsConfigured { get; set; }
    public DateTime? LastQueryAt { get; set; }
    public List<AiUsageStatusDto> Providers { get; set; } = new();
}
