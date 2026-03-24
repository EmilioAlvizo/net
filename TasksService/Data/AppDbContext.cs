using Microsoft.EntityFrameworkCore;
using TasksService.Models;

namespace TasksService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Producto> Productos { get; set; }
    
}