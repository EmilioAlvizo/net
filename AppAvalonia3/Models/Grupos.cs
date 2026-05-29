// Models/.cs
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;


namespace AppAvalonia3.Models;

[Table("grupos")]
public class GrupoAnimal : BaseModel
{
    [PrimaryKey("id", false)] public string Id { get; set; } = Guid.NewGuid().ToString();
    [Column("nombre")] public string Nombre { get; set; } = string.Empty; // Ej: "Gallinero"
    [Column("descripcion")] public string? Descripcion { get; set; }
    [Column("tipo_animal_id")] public string TipoAnimalId { get; set; } = string.Empty;
    [Column("granja_id")] public string GranjaId { get; set; } = string.Empty;
}