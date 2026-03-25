// Models/ProductoFiltro.cs
using Microsoft.EntityFrameworkCore;
using TasksService.Shared;

namespace TasksService.Models;

public class ProductoFiltro : BaseFilter
{
    // Solo agrega lo específico de clientes
    public string? Codigo { get; set; }
    public int? ClienteId { get; set; }
    public double? PrecioMin { get; set; }
    public double? PrecioMax { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
};

public static class ProductoFiltroExtensions
{
    private static readonly HashSet<string> ColumnasOrdenables =
    ["ProductoId", "Codigo", "Precio", "Cantidad", "FCaptura"];
    public static IQueryable<Producto> AplicarFiltros(
        this IQueryable<Producto> query, ProductoFiltro filtro)
    {
        if (!string.IsNullOrEmpty(filtro.Codigo))
            query = query.Where(p => p.Codigo!.Contains(filtro.Codigo));

        if (filtro.ClienteId.HasValue)
            query = query.Where(p => p.ClienteId == filtro.ClienteId);

        if (filtro.PrecioMin.HasValue)
            query = query.Where(p => p.Precio >= filtro.PrecioMin);

        if (filtro.PrecioMax.HasValue)
            query = query.Where(p => p.Precio <= filtro.PrecioMax);

        if (filtro.FechaDesde.HasValue)
            query = query.Where(p => p.FCaptura >= filtro.FechaDesde);

        if (filtro.FechaHasta.HasValue)
            query = query.Where(p => p.FCaptura <= filtro.FechaHasta);
        // 🔃 Orden
        if (!string.IsNullOrEmpty(filtro.OrdenarPor) && ColumnasOrdenables.Contains(filtro.OrdenarPor))
        {
            query = filtro.Descendente
                ? query.OrderByDescending(e => EF.Property<object>(e, filtro.OrdenarPor))
                : query.OrderBy(e => EF.Property<object>(e, filtro.OrdenarPor));
        }
        else
        {
            query = query.OrderBy(p => p.ProductoId); // default
        }

        return query;
    }
}