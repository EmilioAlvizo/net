// Models/DTOs/GranjaDto.cs
namespace TasksService.Models.DTOs;
public class GranjaDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? Ubicacion { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}