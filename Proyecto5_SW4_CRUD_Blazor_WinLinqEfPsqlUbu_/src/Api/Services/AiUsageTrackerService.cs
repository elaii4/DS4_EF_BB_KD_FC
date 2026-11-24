using System;
using System.Collections.Generic;
using Api.Models;
using Microsoft.Extensions.Configuration;

namespace Api.Services;

// AiUsageTrackerService
// Servicio singleton thread-safe que gestiona y contabiliza en memoria
// el consumo real de tokens de forma independiente para Gemini, Qwen 3.8 y GPT-OSS 120B.
public class AiUsageTrackerService
{
    private class ProviderStats
    {
        public int UsedTokens;
        public int TotalQueries;
        public DateTime? LastQueryAt;
    }

    private readonly object _lock = new();
    private readonly IConfiguration _config;
    private readonly Dictionary<string, ProviderStats> _stats = new(StringComparer.OrdinalIgnoreCase);

    public AiUsageTrackerService(IConfiguration config)
    {
        _config = config;
        _stats["gemini"] = new ProviderStats();
        _stats["qwen"] = new ProviderStats();
        _stats["gpt-oss"] = new ProviderStats();
    }

    public void RegisterUsage(string provider, int tokens)
    {
        if (tokens <= 0) return;
        var key = NormalizeKey(provider);

        lock (_lock)
        {
            if (!_stats.TryGetValue(key, out var stat))
            {
                stat = new ProviderStats();
                _stats[key] = stat;
            }

            stat.UsedTokens += tokens;
            stat.TotalQueries++;
            stat.LastQueryAt = DateTime.UtcNow;
        }
    }

    public AiUsageStatusDto GetStatus(string provider = "gemini", bool mcpServerOnline = false)
    {
        var key = NormalizeKey(provider);
        return BuildStatusDto(key, mcpServerOnline);
    }

    public McpUsageSummaryDto GetSummary(bool mcpServerOnline = false, string activeProvider = "gemini")
    {
        var activeKey = NormalizeKey(activeProvider);
        var activeStatus = BuildStatusDto(activeKey, mcpServerOnline);

        var list = new List<AiUsageStatusDto>
        {
            BuildStatusDto("gemini", mcpServerOnline),
            BuildStatusDto("qwen", mcpServerOnline),
            BuildStatusDto("gpt-oss", mcpServerOnline)
        };

        return new McpUsageSummaryDto
        {
            McpServerOnline = mcpServerOnline,
            Provider = activeStatus.Provider,
            Model = activeStatus.Model,
            UsedTokens = activeStatus.UsedTokens,
            TokenBudget = activeStatus.TokenBudget,
            AvailableTokens = activeStatus.AvailableTokens,
            UsagePercentage = activeStatus.UsagePercentage,
            TotalQueries = activeStatus.TotalQueries,
            IsConfigured = activeStatus.IsConfigured,
            LastQueryAt = activeStatus.LastQueryAt,
            Providers = list
        };
    }

    private AiUsageStatusDto BuildStatusDto(string key, bool mcpServerOnline)
    {
        string displayName;
        string model;
        int budget;
        bool isConfigured;

        if (key == "qwen")
        {
            var sec = _config.GetSection("AiProviders:qwen");
            displayName = sec["Name"] ?? "Qwen 3.8";
            model = sec["Model"] ?? "qwen/qwen3.8-27b";
            budget = int.TryParse(sec["TokenBudget"], out var b) ? b : 200000;
            var apiKey = _config["AiProviders:Groq:ApiKey"] 
                ?? Environment.GetEnvironmentVariable("AiProviders__Groq__ApiKey") 
                ?? Environment.GetEnvironmentVariable("GROQ_API_KEY");
            isConfigured = !string.IsNullOrWhiteSpace(apiKey);
        }
        else if (key == "gpt-oss")
        {
            var sec = _config.GetSection("AiProviders:gpt-oss");
            displayName = sec["Name"] ?? "GPT-OSS 120B";
            model = sec["Model"] ?? "openai/gpt-oss-120b";
            budget = int.TryParse(sec["TokenBudget"], out var b) ? b : 200000;
            var apiKey = _config["AiProviders:Groq:ApiKey"] 
                ?? Environment.GetEnvironmentVariable("AiProviders__Groq__ApiKey") 
                ?? Environment.GetEnvironmentVariable("GROQ_API_KEY");
            isConfigured = !string.IsNullOrWhiteSpace(apiKey);
        }
        else
        {
            var sec = _config.GetSection("AiProviders:Gemini");
            displayName = sec["Name"] ?? "Google Gemini";
            model = sec["Model"] ?? "gemini-3.5-flash";
            budget = int.TryParse(sec["TokenBudget"], out var b) ? b : 100000;
            var apiKey = sec["ApiKey"] 
                ?? _config["AiProviders:Gemini:ApiKey"] 
                ?? Environment.GetEnvironmentVariable("AiProviders__Gemini__ApiKey") 
                ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            isConfigured = !string.IsNullOrWhiteSpace(apiKey);
        }

        lock (_lock)
        {
            if (!_stats.TryGetValue(key, out var stat))
            {
                stat = new ProviderStats();
                _stats[key] = stat;
            }

            var available = Math.Max(0, budget - stat.UsedTokens);
            var percentage = budget > 0 ? Math.Round((double)stat.UsedTokens / budget * 100.0, 2) : 0.0;

            return new AiUsageStatusDto
            {
                McpServerOnline = mcpServerOnline,
                Provider = key,
                DisplayName = displayName,
                Model = model,
                UsedTokens = stat.UsedTokens,
                TokenBudget = budget,
                AvailableTokens = available,
                UsagePercentage = percentage,
                TotalQueries = stat.TotalQueries,
                IsConfigured = isConfigured,
                LastQueryAt = stat.LastQueryAt
            };
        }
    }

    public static string NormalizeKey(string? provider)
    {
        if (string.IsNullOrWhiteSpace(provider)) return "gemini";
        var lower = provider.Trim().ToLowerInvariant();
        if (lower.Contains("qwen")) return "qwen";
        if (lower.Contains("gpt") || lower.Contains("oss")) return "gpt-oss";
        return "gemini";
    }
}
