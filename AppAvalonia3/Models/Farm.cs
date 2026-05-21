// Models/Farm.cs
using System;

using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace AppAvalonia3.Models;

/* public record Farm(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("nombre")] string Nombre,
    [property: JsonPropertyName("ubicacion")] string? Ubicacion,
    [property: JsonPropertyName("descripcion")] string? Descripcion,
    [property: JsonPropertyName("owner_id")] string OwnerId
); */

/* public class Farm 
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("owner_id")]
    public Guid OwnerId { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("ubicacion")]
    public string? Ubicacion { get; set; }

    // Esta es la clave: el nombre debe coincidir con el alias que usaremos en la URL
    [JsonPropertyName("OwnerPerfil")]
    public Perfil? OwnerPerfil { get; set; }
} */

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