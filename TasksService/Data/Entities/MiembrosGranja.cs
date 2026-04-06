using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.Data.Entities;

/// <summary>
/// Relación N:M entre usuarios y granjas con rol.
/// </summary>
[Table("miembros_granja")]
[Index("GranjaId", "UserId", Name = "miembros_granja_granja_id_user_id_key", IsUnique = true)]
public partial class MiembrosGranja
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("joined_at")]
    public DateTime JoinedAt { get; set; }

    [Column("invited_by")]
    public Guid? InvitedBy { get; set; }

    [ForeignKey("GranjaId")]
    [InverseProperty("MiembrosGranja")]
    public virtual Granjas Granja { get; set; } = null!;

    [ForeignKey("InvitedBy")]
    [InverseProperty("MiembrosGranjaInvitedByNavigation")]
    public virtual Perfiles? InvitedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("MiembrosGranjaUser")]
    public virtual Perfiles User { get; set; } = null!;
}
