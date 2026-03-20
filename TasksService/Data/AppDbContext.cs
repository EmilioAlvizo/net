using Microsoft.EntityFrameworkCore;
using TasksService.Models;

namespace TasksService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Cliente> Clientes { get; set; }
    
}