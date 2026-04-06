using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Catálogo de propósitos de un animal (postura, carne, ornamental...).
/// </summary>
[Table("cat_proposito_animal")]
[Index("GranjaId", "Nombre", Name = "cat_proposito_animal_granja_id_nombre_key", IsUnique = true)]
public partial class CatPropositoAnimal
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// null = valor global predefinido. uuid = valor personalizado de esa granja.
    /// </summary>
    [Column("granja_id")]
    public Guid? GranjaId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("orden")]
    public short Orden { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CatPropositoAnimal")]
    public virtual Perfiles? CreatedByNavigation { get; set; }

    [InverseProperty("Proposito")]
    public virtual ICollection<Ejemplares> Ejemplares { get; set; } = new List<Ejemplares>();

    [ForeignKey("GranjaId")]
    [InverseProperty("CatPropositoAnimal")]
    public virtual Granjas? Granja { get; set; }
}
