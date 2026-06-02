// Models/Grupos.cs
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;

namespace AppAvalonia3.Models;

[Table("grupos")]
public class GrupoAnimal : BaseModel
{
    [PrimaryKey("id")] public string Id { get; set; }
    [Column("granja_id")] public string GranjaId { get; set; }
    [Column("tipo_animal_id")] public string TipoAnimalId { get; set; }
    [Column("nombre")] public string Nombre { get; set; } // Ej: "Gallinero"
    [Column("descripcion")] public string? Descripcion { get; set; }
    [Column("created_by")] public string CreatedBy { get; set; }
    
    
}