// Services/SupabaseAuthService.cs
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TasksService.Services;

public class SupabaseAuthService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public SupabaseAuthService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<Guid> RegisterUserAsync(string email, string password, string nombre)
    {
        var url = _config["Supabase:Url"];
        var serviceKey = _config["Supabase:ServiceRoleKey"];

        // Usamos el endpoint admin para crear usuarios sin necesitar confirmación de email
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{url}/auth/v1/admin/users");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", serviceKey);
        request.Headers.Add("apikey", serviceKey);

        var body = new
        {
            email,
            password,
            email_confirm = true,  // confirma el email automáticamente
            user_metadata = new
            {
                nombre = nombre
            }
        };

        request.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Error al registrar en Supabase Auth: {error}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        // Supabase devuelve el UUID en la propiedad "id"
        var userId = doc.RootElement.GetProperty("id").GetString();
        return Guid.Parse(userId!);
    }
}