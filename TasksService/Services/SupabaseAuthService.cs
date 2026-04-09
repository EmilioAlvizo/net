using System.Text.Json;

namespace TasksService.Services;

public class SupabaseAuthService
{
    private readonly HttpClient _http;
    private readonly string _supabaseUrl;
    private readonly string _anonKey;

    public SupabaseAuthService(HttpClient http, SupabaseConfig config)
    {
        _http = http;
        _supabaseUrl = config.Url;
        _anonKey = config.AnonKey;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var url = $"{_supabaseUrl}/auth/v1/token?grant_type=password";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new { email, password })
        };
        request.Headers.Add("apikey", _anonKey);

        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("access_token").GetString();
    }

    /* public async Task<bool> RegisterAsync(string email, string password)
    {
        var url     = $"{_supabaseUrl}/auth/v1/signup";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new { email, password })
        };
        request.Headers.Add("apikey", _anonKey);

        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    } */
    public async Task<bool> RegisterAsync(string email, string password)
    {
        var url = $"{_supabaseUrl}/auth/v1/signup";

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(new { email, password })
        };

        request.Headers.Add("apikey", _anonKey);
        request.Headers.Add("Authorization", $"Bearer {_anonKey}");

        var response = await _http.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"STATUS: {response.StatusCode}");
        Console.WriteLine($"BODY: {content}");

        return response.IsSuccessStatusCode;
    }
}