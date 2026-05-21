// Services/FarmService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers; // 👈 Obligatorio para AuthenticationHeaderValue
using System.Net.Http.Json;
using System.Threading.Tasks;
using AppAvalonia3.Models;
using Supabase.Postgrest.Exceptions;
using Supabase;

namespace AppAvalonia3.Services;

public interface IFarmService
{
    Task<bool> CreateFarmAsync(string name, string location, string notes);
    Task<List<Farm>> GetUserFarmsAsync(bool forceRefresh = false);
}

public class SupabaseFarmService : IFarmService
{
    private readonly Client _supabase;
    private List<Farm>? _cachedFarms;

    public SupabaseFarmService(Client supabase)
    {
        _supabase = supabase;
    }

    public async Task<List<Farm>> GetUserFarmsAsync(bool forceRefresh = false)
    {
        if (_cachedFarms != null && !forceRefresh) return _cachedFarms;

        /* var result00 = await _supabase.From<Farm>()
                .Select("*,perfiles:owner_id(nombre)")
                .Get(); */

        /* var result00 = await _supabase.From<Farm>()
                .Get();

        var result0 = await _supabase.From<Farm>()
                .Select("*,perfiles:owner_id(nombre)")
                .Get();

        var result2 = await _supabase.From<Farm>()
            .Select("*, OwnerPerfil:owner_id(*)")
            .Get();

        var result3 = await _supabase.From<Farm>()
            .Select("*, OwnerPerfil(*)")
            .Get();

        var result4 = await _supabase.From<Farm>()
            .Select("*, perfiles(*)")
            .Get();

        var result5 = await _supabase.From<Farm>()
            .Select("id, nombre, ubicacion, owner_id, created_by, OwnerPerfil:perfiles!granja_owner_id_fkey(id, nombre)")
            .Get(); */

        try
        {

            var user = _supabase.Auth.CurrentUser;

            Console.WriteLine(user?.Id);
            // Sintaxis ultra-específica:
            // 1. Traemos todo de la granja (*)
            // 2. Traemos el perfil usando el alias 'OwnerPerfil'
            // 3. Especificamos la FK con '!' y pedimos solo el 'nombre'
            var result = await _supabase.From<Farm>()
                .Get();
            //Sintaxis ultra-específica: 1. Traemos todo de la granja (*) 2. Traemos el perfil usando el alias 'OwnerPerfil' 3. Especificamos la FK con '!' y pedimos solo el 'nombre'"result" no es NULL aquí.
            /* var result = await _supabase.From<Farm>()
                .Select("*, OwnerPerfil:perfiles!granja_owner_id_fkey(*)")
                .Get(); */

            /* var result2 = await _supabase.From<Farm>()
                .Select("*, OwnerPerfil:owner_id(*)")
                .Get();

            var result3 = await _supabase.From<Farm>()
                .Select("*, OwnerPerfil(*)")
                .Get();

            var result4 = await _supabase.From<Farm>()
                .Select("*, perfiles(*)")
                .Get();

            var result5 = await _supabase.From<Farm>()
                .Select("id, nombre, ubicacion, owner_id, created_by, OwnerPerfil:perfiles!granja_owner_id_fkey(id, nombre)")
                .Get(); */
            Console.WriteLine(result);


            _cachedFarms = result.Models;
            return _cachedFarms;
        }
        catch (PostgrestException ex)
        {
            // Esto te ayudará a ver en la consola si el error cambió de código o mensaje
            System.Diagnostics.Debug.WriteLine($"Error de Supabase: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CreateFarmAsync(string name, string location, string notes)
    {
        var userId = _supabase.Auth.CurrentUser?.Id;
        if (userId == null) return false;

        var nuevaGranja = new Farm
        {
            Nombre = name,
            Ubicacion = location,
            OwnerId = userId
        };

        var result = await _supabase.From<Farm>().Insert(nuevaGranja);

        if (result.ResponseMessage.IsSuccessStatusCode)
            _cachedFarms = null;

        return result.ResponseMessage.IsSuccessStatusCode;
    }


    // Método vital: Si el usuario cierra sesión, hay que borrar la caché
    public void ClearCache()
    {
        _cachedFarms = null;
    }
}


