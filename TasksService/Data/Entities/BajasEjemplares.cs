using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Registro histórico de baja de un ejemplar.
/// </summary>
[Table("bajas_ejemplares")]
[Index("EjemplarId", Name = "bajas_ejemplares_ejemplar_id_key", IsUnique = true)]
public partial class BajasEjemplares
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("ejemplar_id")]
    public Guid EjemplarId { get; set; }

    [Column("razon_baja_id")]
    public Guid RazonBajaId { get; set; }

    /// <summary>
    /// Requerido cuando cat_razon_baja.genera_ingreso = true.
    /// </summary>
    [Column("importe_venta")]
    [Precision(10, 2)]
    public decimal? ImporteVenta { get; set; }

    [Column("fecha_baja")]
    public DateOnly FechaBaja { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BajasEjemplares")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [ForeignKey("EjemplarId")]
    [InverseProperty("BajasEjemplares")]
    public virtual Ejemplares Ejemplar { get; set; } = null!;

    [ForeignKey("RazonBajaId")]
    [InverseProperty("BajasEjemplares")]
    public virtual CatRazonBaja RazonBaja { get; set; } = null!;
}
