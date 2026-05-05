using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System;
using AppAvalonia.Models;

namespace AppAvalonia.Services;

public interface IGranjasService
{
    Task<IEnumerable<GranjaCardModel>> GetMisGranjasAsync();
    Task<GranjaDto> CrearGranjaAsync(string nombre, string? descripcion, string? ubicacion);
}

public class SupabaseGranjasService : IGranjasService
{
    private readonly SupabaseRestClient _rest;
    private readonly IAuthService _auth;

    public SupabaseGranjasService(SupabaseRestClient rest, IAuthService auth)
    {
        _rest = rest;
        _auth = auth;
    }

    public async Task<IEnumerable<GranjaCardModel>> GetMisGranjasAsync()
    {
        var userId = _auth.GetUserId()
            ?? throw new InvalidOperationException("Usuario no autenticado");

        // RLS en Supabase ya filtra, pero agregamos filtro explícito como respaldo:
        // granjas donde el usuario es owner O es miembro
        // Por simplicidad empezamos con owner_id
        var granjas = await _rest.GetAsync<GranjaDto>(
            "granjas",
            $"owner_id=eq.{userId}&order=created_at.desc"
        );

        // Enriquecer con stats usando consultas paralelas
        var tasks = granjas.Select(async g =>
        {
            var (aves, huevos, balance) = await GetGranjaStatsAsync(g.Id);
            return new GranjaCardModel
            {
                Id = g.Id,
                Nombre = g.Nombre,
                Descripcion = g.Descripcion,
                Ubicacion = g.Ubicacion,
                TotalAves = aves,
                TotalHuevos = huevos,
                Balance = balance
            };
        });

        return await Task.WhenAll(tasks);
    }

    public async Task<GranjaDto> CrearGranjaAsync(string nombre, string? descripcion, string? ubicacion)
    {
        var userId = _auth.GetUserId()
            ?? throw new InvalidOperationException("Usuario no autenticado");

        return await _rest.PostAsync<GranjaDto>("granjas", new
        {
            nombre,
            descripcion,
            ubicacion,
            owner_id = userId,
            created_by = userId
        });
    }

    /// <summary>
    /// Obtiene estadísticas básicas de una granja:
    /// - Total de aves activas (ejemplares.activo = true)
    /// - Total de huevos buenos (suma de recolecciones_huevo)
    /// - Balance = ingresos - gastos (gastos_ingresos_extra)
    /// 
    /// Nota: PostgREST soporta aggregate queries con ?select=count,sum
    /// Docs: https://supabase.com/docs/guides/database/full-text-search
    /// </summary>
    private async Task<(int aves, int huevos, decimal balance)> GetGranjaStatsAsync(string granjaId)
    {
        try
        {
            // Conteo de aves activas
            // GET /rest/v1/ejemplares?granja_id=eq.{id}&activo=eq.true&select=count
            var avesResult = await _rest.GetAsync<CountResult>(
                "ejemplares",
                $"granja_id=eq.{granjaId}&activo=eq.true&select=count"
            );
            var aves = avesResult.FirstOrDefault()?.Count ?? 0;

            // Suma de huevos buenos (todos los periodos)
            var huevosResult = await _rest.GetAsync<SumHuevosResult>(
                "recolecciones_huevo",
                $"granja_id=eq.{granjaId}&select=huevos_buenos.sum()"
            );
            var huevos = (int)(huevosResult.FirstOrDefault()?.HuenosBuenosSum ?? 0);

            // Balance: suma de importes por tipo (ingreso - gasto)
            var gastosIngresosResult = await _rest.GetAsync<GastoIngresoStat>(
                "gastos_ingresos_extra",
                $"granja_id=eq.{granjaId}&select=tipo,importe.sum()"
            );

            decimal ingresos = gastosIngresosResult
                .Where(x => x.Tipo == "ingreso")
                .Sum(x => x.ImporteSum);
            decimal gastos = gastosIngresosResult
                .Where(x => x.Tipo == "gasto")
                .Sum(x => x.ImporteSum);

            // Sumar también ventas de bajas (cat_razon_baja.genera_ingreso = true)
            var bajasResult = await _rest.GetAsync<SumBajasResult>(
                "bajas_ejemplares",
                $"ejemplar_id=in.(select id from ejemplares where granja_id=eq.{granjaId})&select=importe_venta.sum()"
            );
            ingresos += bajasResult.FirstOrDefault()?.ImporteVentaSum ?? 0;

            return (aves, huevos, ingresos - gastos);
        }
        catch
        {
            // Si falla el enriquecimiento, devolvemos la granja sin stats
            return (0, 0, 0);
        }
    }

    // DTOs para aggregate queries de PostgREST
    private record CountResult([property: JsonPropertyName("count")] int Count);
    private record SumHuevosResult(
        [property: JsonPropertyName("huevos_buenos")] decimal? HuenosBuenosSum);
    private record GastoIngresoStat(
        [property: JsonPropertyName("tipo")] string Tipo,
        [property: JsonPropertyName("importe")] decimal ImporteSum);
    private record SumBajasResult(
        [property: JsonPropertyName("importe_venta")] decimal? ImporteVentaSum);
}

// DTO de Granja que mapea la tabla granjas
public class GranjaDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; } = string.Empty;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("ubicacion")]
    public string? Ubicacion { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}