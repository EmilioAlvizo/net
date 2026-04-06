using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Catálogo de formas de adquisición de un animal.
/// </summary>
[Table("cat_tipo_adquisicion")]
[Index("GranjaId", "Nombre", Name = "cat_tipo_adquisicion_granja_id_nombre_key", IsUnique = true)]
public partial class CatTipoAdquisicion
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
    [InverseProperty("CatTipoAdquisicion")]
    public virtual Perfiles? CreatedByNavigation { get; set; }

    [InverseProperty("TipoAdquisicion")]
    public virtual ICollection<Ejemplares> Ejemplares { get; set; } = new List<Ejemplares>();

    [ForeignKey("GranjaId")]
    [InverseProperty("CatTipoAdquisicion")]
    public virtual Granjas? Granja { get; set; }
}
