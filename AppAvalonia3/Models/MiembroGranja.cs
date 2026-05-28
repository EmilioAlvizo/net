using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;

namespace AppAvalonia3.Models;

[Table("miembros_granja")]
public class MiembroGranja : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("granja_id")]
    public string GranjaId { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("rol")]
    public string Rol { get; set; } // Tipo: rol_miembro

    [Column("joined_at")]
    public DateTimeOffset JoinedAt { get; set; }

    [Column("invited_by")]
    public string? InvitedBy { get; set; } // Es opcional (rombo vacío)

    // ==========================================
    // Propiedades de Navegación (Relaciones)
    // ==========================================

    [Reference(typeof(Farm), includeInQuery: false)]
    public Farm Granjas { get; set; }

    [Reference(typeof(Perfil), includeInQuery: false)]
    public Perfil Perfiles { get; set; }

}

[Table("miembros_granja")]
public class MiembroGranjaWrite  : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;

    [Column("granja_id")]
    public string GranjaId { get; set; } = string.Empty;

    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [Column("rol")]
    public string Rol { get; set; } = string.Empty;

    [Column("joined_at")]
    public DateTimeOffset JoinedAt { get; set; }

    [Column("invited_by")]
    public string? InvitedBy { get; set; }

}