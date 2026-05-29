// Models/.cs
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;


namespace AppAvalonia3.Models;

[Table("ejemplares")]
public class LoteAnimal : BaseModel
{
    [PrimaryKey("id", false)] public string Id { get; set; } = Guid.NewGuid().ToString();
    [Column("grupo_id")] public string GrupoId { get; set; } = string.Empty;
    [Column("cantidad")] public int Cantidad { get; set; }
    [Column("proposito")] public string Proposito { get; set; } = "Huevos"; // Huevos, Carne, etc.
    [Column("adquisicion")] public string Adquisicion { get; set; } = "Compra"; // Compra, Cria, etc.
    [Column("costo_total")] public decimal CostoTotal { get; set; }
    [Column("fecha")] public DateTime Fecha { get; set; } = DateTime.Today;
    [Column("brazaletes")] public int Brazaletes { get; set; }
    [Column("muertes")] public int Muertes { get; set; }
}