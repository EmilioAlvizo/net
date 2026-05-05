using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AppAvalonia.Services;

public record LoginResult(bool Success, string? Token, string? UserId, string? ErrorMessage);

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string email, string password);
    void Logout();
    string? GetToken();
    string? GetUserId();
}

public class SupabaseAuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly SupabaseConfig _config;
    private string? _accessToken;
    private string? _userId;

    public SupabaseAuthService(HttpClient http, SupabaseConfig config)
    {
        _http = http;
        _config = config;
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        var url = $"{_config.Url}/auth/v1/token?grant_type=password";

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("apikey", _config.AnonKey);
        request.Content = JsonContent.Create(new { email, password });

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<SupabaseAuthError>();
            return new LoginResult(false, null, null, error?.ErrorDescription ?? "Credenciales incorrectas");
        }

        var result = await response.Content.ReadFromJsonAsync<SupabaseAuthResponse>();
        _accessToken = result?.AccessToken;
        _userId = result?.User?.Id;

        return new LoginResult(true, _accessToken, _userId, null);
    }

    public void Logout()
    {
        _accessToken = null;
        _userId = null;
    }

    public string? GetToken() => _accessToken;
    public string? GetUserId() => _userId;

    private record SupabaseAuthResponse(string AccessToken, string TokenType, SupabaseUser? User);
    private record SupabaseUser(string Id, string Email);
    private record SupabaseAuthError(string Error, string ErrorDescription);
}