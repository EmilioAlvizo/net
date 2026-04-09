using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksService.Entities;

[Table("perfiles")]
public class Perfil
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("nombre")]
    public string? Nombre { get; set; }

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}