using Microsoft.EntityFrameworkCore;
using TasksService.Models;

namespace TasksService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    // Esta línea = tabla "Tasks" en la base de datos
    public DbSet<TaskItem> Tasks { get; set; }
    
}