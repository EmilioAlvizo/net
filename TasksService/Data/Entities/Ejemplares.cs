using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Ejemplar individual de un animal.
/// </summary>
[Table("ejemplares")]
public partial class Ejemplares
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Desnormalizado desde grupos para facilitar índices y RLS.
    /// </summary>
    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    /// <summary>
    /// Desnormalizado desde grupos para el índice de brazalete.
    /// </summary>
    [Column("tipo_animal_id")]
    public Guid TipoAnimalId { get; set; }

    [Column("grupo_id")]
    public Guid GrupoId { get; set; }

    /// <summary>
    /// Número de brazalete físico. Único por granja+tipo_animal entre activos. Reutilizable tras baja.
    /// </summary>
    [Column("brazalete")]
    public short Brazalete { get; set; }

    [Column("proposito_id")]
    public Guid PropositoId { get; set; }

    [Column("tipo_adquisicion_id")]
    public Guid TipoAdquisicionId { get; set; }

    [Column("fecha_adquisicion")]
    public DateOnly FechaAdquisicion { get; set; }

    [Column("costo_adquisicion")]
    [Precision(10, 2)]
    public decimal? CostoAdquisicion { get; set; }

    /// <summary>
    /// false cuando el ejemplar ha sido dado de baja.
    /// </summary>
    [Column("activo")]
    public bool Activo { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [InverseProperty("Ejemplar")]
    public virtual BajasEjemplares? BajasEjemplares { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Ejemplares")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [ForeignKey("GranjaId")]
    [InverseProperty("Ejemplares")]
    public virtual Granjas Granja { get; set; } = null!;

    [ForeignKey("GrupoId")]
    [InverseProperty("Ejemplares")]
    public virtual Grupos Grupo { get; set; } = null!;

    [ForeignKey("PropositoId")]
    [InverseProperty("Ejemplares")]
    public virtual CatPropositoAnimal Proposito { get; set; } = null!;

    [ForeignKey("TipoAdquisicionId")]
    [InverseProperty("Ejemplares")]
    public virtual CatTipoAdquisicion TipoAdquisicion { get; set; } = null!;

    [ForeignKey("TipoAnimalId")]
    [InverseProperty("Ejemplares")]
    public virtual TipoAnimal TipoAnimal { get; set; } = null!;
}
