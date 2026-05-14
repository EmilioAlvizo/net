using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAvalonia3.Models;

[Table("miembros_granja")]
public class MiembroGranja
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("granja_id")]
    public Guid GranjaId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("rol")]
    public string Rol { get; set; } // Tipo: rol_miembro

    [Column("joined_at")]
    public DateTimeOffset JoinedAt { get; set; }

    [Column("invited_by")]
    public Guid? InvitedBy { get; set; } // Es opcional (rombo vacío)

    // ==========================================
    // Propiedades de Navegación (Relaciones)
    // ==========================================

    [ForeignKey("GranjaId")]
    public Farm Granja { get; set; }

    [ForeignKey("UserId")]
    public Perfil Perfil { get; set; }
}