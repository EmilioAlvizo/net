using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Perfil público de cada usuario autenticado.
/// </summary>
[Table("perfiles")]
public partial class Perfiles
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BajasEjemplares> BajasEjemplares { get; set; } = new List<BajasEjemplares>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CatPropositoAnimal> CatPropositoAnimal { get; set; } = new List<CatPropositoAnimal>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CatRazonBaja> CatRazonBaja { get; set; } = new List<CatRazonBaja>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CatRazonReduccion> CatRazonReduccion { get; set; } = new List<CatRazonReduccion>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CatTipoAdquisicion> CatTipoAdquisicion { get; set; } = new List<CatTipoAdquisicion>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Ejemplares> Ejemplares { get; set; } = new List<Ejemplares>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<GastosIngresosExtra> GastosIngresosExtra { get; set; } = new List<GastosIngresosExtra>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Granjas> GranjasCreatedByNavigation { get; set; } = new List<Granjas>();

    [InverseProperty("Owner")]
    public virtual ICollection<Granjas> GranjasOwner { get; set; } = new List<Granjas>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Grupos> Grupos { get; set; } = new List<Grupos>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<LotesAlimento> LotesAlimento { get; set; } = new List<LotesAlimento>();

    [InverseProperty("InvitedByNavigation")]
    public virtual ICollection<MiembrosGranja> MiembrosGranjaInvitedByNavigation { get; set; } = new List<MiembrosGranja>();

    [InverseProperty("User")]
    public virtual ICollection<MiembrosGranja> MiembrosGranjaUser { get; set; } = new List<MiembrosGranja>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PeriodosAlimento> PeriodosAlimento { get; set; } = new List<PeriodosAlimento>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<RecoleccionesHuevo> RecoleccionesHuevo { get; set; } = new List<RecoleccionesHuevo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReduccionesHuevo> ReduccionesHuevo { get; set; } = new List<ReduccionesHuevo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TipoAnimal> TipoAnimal { get; set; } = new List<TipoAnimal>();
}
