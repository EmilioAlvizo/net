using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Grupos dentro de un tipo de animal (ej: &quot;Lote A&quot;, &quot;Lote B&quot;).
/// </summary>
[Table("grupos")]
[Index("GranjaId", "Nombre", Name = "grupos_granja_id_nombre_key", IsUnique = true)]
public partial class Grupos
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("tipo_animal_id")]
    public Guid TipoAnimalId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Grupos")]
    public virtual Perfiles CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Grupo")]
    public virtual ICollection<Ejemplares> Ejemplares { get; set; } = new List<Ejemplares>();

    [ForeignKey("GranjaId")]
    [InverseProperty("Grupos")]
    public virtual Granjas Granja { get; set; } = null!;

    [ForeignKey("TipoAnimalId")]
    [InverseProperty("Grupos")]
    public virtual TipoAnimal TipoAnimal { get; set; } = null!;
}
