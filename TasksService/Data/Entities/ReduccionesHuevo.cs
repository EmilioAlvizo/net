using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Salidas del inventario acumulado de huevos en un período.
/// </summary>
[Table("reducciones_huevo")]
public partial class ReduccionesHuevo
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("tipo_animal_id")]
    public Guid TipoAnimalId { get; set; }

    /// <summary>
    /// Reduce del acumulado del período completo, no de una recolección específica.
    /// </summary>
    [Column("periodo_alimento_id")]
    public Guid PeriodoAlimentoId { get; set; }

    [Column("razon_reduccion_id")]
    public Guid RazonReduccionId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    /// <summary>
    /// Solo aplica cuando cat_razon_reduccion.genera_ingreso = true.
    /// </summary>
    [Column("importe")]
    [Precision(10, 2)]
    public decimal? Importe { get; set; }

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReduccionesHuevo")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [ForeignKey("GranjaId")]
    [InverseProperty("ReduccionesHuevo")]
    public virtual Granjas Granja { get; set; } = null!;

    [ForeignKey("PeriodoAlimentoId")]
    [InverseProperty("ReduccionesHuevo")]
    public virtual PeriodosAlimento PeriodoAlimento { get; set; } = null!;

    [ForeignKey("RazonReduccionId")]
    [InverseProperty("ReduccionesHuevo")]
    public virtual CatRazonReduccion RazonReduccion { get; set; } = null!;

    [ForeignKey("TipoAnimalId")]
    [InverseProperty("ReduccionesHuevo")]
    public virtual TipoAnimal TipoAnimal { get; set; } = null!;
}
