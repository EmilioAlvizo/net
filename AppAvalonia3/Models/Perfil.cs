// Models/Perfil.cs
using System;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace AppAvalonia3.Models;

[Table("perfiles")]
public class Perfil : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; }
    
    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("email")]
    public string Email { get; set; }
}

/* [Table("perfiles")]
public class Perfil 
{
    [Key]
    [Column("id")]
    public string Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; }

    [Column("avatar_url")]
    public string AvatarUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} */

/* public class Perfil
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }
} */

