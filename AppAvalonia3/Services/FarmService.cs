// Services/FarmService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers; // 👈 Obligatorio para AuthenticationHeaderValue
using System.Net.Http.Json;
using System.Threading.Tasks;
using AppAvalonia3.Models;

namespace AppAvalonia3.Services;

public interface IFarmService
{
    Task<bool> CreateFarmAsync(string name, string location, string notes);
    Task<List<Farm>> GetUserFarmsAsync(bool forceRefresh = false);
}

public class SupabaseFarmService : IFarmService
{
    private readonly HttpClient _http;
    private readonly SupabaseConfig _config;
    private readonly IAuthService _auth;

    // Guardamos las granjas en memoria aquí dentro
    private List<Farm>? _cachedFarms;

    public SupabaseFarmService(HttpClient http, SupabaseConfig config, IAuthService auth)
    {
        _http = http;
        _config = config;
        _auth = auth;
    }

    public async Task<bool> CreateFarmAsync(string name, string location, string notes)
    {
        var userId = _auth.GetUserId();
        var token = _auth.GetToken();

        // 🛡️ Validación: Detener la operación si no hay sesión activa
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
        {
            return false;
        }

        var url = $"{_config.Url}/rest/v1/granjas";

        var request = new HttpRequestMessage(HttpMethod.Post, url);

        // Cabeceras estándar de Supabase
        request.Headers.Add("apikey", _config.AnonKey);
        request.Headers.Add("Prefer", "return=minimal");

        // 🛠️ Corrección de la cabecera Authorization
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Mapeo exacto a las columnas de la tabla 'granjas'
        request.Content = JsonContent.Create(new
        {
            nombre = name,
            ubicacion = location,
            descripcion = notes,
            owner_id = userId,
            created_by = userId
        });

        var response = await _http.SendAsync(request);
        _cachedFarms = null; //limpiamos cache

        return response.IsSuccessStatusCode;
    }

    // Agregamos un parámetro opcional 'forceRefresh' por si quieren actualizar tirando hacia abajo (pull-to-refresh)
    public async Task<List<Farm>> GetUserFarmsAsync(bool forceRefresh = false)
    {
        // Si YA tenemos los datos guardados y NO se exige refrescar, los devolvemos sin llamar a la BD
        if (_cachedFarms != null && !forceRefresh)
        {
            return _cachedFarms;
        }

        var token = _auth.GetToken();
        var userId = _auth.GetUserId();

        // Si no está autenticado, devolvemos una lista vacía
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            return new List<Farm>();

        // Endpoint de PostgREST para filtrar por el owner_id del usuario actual
        var url = $"{_config.Url}/rest/v1/granjas?owner_id=eq.{userId}&select=*";

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        // Cabeceras obligatorias requeridas por la API de Supabase
        request.Headers.Add("apikey", _config.AnonKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            // Aquí podrías loguear el error si lo deseas
            return _cachedFarms ?? new List<Farm>();
        }

        var farms = await response.Content.ReadFromJsonAsync<List<Farm>>();
        
        // Guardamos el resultado en la caché antes de devolverlo
        _cachedFarms = farms ?? new List<Farm>();
        
        return _cachedFarms;
    }

    // Método vital: Si el usuario cierra sesión, hay que borrar la caché
    public void ClearCache()
    {
        _cachedFarms = null;
    }
}
