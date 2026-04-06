using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Cada granja pertenece a un usuario dueño.
/// </summary>
[Table("granjas")]
public partial class Granjas
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("owner_id")]
    public Guid OwnerId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("ubicacion")]
    public string? Ubicacion { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Granja")]
    public virtual ICollection<CatPropositoAnimal> CatPropositoAnimal { get; set; } = new List<CatPropositoAnimal>();

    [InverseProperty("Granja")]
    public virtual ICollection<CatRazonBaja> CatRazonBaja { get; set; } = new List<CatRazonBaja>();

    [InverseProperty("Granja")]
    public virtual ICollection<CatRazonReduccion> CatRazonReduccion { get; set; } = new List<CatRazonReduccion>();

    [InverseProperty("Granja")]
    public virtual ICollection<CatTipoAdquisicion> CatTipoAdquisicion { get; set; } = new List<CatTipoAdquisicion>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("GranjasCreatedByNavigation")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Granja")]
    public virtual ICollection<Ejemplares> Ejemplares { get; set; } = new List<Ejemplares>();

    [InverseProperty("Granja")]
    public virtual ICollection<GastosIngresosExtra> GastosIngresosExtra { get; set; } = new List<GastosIngresosExtra>();

    [InverseProperty("Granja")]
    public virtual ICollection<Grupos> Grupos { get; set; } = new List<Grupos>();

    [InverseProperty("Granja")]
    public virtual ICollection<LotesAlimento> LotesAlimento { get; set; } = new List<LotesAlimento>();

    [InverseProperty("Granja")]
    public virtual ICollection<MiembrosGranja> MiembrosGranja { get; set; } = new List<MiembrosGranja>();

    [ForeignKey("OwnerId")]
    [InverseProperty("GranjasOwner")]
    public virtual Perfiles Owner { get; set; } = null!;

    [InverseProperty("Granja")]
    public virtual ICollection<RecoleccionesHuevo> RecoleccionesHuevo { get; set; } = new List<RecoleccionesHuevo>();

    [InverseProperty("Granja")]
    public virtual ICollection<ReduccionesHuevo> ReduccionesHuevo { get; set; } = new List<ReduccionesHuevo>();

    [InverseProperty("Granja")]
    public virtual ICollection<TipoAnimal> TipoAnimal { get; set; } = new List<TipoAnimal>();
}
