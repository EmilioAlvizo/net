using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

[Keyless]
public partial class VPuntoEquilibrio
{
    [Column("periodo_id")]
    public Guid? PeriodoId { get; set; }

    [Column("granja_id")]
    public Guid? GranjaId { get; set; }

    [Column("tipo_animal_id")]
    public Guid? TipoAnimalId { get; set; }

    [Column("fecha_inicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly? FechaFin { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("kg_comprados")]
    [Precision(10, 3)]
    public decimal? KgComprados { get; set; }

    [Column("costo_alimento")]
    [Precision(10, 2)]
    public decimal? CostoAlimento { get; set; }

    [Column("kg_consumidos")]
    [Precision(10, 3)]
    public decimal? KgConsumidos { get; set; }

    [Column("num_aves")]
    public long? NumAves { get; set; }

    [Column("total_huevos")]
    public long? TotalHuevos { get; set; }

    [Column("huevos_buenos")]
    public long? HuevosBuenos { get; set; }

    [Column("gastos_extra")]
    public decimal? GastosExtra { get; set; }

    [Column("ingresos_extra")]
    public decimal? IngresosExtra { get; set; }

    [Column("dias_periodo")]
    public int? DiasPeriodo { get; set; }

    [Column("kg_por_ave_dia")]
    public decimal? KgPorAveDia { get; set; }

    [Column("huevos_por_ave_dia")]
    public decimal? HuevosPorAveDia { get; set; }

    [Column("costo_total_periodo")]
    public decimal? CostoTotalPeriodo { get; set; }

    [Column("punto_equilibrio_por_huevo")]
    public decimal? PuntoEquilibrioPorHuevo { get; set; }
}
