using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Registro de compra de alimento.
/// </summary>
[Table("lotes_alimento")]
public partial class LotesAlimento
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("tipo_animal_id")]
    public Guid TipoAnimalId { get; set; }

    [Column("fecha_compra")]
    public DateOnly FechaCompra { get; set; }

    [Column("cantidad_kg")]
    [Precision(10, 3)]
    public decimal CantidadKg { get; set; }

    [Column("precio_total")]
    [Precision(10, 2)]
    public decimal PrecioTotal { get; set; }

    /// <summary>
    /// Calculado automáticamente: precio_total / cantidad_kg.
    /// </summary>
    [Column("precio_por_kg")]
    [Precision(10, 4)]
    public decimal? PrecioPorKg { get; set; }

    [Column("proveedor")]
    public string? Proveedor { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("LotesAlimento")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [ForeignKey("GranjaId")]
    [InverseProperty("LotesAlimento")]
    public virtual Granjas Granja { get; set; } = null!;

    [InverseProperty("LoteAlimento")]
    public virtual ICollection<PeriodosAlimento> PeriodosAlimento { get; set; } = new List<PeriodosAlimento>();

    [ForeignKey("TipoAnimalId")]
    [InverseProperty("LotesAlimento")]
    public virtual TipoAnimal TipoAnimal { get; set; } = null!;
}
