using Microsoft.EntityFrameworkCore;
using TasksService.Data.Entities;
using TasksService.Data;

public interface IEjemplarService
{
    Task<IEnumerable<Ejemplares>> GetActivosByGrupoAsync(Guid grupoId);
    Task<Ejemplares> RegistrarAsync(Ejemplares ejemplar);
    Task DarDeBajaAsync(Guid ejemplarId, BajasEjemplares baja);
    Task<IEnumerable<int>> GetBrazoletesDisponiblesAsync(Guid granjaId, Guid tipoAnimalId);
}

public class EjemplarService : IEjemplarService
{
    private readonly IEjemplarRepository _repo;
    private readonly GranjaDbContext _context;

    public EjemplarService(IEjemplarRepository repo, GranjaDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public Task<IEnumerable<Ejemplares>> GetActivosByGrupoAsync(Guid grupoId) =>
        _repo.GetActivosByGrupoAsync(grupoId);

    public async Task<Ejemplares> RegistrarAsync(Ejemplares ejemplar)
    {
        // Validar que el brazalete no esté en uso
        var enUso = await _context.Ejemplares.AnyAsync(e =>
            e.GranjaId      == ejemplar.GranjaId &&
            e.TipoAnimalId  == ejemplar.TipoAnimalId &&
            e.Brazalete     == ejemplar.Brazalete &&
            e.Activo        == true);

        if (enUso)
            throw new InvalidOperationException(
                $"El brazalete {ejemplar.Brazalete} ya está en uso para este tipo de animal.");

        ejemplar.Activo = true;
        return await _repo.CreateAsync(ejemplar);
    }

    public async Task DarDeBajaAsync(Guid ejemplarId, BajasEjemplares baja)
    {
        var ejemplar = await _repo.GetByIdAsync(ejemplarId)
            ?? throw new KeyNotFoundException("Ejemplares no encontrado.");

        if (!ejemplar.Activo)
            throw new InvalidOperationException("El ejemplar ya fue dado de baja.");

        baja.EjemplarId = ejemplarId;

        // El trigger en BD marca activo = false automáticamente
        await _context.BajasEjemplares.AddAsync(baja);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<int>> GetBrazoletesDisponiblesAsync(
        Guid granjaId, Guid tipoAnimalId) =>
        _repo.GetBrazoletesDisponiblesAsync(granjaId, tipoAnimalId);
}