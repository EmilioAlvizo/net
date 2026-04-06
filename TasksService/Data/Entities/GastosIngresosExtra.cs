using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Gastos e ingresos adicionales asociados a un período de alimento.
/// </summary>
[Table("gastos_ingresos_extra")]
public partial class GastosIngresosExtra
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("periodo_alimento_id")]
    public Guid PeriodoAlimentoId { get; set; }

    [Column("concepto")]
    public string Concepto { get; set; } = null!;

    [Column("importe")]
    [Precision(10, 2)]
    public decimal Importe { get; set; }

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("GastosIngresosExtra")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [ForeignKey("GranjaId")]
    [InverseProperty("GastosIngresosExtra")]
    public virtual Granjas Granja { get; set; } = null!;

    [ForeignKey("PeriodoAlimentoId")]
    [InverseProperty("GastosIngresosExtra")]
    public virtual PeriodosAlimento PeriodoAlimento { get; set; } = null!;
}
