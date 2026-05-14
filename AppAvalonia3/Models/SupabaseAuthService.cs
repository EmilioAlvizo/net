// SupabaseAuthService.cs
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AppAvalonia3.Services;

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
    // 1. Eliminamos las variables locales string? _accessToken y _userId
    private readonly ISessionContext _session; // 👈 NUEVA VARIABLE GLOBAL

    // 2. Modificamos el constructor para recibir el contexto de sesión
    public SupabaseAuthService(HttpClient http, SupabaseConfig config, ISessionContext session)
    {
        _http = http;
        _config = config;
        _session = session; // 👈 ASIGNACIÓN
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
        
        // 3. 💾 GUARDAR DIRECTAMENTE EN EL SINGLETON DE SESIÓN
        _session.Token = result?.AccessToken;
        _session.UserId = result?.User?.Id;

        return new LoginResult(true, _session.Token, _session.UserId, null);
    }

    // 4. Limpiamos el contenedor global al cerrar sesión
    public void Logout() => _session.Clear();

    // 5. Retornamos los valores directamente desde el Singleton de sesión
    public string? GetToken() => _session.Token;
    public string? GetUserId() => _session.UserId;

    private record SupabaseAuthResponse(
    [property: JsonPropertyName("access_token")] string AccessToken, // 👈 CORRECCIÓN
    [property: JsonPropertyName("token_type")] string TokenType,
    [property: JsonPropertyName("user")] SupabaseUser? User
);

private record SupabaseUser(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("email")] string Email
);

private record SupabaseAuthError(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("error_description")] string ErrorDescription
);
}
