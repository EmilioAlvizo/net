using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksService.Entities;

[Table("granjas")]
public class Granja
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("owner_id")]
    public Guid OwnerId { get; set; }

    [Column("nombre")]
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("ubicacion")]
    public string? Ubicacion { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}