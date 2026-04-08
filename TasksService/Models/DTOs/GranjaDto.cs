// Models/DTOs/GranjaDto.cs
namespace TasksService.Models.DTOs;

// ─────────────────────────────────────────────
// Lo que devuelves en GET
// ─────────────────────────────────────────────
public class LeerGranjaDto
{
    public Guid   Id          { get; set; }
    public string Nombre      { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? Ubicacion   { get; set; }
    public Guid   OwnerId     { get; set; }
}

// ─────────────────────────────────────────────
// Lo que recibes en POST
// ─────────────────────────────────────────────
public class CrearGranjaDto
{
    public string  Nombre      { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? Ubicacion   { get; set; }
}

// ─────────────────────────────────────────────
// Lo que recibes en PUT (solo campos editables)
// ─────────────────────────────────────────────
public class ActualizarGranjaDto
{
    public string  Nombre      { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? Ubicacion   { get; set; }
}