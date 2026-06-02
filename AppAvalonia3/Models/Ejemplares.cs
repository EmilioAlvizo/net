// Models/Ejemplares.cs
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;

namespace AppAvalonia3.Models;

[Table("ejemplares")]
public class LoteAnimal : BaseModel
{
    [PrimaryKey("id")] public string Id { get; set; }
    [Column("granja_id")] public string GranjaId { get; set; }
    [Column("tipo_animal_id")] public string TipoAnimalId { get; set; }
    [Column("grupo_id")] public string GrupoId { get; set; }
    [Column("brazalete")] public int Brazalete { get; set; }
    [Column("proposito_id")] public string PropositoId { get; set; } // esto es el id de la tabla cat_proposito_animal
    [Column("tipo_adquisicion_id")] public string TipoAdquisicionId { get; set; } // esto es el id de la tabla cat_tipo_adquisicion
    [Column("fecha_adquisicion")] public DateTime FechaAdquisicion { get; set; } = DateTime.Today;
    [Column("costo_adquisicion")] public decimal? CostoAdquisicion { get; set; }
    [Column("cantidad")] public int Cantidad { get; set; }
    [Column("activo")] public bool Activo { get; set; }
    [Column("notas")] public string? Notas { get; set; }
    [Column("created_by")] public string CreatedBy { get; set; }
}