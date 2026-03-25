// Models/Producto.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TasksService.Models;

[Table("productos")]  // nombre exacto de la tabla en la BD
public class Producto
{
    [Key]
    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("cliente")]
    public int? ClienteId { get; set; }

    [Column("codigo")]
    public string? Codigo { get; set; } = string.Empty;

    [Column("precio")]
    public double? Precio { get; set; }

    [Column("cantidad")]
    public int? Cantidad { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("fcaptura")]
    public DateTime? FCaptura { get; set; }
}