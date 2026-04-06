using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TasksService.Data.Entities;

namespace TasksService.Data;

public partial class GranjaDbContext : DbContext
{
    public GranjaDbContext(DbContextOptions<GranjaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BajasEjemplares> BajasEjemplares { get; set; }

    public virtual DbSet<CatPropositoAnimal> CatPropositoAnimal { get; set; }

    public virtual DbSet<CatRazonBaja> CatRazonBaja { get; set; }

    public virtual DbSet<CatRazonReduccion> CatRazonReduccion { get; set; }

    public virtual DbSet<CatTipoAdquisicion> CatTipoAdquisicion { get; set; }

    public virtual DbSet<Ejemplares> Ejemplares { get; set; }

    public virtual DbSet<GastosIngresosExtra> GastosIngresosExtra { get; set; }

    public virtual DbSet<Granjas> Granjas { get; set; }

    public virtual DbSet<Grupos> Grupos { get; set; }

    public virtual DbSet<LotesAlimento> LotesAlimento { get; set; }

    public virtual DbSet<MiembrosGranja> MiembrosGranja { get; set; }

    public virtual DbSet<Perfiles> Perfiles { get; set; }

    public virtual DbSet<PeriodosAlimento> PeriodosAlimento { get; set; }

    public virtual DbSet<RecoleccionesHuevo> RecoleccionesHuevo { get; set; }

    public virtual DbSet<ReduccionesHuevo> ReduccionesHuevo { get; set; }

    public virtual DbSet<TipoAnimal> TipoAnimal { get; set; }

    public virtual DbSet<VPuntoEquilibrio> VPuntoEquilibrio { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("rol_miembro", new[] { "owner", "editor", "viewer" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresEnum("tipo_movimiento", new[] { "gasto", "ingreso" })
            .HasPostgresExtension("extensions", "hypopg")
            .HasPostgresExtension("extensions", "index_advisor")
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<BajasEjemplares>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bajas_ejemplares_pkey");

            entity.ToTable("bajas_ejemplares", tb => tb.HasComment("Registro histórico de baja de un ejemplar."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FechaBaja).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.ImporteVenta).HasComment("Requerido cuando cat_razon_baja.genera_ingreso = true.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BajasEjemplares)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bajas_ejemplares_created_by_fkey");

            entity.HasOne(d => d.Ejemplar).WithOne(p => p.BajasEjemplares).HasConstraintName("bajas_ejemplares_ejemplar_id_fkey");

            entity.HasOne(d => d.RazonBaja).WithMany(p => p.BajasEjemplares)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bajas_ejemplares_razon_baja_id_fkey");
        });

        modelBuilder.Entity<CatPropositoAnimal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cat_proposito_animal_pkey");

            entity.ToTable("cat_proposito_animal", tb => tb.HasComment("Catálogo de propósitos de un animal (postura, carne, ornamental...)."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.GranjaId).HasComment("null = valor global predefinido. uuid = valor personalizado de esa granja.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CatPropositoAnimal).HasConstraintName("cat_proposito_animal_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.CatPropositoAnimal)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cat_proposito_animal_granja_id_fkey");
        });

        modelBuilder.Entity<CatRazonBaja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cat_razon_baja_pkey");

            entity.ToTable("cat_razon_baja", tb => tb.HasComment("Catálogo de razones de baja de un ejemplar."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.GeneraIngreso).HasComment("Indica si la baja genera un importe (venta). La app usa esto para mostrar/ocultar el campo de importe.");
            entity.Property(e => e.GranjaId).HasComment("null = valor global predefinido. uuid = valor personalizado de esa granja.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CatRazonBaja).HasConstraintName("cat_razon_baja_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.CatRazonBaja)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cat_razon_baja_granja_id_fkey");
        });

        modelBuilder.Entity<CatRazonReduccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cat_razon_reduccion_pkey");

