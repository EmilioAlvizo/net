using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Catálogo de razones de baja de un ejemplar.
/// </summary>
[Table("cat_razon_baja")]
[Index("GranjaId", "Nombre", Name = "cat_razon_baja_granja_id_nombre_key", IsUnique = true)]
public partial class CatRazonBaja
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
    /// Indica si la baja genera un importe (venta). La app usa esto para mostrar/ocultar el campo de importe.
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

    [InverseProperty("RazonBaja")]
    public virtual ICollection<BajasEjemplares> BajasEjemplares { get; set; } = new List<BajasEjemplares>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("CatRazonBaja")]
    public virtual Perfiles? CreatedByNavigation { get; set; }

    [ForeignKey("GranjaId")]
    [InverseProperty("CatRazonBaja")]
    public virtual Granjas? Granja { get; set; }
}
