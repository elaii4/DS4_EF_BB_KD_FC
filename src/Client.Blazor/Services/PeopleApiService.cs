using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Blazor.Models;
using Microsoft.Extensions.Configuration;

namespace Client.Blazor.Services;

public enum ServerTarget
{
    Windows,
    Ubuntu
}

// PeopleApiService
// Servicio cliente que gestiona la comunicación HTTP RESTful con los servidores Windows y Ubuntu.
// Maneja la alternancia dinámica de servidores, lectura de configuración y operaciones CRUD.
public class PeopleApiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;

    public ServerTarget CurrentServer { get; private set; } = ServerTarget.Ubuntu;
    public string CurrentBaseUrl { get; private set; } = string.Empty;
    public string WindowsUrl { get; private set; } = "http://localhost:5144/";
    public string UbuntuUrl { get; private set; } = "https://talked-cart-essential-technician.trycloudflare.com/";

    public event Action? OnServerChanged;

    public PeopleApiService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _configuration = configuration;

        LoadConfiguration();
    }

    private void LoadConfiguration()
    {
        var win = _configuration["ApiSettings:ApiUrl_Windows"] 
            ?? _configuration["ApiUrl_Windows"] 
            ?? "http://localhost:5144";

        var ubu = _configuration["ApiSettings:ApiUrl_Ubuntu"] 
            ?? _configuration["ApiUrl_Ubuntu"] 
            ?? "https://talked-cart-essential-technician.trycloudflare.com";

        var def = _configuration["ApiSettings:DefaultServer"] 
            ?? _configuration["DefaultServer"] 
            ?? "Ubuntu";

        WindowsUrl = EnsureTrailingSlash(win.Trim());
        UbuntuUrl = EnsureTrailingSlash(ubu.Trim());

        CurrentServer = def.Equals("Windows", StringComparison.OrdinalIgnoreCase)
            ? ServerTarget.Windows
            : ServerTarget.Ubuntu;

        CurrentBaseUrl = CurrentServer == ServerTarget.Windows ? WindowsUrl : UbuntuUrl;
    }

    private static string EnsureTrailingSlash(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return "http://localhost:5000/";
        return url.EndsWith('/') ? url : url + "/";
    }

    public ServerTarget ToggleServer()
    {
        CurrentServer = CurrentServer == ServerTarget.Windows ? ServerTarget.Ubuntu : ServerTarget.Windows;
        CurrentBaseUrl = CurrentServer == ServerTarget.Windows ? WindowsUrl : UbuntuUrl;
        OnServerChanged?.Invoke();
        return CurrentServer;
    }

    public void SetServer(ServerTarget target)
    {
        if (CurrentServer != target)
        {
            CurrentServer = target;
            CurrentBaseUrl = CurrentServer == ServerTarget.Windows ? WindowsUrl : UbuntuUrl;
            OnServerChanged?.Invoke();
        }
    }

    public async Task<List<Person>> GetPeopleAsync(string? search = null)
    {
        var endpoint = $"{CurrentBaseUrl}api/people";
        if (!string.IsNullOrWhiteSpace(search))
        {
            endpoint += $"?search={Uri.EscapeDataString(search.Trim())}";
        }

        try
        {
            var result = await _http.GetFromJsonAsync<List<Person>>(endpoint);
            return result ?? new List<Person>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error de conexión con el servidor {CurrentServer} ({CurrentBaseUrl}): {ex.Message}", ex);
        }
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        var endpoint = $"{CurrentBaseUrl}api/people/{id}";
        try
        {
            var response = await _http.GetAsync(endpoint);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Person>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al consultar ID {id} en {CurrentServer}: {ex.Message}", ex);
        }
    }

    public async Task<Person> CreateAsync(PersonCreateDto dto)
    {
        var endpoint = $"{CurrentBaseUrl}api/people";
        try
        {
            var response = await _http.PostAsJsonAsync(endpoint, dto);
            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear ({response.StatusCode}): {errorText}");
            }

            var created = await response.Content.ReadFromJsonAsync<Person>();
            return created ?? new Person { Name = dto.Name, Age = dto.Age };
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al enviar creación a {CurrentServer}: {ex.Message}", ex);
        }
    }

    public async Task UpdateAsync(int id, PersonUpdateDto dto)
    {
        var endpoint = $"{CurrentBaseUrl}api/people/{id}";
        try
        {
            var response = await _http.PutAsJsonAsync(endpoint, dto);
            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar ({response.StatusCode}): {errorText}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al actualizar en {CurrentServer}: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var endpoint = $"{CurrentBaseUrl}api/people/{id}";
        try
        {
            var response = await _http.DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar ({response.StatusCode}): {errorText}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al eliminar en {CurrentServer}: {ex.Message}", ex);
        }
    }

    public string GetReportPdfUrl(string? search = null)
    {
        var endpoint = $"{CurrentBaseUrl}api/people/report/pdf";
        if (!string.IsNullOrWhiteSpace(search))
        {
            endpoint += $"?search={Uri.EscapeDataString(search.Trim())}";
        }
        return endpoint;
    }

    public async Task<byte[]> DownloadReportPdfBytesAsync(string? search = null)
    {
        var endpoint = GetReportPdfUrl(search);
        try
        {
            var response = await _http.GetAsync(endpoint);

            // Si el servidor activo (ej: VPS Ubuntu) responde 404 porque aún no ha desplegado el endpoint nativo:
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Fallback transparente: obtenemos los datos actuales y generamos el PDF usando el endpoint generador local
                var people = await GetPeopleAsync(search);
                var fallbackPayload = new
                {
                    People = people,
                    ServerName = CurrentServer == ServerTarget.Ubuntu ? "Ubuntu Server (AWS Cloud)" : "Windows Server (Local)",
                    DatabaseName = CurrentServer == ServerTarget.Ubuntu ? "crud_windows_forms_ubu" : "crud_windows_forms_win",
                    Search = search
                };

                var fallbackUrl = $"{WindowsUrl}api/people/report/pdf/generate";
                var localResponse = await _http.PostAsJsonAsync(fallbackUrl, fallbackPayload);
                if (localResponse.IsSuccessStatusCode)
                {
                    return await localResponse.Content.ReadAsByteArrayAsync();
                }
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al generar el reporte en {CurrentServer}: {ex.Message}", ex);
        }
    }

    public async Task<McpQueryResponseDto> SendMcpQueryAsync(string prompt, string provider = "gemini")
    {
        var endpoint = $"{CurrentBaseUrl}api/mcp/query";
        var payload = new McpQueryRequestDto
        {
            Prompt = prompt,
            Provider = provider,
            Target = CurrentServer == ServerTarget.Windows ? "windows" : "ubuntu"
        };

        try
        {
            var response = await _http.PostAsJsonAsync(endpoint, payload);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                return new McpQueryResponseDto
                {
                    IsSuccess = false,
                    Reply = $"Error al enviar consulta a {CurrentServer} ({response.StatusCode}): {errorBody}",
                    ErrorMessage = errorBody
                };
            }

            var result = await response.Content.ReadFromJsonAsync<McpQueryResponseDto>();
            return result ?? new McpQueryResponseDto
            {
                IsSuccess = false,
                Reply = "Respuesta vacía recibida del servidor."
            };
        }
        catch (Exception ex)
        {
            return new McpQueryResponseDto
            {
                IsSuccess = false,
                Reply = $"Error de conexión con el servidor activo ({CurrentServer}): {ex.Message}",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<McpUsageSummaryDto?> GetMcpUsageAsync(string provider = "gemini")
    {
        var endpoint = $"{CurrentBaseUrl}api/mcp/usage?provider={provider}";
        try
        {
            return await _http.GetFromJsonAsync<McpUsageSummaryDto>(endpoint);
        }
        catch
        {
            return null;
        }
    }
}
