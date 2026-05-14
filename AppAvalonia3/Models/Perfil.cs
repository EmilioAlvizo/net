// Models/Perfil.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAvalonia3.Models;

[Table("perfiles")]
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
}