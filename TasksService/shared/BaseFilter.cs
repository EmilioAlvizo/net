// Shared/BaseFilter.cs
using System.ComponentModel.DataAnnotations;
namespace TasksService.Shared;

public class BaseFilter
{
    public int Pagina { get; set; } = 1;
    [Range(1, 100)]
    public int TamanoPagina { get; set; } = 10;
    public string? OrdenarPor { get; set; }
    public bool Descendente { get; set; } = false;
}