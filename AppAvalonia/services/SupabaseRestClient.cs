using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AppAvalonia.Services;

/// <summary>
/// Cliente base para PostgREST de Supabase.
/// Todas las llamadas a tablas van por aquí.
/// 
/// Base URL:  https://{project}.supabase.co/rest/v1/{tabla}
/// Headers requeridos:
///   apikey: {anonKey}
///   Authorization: Bearer {jwt}
///   Content-Type: application/json
/// </summary>
public class SupabaseRestClient
{
    private readonly HttpClient _http;
    private readonly SupabaseConfig _config;
    private readonly IAuthService _auth;

    public SupabaseRestClient(HttpClient http, SupabaseConfig config, IAuthService auth)
    {
        _http = http;
        _config = config;
        _auth = auth;
    }

    /// <summary>
    /// GET /rest/v1/{tabla}?{filtros}
    /// Ejemplo: GetAsync&lt;GranjaDto&gt;("granjas", "owner_id=eq.{userId}")
    /// </summary>
    public async Task<List<T>> GetAsync<T>(string tabla, string? query = null)
    {
        var url = $"{_config.Url}/rest/v1/{tabla}";
        if (!string.IsNullOrEmpty(query))
            url += $"?{query}";

        var request = BuildRequest(HttpMethod.Get, url);
        // Pedir objeto completo
        request.Headers.Add("Prefer", "return=representation");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<T>>()
               ?? new List<T>();
    }

    /// <summary>
    /// POST /rest/v1/{tabla}
    /// </summary>
    public async Task<T> PostAsync<T>(string tabla, object body)
    {
        var url = $"{_config.Url}/rest/v1/{tabla}";
        var request = BuildRequest(HttpMethod.Post, url);
        request.Headers.Add("Prefer", "return=representation");
        request.Content = JsonContent.Create(body);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // PostgREST devuelve un array incluso para inserts individuales
        var list = await response.Content.ReadFromJsonAsync<List<T>>();
        return list![0];
    }

    /// <summary>
    /// PATCH /rest/v1/{tabla}?{filtro}
    /// </summary>
    public async Task PatchAsync(string tabla, string query, object body)
    {
        var url = $"{_config.Url}/rest/v1/{tabla}?{query}";
        var request = BuildRequest(HttpMethod.Patch, url);
        request.Headers.Add("Prefer", "return=minimal");
        request.Content = JsonContent.Create(body);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// DELETE /rest/v1/{tabla}?{filtro}
    /// </summary>
    public async Task DeleteAsync(string tabla, string query)
    {
        var url = $"{_config.Url}/rest/v1/{tabla}?{query}";
        var request = BuildRequest(HttpMethod.Delete, url);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    // Construye el request con los headers de Supabase
    private HttpRequestMessage BuildRequest(HttpMethod method, string url)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Add("apikey", _config.AnonKey);

        var token = _auth.GetToken();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Add("Authorization", $"Bearer {token}");

        return request;
    }
}