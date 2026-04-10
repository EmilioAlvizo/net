using System.Text.Json;
using TasksService.Models.DTOs.PerfilesDto;

namespace TasksService.Services;

public class PerfilService
{
    private readonly SupabaseRestService _rest;
    private readonly HttpClient _http;

    public PerfilService(SupabaseRestService rest, HttpClient http)
    {
        _rest = rest;
        _http = http;
    }

    public async Task<JsonElement?> GetById(Guid id, string token)
    {
        var request = _rest.CreateRequest(
            HttpMethod.Get,
            $"perfiles?id=eq.{id}&select=*",
            null,
            token
        );

        var res = await _http.SendAsync(request);
        if (!res.IsSuccessStatusCode) return null;

        var json = await res.Content.ReadFromJsonAsync<JsonElement>();

        // Fix: array vacío → null en vez de excepción
        if (json.GetArrayLength() == 0) return null;
        return json[0];
    }

    public async Task<IEnumerable<JsonElement>> GetAll(string token)
    {
        var request = _rest.CreateRequest(
            HttpMethod.Get,
            "perfiles?select=*",
            null,
            token
        );

        var res = await _http.SendAsync(request);
        if (!res.IsSuccessStatusCode) return [];

        var json = await res.Content.ReadFromJsonAsync<JsonElement>();
        return json.EnumerateArray().ToList();
    }

    public async Task<JsonElement?> Create(CreatePerfilDto dto, string token)
    {
        var request = _rest.CreateRequest(
            HttpMethod.Post,
            "perfiles",
            new
            {
                nombre = dto.Nombre,
                avatar_url = dto.AvatarUrl
            },
            token,
            returnRepresentation: true  // ← Prefer: return=representation
        );

        var res = await _http.SendAsync(request);
        if (!res.IsSuccessStatusCode)
        {
            var err = await res.Content.ReadAsStringAsync();
            throw new Exception($"Error al crear perfil: {err}");
        }

        var json = await res.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetArrayLength() > 0 ? json[0] : null;
    }

    public async Task<bool> Update(Guid id, UpdatePerfilDto dto, string token)
    {
        var request = _rest.CreateRequest(
            HttpMethod.Patch,
            $"perfiles?id=eq.{id}",
            new
            {
                nombre = dto.Nombre,
                avatar_url = dto.AvatarUrl
            },
            token
        );

        var res = await _http.SendAsync(request);

        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"STATUS: {res.StatusCode}");
            Console.WriteLine($"BODY: {body}");
        }

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(Guid id, string token)
    {
        var request = _rest.CreateRequest(
            HttpMethod.Delete,
            $"perfiles?id=eq.{id}",
            null,
            token
        );

        var res = await _http.SendAsync(request);

        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"STATUS: {res.StatusCode}");
            Console.WriteLine($"BODY: {body}");
        }

        return res.IsSuccessStatusCode;
    }
}