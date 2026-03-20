using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksService.Models;

[Table("clientes")]  // nombre exacto de la tabla en la BD
public class Cliente
{
    [Key]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("edad")]
    public int Edad { get; set; }

    [Column("fecha_nacimiento")]
    public DateTime FechaNacimiento { get; set; }
}