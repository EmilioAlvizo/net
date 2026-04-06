using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Recolección diaria de huevos por tipo de animal.
/// </summary>
[Table("recolecciones_huevo")]
[Index("GranjaId", "TipoAnimalId", "Fecha", Name = "recolecciones_huevo_granja_id_tipo_animal_id_fecha_key", IsUnique = true)]
public partial class RecoleccionesHuevo
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("tipo_animal_id")]
    public Guid TipoAnimalId { get; set; }

    /// <summary>
    /// Período activo al momento de la recolección.
    /// </summary>
    [Column("periodo_alimento_id")]
    public Guid PeriodoAlimentoId { get; set; }

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("huevos_buenos")]
    public int HuevosBuenos { get; set; }

    [Column("huevos_rotos")]
    public int HuevosRotos { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("RecoleccionesHuevo")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [ForeignKey("GranjaId")]
    [InverseProperty("RecoleccionesHuevo")]
    public virtual Granjas Granja { get; set; } = null!;

    [ForeignKey("PeriodoAlimentoId")]
    [InverseProperty("RecoleccionesHuevo")]
    public virtual PeriodosAlimento PeriodoAlimento { get; set; } = null!;

    [ForeignKey("TipoAnimalId")]
    [InverseProperty("RecoleccionesHuevo")]
    public virtual TipoAnimal TipoAnimal { get; set; } = null!;
}