            entity.ToTable("cat_razon_reduccion", tb => tb.HasComment("Catálogo de razones de reducción del inventario de huevos."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.GeneraIngreso).HasComment("Indica si la reducción genera un importe (venta).");
            entity.Property(e => e.GranjaId).HasComment("null = valor global predefinido. uuid = valor personalizado de esa granja.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CatRazonReduccion).HasConstraintName("cat_razon_reduccion_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.CatRazonReduccion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cat_razon_reduccion_granja_id_fkey");
        });

        modelBuilder.Entity<CatTipoAdquisicion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cat_tipo_adquisicion_pkey");

            entity.ToTable("cat_tipo_adquisicion", tb => tb.HasComment("Catálogo de formas de adquisición de un animal."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.GranjaId).HasComment("null = valor global predefinido. uuid = valor personalizado de esa granja.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CatTipoAdquisicion).HasConstraintName("cat_tipo_adquisicion_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.CatTipoAdquisicion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cat_tipo_adquisicion_granja_id_fkey");
        });

        modelBuilder.Entity<Ejemplares>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ejemplares_pkey");

            entity.ToTable("ejemplares", tb => tb.HasComment("Ejemplar individual de un animal."));

            entity.HasIndex(e => new { e.GranjaId, e.TipoAnimalId, e.Brazalete }, "ejemplares_brazalete_activo_idx")
                .IsUnique()
                .HasFilter("(activo = true)");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasComment("false cuando el ejemplar ha sido dado de baja.");
            entity.Property(e => e.Brazalete).HasComment("Número de brazalete físico. Único por granja+tipo_animal entre activos. Reutilizable tras baja.");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FechaAdquisicion).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.GranjaId).HasComment("Desnormalizado desde grupos para facilitar índices y RLS.");
            entity.Property(e => e.TipoAnimalId).HasComment("Desnormalizado desde grupos para el índice de brazalete.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Ejemplares)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ejemplares_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.Ejemplares).HasConstraintName("ejemplares_granja_id_fkey");

            entity.HasOne(d => d.Grupo).WithMany(p => p.Ejemplares).HasConstraintName("ejemplares_grupo_id_fkey");

            entity.HasOne(d => d.Proposito).WithMany(p => p.Ejemplares)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ejemplares_proposito_id_fkey");

            entity.HasOne(d => d.TipoAdquisicion).WithMany(p => p.Ejemplares)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ejemplares_tipo_adquisicion_id_fkey");

            entity.HasOne(d => d.TipoAnimal).WithMany(p => p.Ejemplares)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ejemplares_tipo_animal_id_fkey");
        });

        modelBuilder.Entity<GastosIngresosExtra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gastos_ingresos_extra_pkey");

            entity.ToTable("gastos_ingresos_extra", tb => tb.HasComment("Gastos e ingresos adicionales asociados a un período de alimento."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GastosIngresosExtra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gastos_ingresos_extra_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.GastosIngresosExtra).HasConstraintName("gastos_ingresos_extra_granja_id_fkey");

            entity.HasOne(d => d.PeriodoAlimento).WithMany(p => p.GastosIngresosExtra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gastos_ingresos_extra_periodo_alimento_id_fkey");
        });

        modelBuilder.Entity<Granjas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("granja_pkey");

            entity.ToTable("granjas", tb => tb.HasComment("Cada granja pertenece a un usuario dueño."));

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GranjasCreatedByNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("granja_created_by_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.GranjasOwner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("granja_owner_id_fkey");
        });

        modelBuilder.Entity<Grupos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("grupos_pkey");

            entity.ToTable("grupos", tb => tb.HasComment("Grupos dentro de un tipo de animal (ej: \"Lote A\", \"Lote B\")."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Grupos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("grupos_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.Grupos).HasConstraintName("grupos_granja_id_fkey");

            entity.HasOne(d => d.TipoAnimal).WithMany(p => p.Grupos).HasConstraintName("grupos_tipo_animal_id_fkey");
        });

        modelBuilder.Entity<LotesAlimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lotes_alimento_pkey");

            entity.ToTable("lotes_alimento", tb => tb.HasComment("Registro de compra de alimento."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FechaCompra).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.PrecioPorKg)
                .HasComputedColumnSql("(precio_total / cantidad_kg)", true)
                .HasComment("Calculado automáticamente: precio_total / cantidad_kg.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LotesAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotes_alimento_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.LotesAlimento).HasConstraintName("lotes_alimento_granja_id_fkey");

            entity.HasOne(d => d.TipoAnimal).WithMany(p => p.LotesAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotes_alimento_tipo_animal_id_fkey");
        });

        modelBuilder.Entity<MiembrosGranja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("miembros_granja_pkey");

            entity.ToTable("miembros_granja", tb => tb.HasComment("Relación N:M entre usuarios y granjas con rol."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Granja).WithMany(p => p.MiembrosGranja).HasConstraintName("miembros_granja_granja_id_fkey");

            entity.HasOne(d => d.InvitedByNavigation).WithMany(p => p.MiembrosGranjaInvitedByNavigation).HasConstraintName("miembros_granja_invited_by_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.MiembrosGranjaUser).HasConstraintName("miembros_granja_user_id_fkey");
        });

        modelBuilder.Entity<Perfiles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("perfiles_pkey");

            entity.ToTable("perfiles", tb => tb.HasComment("Perfil público de cada usuario autenticado."));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<PeriodosAlimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("periodos_alimento_pkey");

            entity.ToTable("periodos_alimento", tb => tb.HasComment("Período de tiempo que abarca un lote de alimento."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasComment("true mientras el lote no se ha agotado.");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FechaInicio).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.KgConsumidos).HasComment("Acumulado actualizado desde la app.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PeriodosAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("periodos_alimento_created_by_fkey");

            entity.HasOne(d => d.LoteAlimento).WithMany(p => p.PeriodosAlimento).HasConstraintName("periodos_alimento_lote_alimento_id_fkey");
        });

        modelBuilder.Entity<RecoleccionesHuevo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recolecciones_huevo_pkey");

            entity.ToTable("recolecciones_huevo", tb => tb.HasComment("Recolección diaria de huevos por tipo de animal."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.PeriodoAlimentoId).HasComment("Período activo al momento de la recolección.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RecoleccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recolecciones_huevo_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.RecoleccionesHuevo).HasConstraintName("recolecciones_huevo_granja_id_fkey");

            entity.HasOne(d => d.PeriodoAlimento).WithMany(p => p.RecoleccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recolecciones_huevo_periodo_alimento_id_fkey");

            entity.HasOne(d => d.TipoAnimal).WithMany(p => p.RecoleccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recolecciones_huevo_tipo_animal_id_fkey");
        });

        modelBuilder.Entity<ReduccionesHuevo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reducciones_huevo_pkey");

            entity.ToTable("reducciones_huevo", tb => tb.HasComment("Salidas del inventario acumulado de huevos en un período."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Importe).HasComment("Solo aplica cuando cat_razon_reduccion.genera_ingreso = true.");
            entity.Property(e => e.PeriodoAlimentoId).HasComment("Reduce del acumulado del período completo, no de una recolección específica.");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReduccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reducciones_huevo_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.ReduccionesHuevo).HasConstraintName("reducciones_huevo_granja_id_fkey");

            entity.HasOne(d => d.PeriodoAlimento).WithMany(p => p.ReduccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reducciones_huevo_periodo_alimento_id_fkey");

            entity.HasOne(d => d.RazonReduccion).WithMany(p => p.ReduccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reducciones_huevo_razon_reduccion_id_fkey");

            entity.HasOne(d => d.TipoAnimal).WithMany(p => p.ReduccionesHuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reducciones_huevo_tipo_animal_id_fkey");
        });

        modelBuilder.Entity<TipoAnimal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipo_animal_pkey");

            entity.ToTable("tipo_animal", tb => tb.HasComment("Catálogo de tipos de animal por granja."));

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TipoAnimal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tipo_animal_created_by_fkey");

            entity.HasOne(d => d.Granja).WithMany(p => p.TipoAnimal).HasConstraintName("tipo_animal_granja_id_fkey");
        });

        modelBuilder.Entity<VPuntoEquilibrio>(entity =>
        {
            entity.ToView("v_punto_equilibrio");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
