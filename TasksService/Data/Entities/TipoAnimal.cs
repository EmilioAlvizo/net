using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Catálogo de tipos de animal por granja.
/// </summary>
[Table("tipo_animal")]
[Index("GranjaId", "Nombre", Name = "tipo_animal_granja_id_nombre_key", IsUnique = true)]
public partial class TipoAnimal
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TipoAnimal")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [InverseProperty("TipoAnimal")]
    public virtual ICollection<Ejemplares> Ejemplares { get; set; } = new List<Ejemplares>();

    [ForeignKey("GranjaId")]
    [InverseProperty("TipoAnimal")]
    public virtual Granjas Granja { get; set; } = null!;

    [InverseProperty("TipoAnimal")]
    public virtual ICollection<Grupos> Grupos { get; set; } = new List<Grupos>();

    [InverseProperty("TipoAnimal")]
    public virtual ICollection<LotesAlimento> LotesAlimento { get; set; } = new List<LotesAlimento>();

    [InverseProperty("TipoAnimal")]
    public virtual ICollection<RecoleccionesHuevo> RecoleccionesHuevo { get; set; } = new List<RecoleccionesHuevo>();

    [InverseProperty("TipoAnimal")]
    public virtual ICollection<ReduccionesHuevo> ReduccionesHuevo { get; set; } = new List<ReduccionesHuevo>();
}
