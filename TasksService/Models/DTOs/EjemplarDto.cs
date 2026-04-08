// Models/DTOs/EjemplarDto.cs
public class EjemplarDto
{
    public Guid Id { get; set; }
    public short Brazalete { get; set; }
    public Guid GrupoId { get; set; }
    public Guid TipoAnimalId { get; set; }
    public string Proposito { get; set; } = null!;
    public string TipoAdquisicion { get; set; } = null!;
    public DateOnly FechaAdquisicion { get; set; }
    public decimal? CostoAdquisicion { get; set; }
    public string? Notas { get; set; }
}