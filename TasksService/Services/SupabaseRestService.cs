using System.Net.Http.Headers;

namespace TasksService.Services;

public class SupabaseRestService
{
    private readonly HttpClient _http;
    private readonly string _url;
    private readonly string _anonKey;

    public SupabaseRestService(HttpClient http, SupabaseConfig config)
    {
        _http = http;
        _url = config.Url;
        _anonKey = config.AnonKey;
    }

    public HttpRequestMessage CreateRequest(
    HttpMethod method,
    string endpoint,
    object? body,
    string token,
    bool returnRepresentation = false)
    {
        var request = new HttpRequestMessage(method, $"{_url}/rest/v1/{endpoint}");

        if (body != null)
            request.Content = JsonContent.Create(body);

        request.Headers.Add("apikey", _anonKey);

        // Si no hay token, usa el anonKey para que Supabase no rechace el request
        var bearerToken = string.IsNullOrEmpty(token) ? _anonKey : token;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        if (returnRepresentation)
            request.Headers.Add("Prefer", "return=representation");

        return request;
    }
}