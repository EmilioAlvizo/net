// Models/Farm.cs
using System;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace AppAvalonia3.Models;

[Table("granjas")]
public class Farm : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("owner_id")]
    public string OwnerId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }
    
    [Column("ubicacion")]
    public string? Ubicacion { get; set; }

    [Column("created_by")]
    public string CreatedBy { get; set; }

    // 👇 ESTA PARTE FALTA
    [Reference(typeof(Perfil), foreignKey: "granja_owner_id_fkey")]
    public Perfil? OwnerPerfil { get; set; }
}