// Models/PagedResponse.cs
using Microsoft.EntityFrameworkCore;

namespace TasksService.Models;

public class PagedResponse<T>
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TotalPaginas { get; set; }
    public int TamañoPagina { get; set; }
    public IEnumerable<T> Datos { get; set; } = [];

    public static async Task<PagedResponse<T>> CrearAsync(IQueryable<T> query, int pagina, int tamaño)
    {
        var total = await query.CountAsync();
        var datos = await query
            .Skip((pagina - 1) * tamaño)
            .Take(tamaño)
            .ToListAsync();

        return new PagedResponse<T>
        {
            Total = total,
            Pagina = pagina,
            TamañoPagina = tamaño,
            TotalPaginas = (int) Math.Ceiling(total / (double)tamaño),
            Datos = datos
        };
    }
    
}