// Services/MiembrosService.cs
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

public interface IMiembrosService
{
    //Task<bool> CreateFarmAsync(string name, string location, string notes);
    Task<List<MiembroGranja>> GetUserMiembrosAsync(string miGranjaId, bool forceRefresh = false);
    //Task<bool> DeleteFarmAsync(string farmId);
}

public class SupabaseMiembrosService : IMiembrosService
{
    private readonly Client _supabase;
    private List<MiembroGranja>? _cachedMiembros;

    public SupabaseMiembrosService(Client supabase)
    {
        _supabase = supabase;
    }

    public async Task<List<MiembroGranja>> GetUserMiembrosAsync(string miGranjaId, bool forceRefresh = false)
    {
        if (_cachedMiembros != null && !forceRefresh) 
        {
            return _cachedMiembros;
        }

        //var miGranjaId = "f510713a-e251-4b37-8235-e59844e3fee2";

        try
        {

            var result = await _supabase.From<MiembroGranja>()
                .Select("granja_id,user_id,rol, perfiles!miembros_granja_user_id_fkey(nombre,email) ,granjas!miembros_granja_granja_id_fkey(nombre)")
                .Filter("granja_id",Supabase.Postgrest.Constants.Operator.Equals, miGranjaId) // <-- Filtro para emparejar con el ID de la granja
                .Get();

            Console.WriteLine(result);


            _cachedMiembros = result.Models;
            return _cachedMiembros;
        }
        catch (PostgrestException ex)
        {
            // Esto te ayudará a ver en la consola si el error cambió de código o mensaje
            System.Diagnostics.Debug.WriteLine($"Error de Supabase: {ex.Message}");
            throw;
        }
    }

    /* public async Task<bool> CreateFarmAsync(string name, string location, string notes)
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

    public async Task<bool> DeleteFarmAsync(string farmId)
    {
        try
        {
            // Filtramos por el ID de la granja y ejecutamos el Delete
            await _supabase.From<Farm>()
                .Where(f => f.Id == farmId)
                .Delete();

            _cachedFarms = null; // Limpiamos caché para forzar recarga
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al eliminar en Supabase: {ex.Message}");
            return false;
        }
    } */


    // Método vital: Si el usuario cierra sesión, hay que borrar la caché
    public void ClearCache()
    {
        _cachedMiembros = null;
    }
}


