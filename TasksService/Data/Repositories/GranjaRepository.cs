using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Data.Entities;

namespace TasksService.Data.Repositories;

public interface IGranjaRepository : IRepository<Granjas>
{
    // Todas las granjas donde el usuario es miembro (owner, editor o viewer)
    Task<IEnumerable<Granjas>> GetByUsuarioAsync(Guid userId);

    // Solo las granjas donde el usuario es owner
    Task<IEnumerable<Granjas>> GetPropiasByUsuarioAsync(Guid userId);
}

public class GranjaRepository : Repository<Granjas>, IGranjaRepository
{
    public GranjaRepository(GranjaDbContext context) : base(context) { }

    public async Task<IEnumerable<Granjas>> GetByUsuarioAsync(Guid userId)
    {
        return await _context.Granjas
            .Where(g => g.MiembrosGranja.Any(m => m.UserId == userId))
            .OrderBy(g => g.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Granjas>> GetPropiasByUsuarioAsync(Guid userId)
    {
        return await _context.Granjas
            .Where(g => g.OwnerId == userId)
            .OrderBy(g => g.Nombre)
            .ToListAsync();
    }
}