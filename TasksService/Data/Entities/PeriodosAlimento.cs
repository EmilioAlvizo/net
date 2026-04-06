using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Período de tiempo que abarca un lote de alimento.
/// </summary>
[Table("periodos_alimento")]
public partial class PeriodosAlimento
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("lote_alimento_id")]
    public Guid LoteAlimentoId { get; set; }

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly? FechaFin { get; set; }

    /// <summary>
    /// true mientras el lote no se ha agotado.
    /// </summary>
    [Column("activo")]
    public bool Activo { get; set; }

    /// <summary>
    /// Acumulado actualizado desde la app.
    /// </summary>
    [Column("kg_consumidos")]
    [Precision(10, 3)]
    public decimal KgConsumidos { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PeriodosAlimento")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [InverseProperty("PeriodoAlimento")]
    public virtual ICollection<GastosIngresosExtra> GastosIngresosExtra { get; set; } = new List<GastosIngresosExtra>();

    [ForeignKey("LoteAlimentoId")]
    [InverseProperty("PeriodosAlimento")]
    public virtual LotesAlimento LoteAlimento { get; set; } = null!;

    [InverseProperty("PeriodoAlimento")]
    public virtual ICollection<RecoleccionesHuevo> RecoleccionesHuevo { get; set; } = new List<RecoleccionesHuevo>();

    [InverseProperty("PeriodoAlimento")]
    public virtual ICollection<ReduccionesHuevo> ReduccionesHuevo { get; set; } = new List<ReduccionesHuevo>();
}
