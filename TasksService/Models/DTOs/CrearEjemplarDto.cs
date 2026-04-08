// Models/DTOs/CrearEjemplarDto.cs
public class CrearEjemplarDto
{
    public Guid GranjaId { get; set; }        
    public short Brazalete { get; set; }
    public Guid GrupoId { get; set; }
    public Guid TipoAnimalId { get; set; }
    public Guid PropositoId { get; set; }
    public Guid TipoAdquisicionId { get; set; }
    public DateOnly FechaAdquisicion { get; set; }
    public decimal? CostoAdquisicion { get; set; }
    public string? Notas { get; set; }
}