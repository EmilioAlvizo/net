using Microsoft.EntityFrameworkCore;
using TasksService.Data.Entities;
using TasksService.Data;
public interface IEjemplarRepository : IRepository<Ejemplares>
{
    Task<IEnumerable<int>> GetBrazoletesDisponiblesAsync(Guid granjaId, Guid tipoAnimalId, int hasta = 50);
    Task<IEnumerable<Ejemplares>> GetActivosByGrupoAsync(Guid grupoId);
}

public class EjemplarRepository : Repository<Ejemplares>, IEjemplarRepository
{
    public EjemplarRepository(GranjaDbContext context) : base(context) { }

    public async Task<IEnumerable<int>> GetBrazoletesDisponiblesAsync(
        Guid granjaId, Guid tipoAnimalId, int hasta = 50)
    {
        // Equivalente a la query generate_series del schema
        var enUso = await _context.Ejemplares
            .Where(e => e.GranjaId == granjaId
                     && e.TipoAnimalId == tipoAnimalId
                     && e.Activo == true)
            .Select(e => e.Brazalete)
            .ToListAsync();

        return Enumerable.Range(1, hasta)
            .Where(n => !enUso.Contains((short)n));
    }

    public async Task<IEnumerable<Ejemplares>> GetActivosByGrupoAsync(Guid grupoId) =>
        await _context.Ejemplares
            .Where(e => e.GrupoId == grupoId && e.Activo == true)
            .OrderBy(e => e.Brazalete)
            .ToListAsync();
}