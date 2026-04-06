using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Catálogo de razones de reducción del inventario de huevos.
/// </summary>
[Table("cat_razon_reduccion")]
[Index("GranjaId", "Nombre", Name = "cat_razon_reduccion_granja_id_nombre_key", IsUnique = true)]
public partial class CatRazonReduccion
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// null = valor global predefinido. uuid = valor personalizado de esa granja.
    /// </summary>
    [Column("granja_id")]
    public Guid? GranjaId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Indica si la reducción genera un importe (venta).
    /// </summary>
    [Column("genera_ingreso")]
    public bool GeneraIngreso { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("orden")]
    public short Orden { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CatRazonReduccion")]
    public virtual Perfiles? CreatedByNavigation { get; set; }

    [ForeignKey("GranjaId")]
    [InverseProperty("CatRazonReduccion")]
    public virtual Granjas? Granja { get; set; }

    [InverseProperty("RazonReduccion")]
    public virtual ICollection<ReduccionesHuevo> ReduccionesHuevo { get; set; } = new List<ReduccionesHuevo>();
}
