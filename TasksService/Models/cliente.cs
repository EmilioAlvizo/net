using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TasksService.Models;

[Table("clientes")]  // nombre exacto de la tabla en la BD
public class Cliente
{
    [Key]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    [DefaultValue("Isabella Romero")]
    public string? Nombre { get; set; } = string.Empty;

    [Column("edad")]
    [Range(0, 120, ErrorMessage = "Edad debe ser entre 0 y 120")]
    public int Edad { get; set; }

    [Column("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }
}