using Microsoft.EntityFrameworkCore;
using TasksService.Entities;

namespace TasksService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Perfil> Perfiles => Set<Perfil>();
    public DbSet<Granja> Granjas => Set<Granja>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Perfil>().ToTable("perfiles");
        modelBuilder.Entity<Granja>().ToTable("granjas");
    }
}