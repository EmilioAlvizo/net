// Models/TipoAnimal.cs
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;


namespace AppAvalonia3.Models;

[Table("tipo_animal")]
public class TipoAnimal : BaseModel
{
    [PrimaryKey("id")] public string Id { get; set; } = string.Empty; // Ej: "gallina", "pollo"
    [Column("nombre")] public string Nombre { get; set; } = string.Empty; // Ej: "Gallina", "Pollo"
    [Column("descripcion")] public string? Descripcion { get; set; }
    [Column("granja_id")] public string GranjaId { get; set; } = string.Empty;
    [Column("created_by")] public string CreatedBy { get; set; } 
    
}