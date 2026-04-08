using TasksService.Data;
using TasksService.Data.Entities;
using TasksService.Data.Repositories;
using TasksService.Models.DTOs;

namespace TasksService.Services;

public interface IGranjaService
{
    Task<IEnumerable<LeerGranjaDto>> GetByUsuarioAsync(Guid userId);
    Task<LeerGranjaDto?>             GetByIdAsync(Guid id);
    Task<LeerGranjaDto>              CrearAsync(CrearGranjaDto dto, Guid userId);
    Task<LeerGranjaDto>              ActualizarAsync(Guid id, ActualizarGranjaDto dto, Guid userId);
    Task                             EliminarAsync(Guid id, Guid userId);
}

public class GranjaService : IGranjaService
{
    private readonly IGranjaRepository _repo;

    public GranjaService(IGranjaRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<LeerGranjaDto>> GetByUsuarioAsync(Guid userId)
    {
        var granjas = await _repo.GetByUsuarioAsync(userId);
        return granjas.Select(ToDto);
    }

    public async Task<LeerGranjaDto?> GetByIdAsync(Guid id)
    {
        var granja = await _repo.GetByIdAsync(id);
        return granja is null ? null : ToDto(granja);
    }

    public async Task<LeerGranjaDto> CrearAsync(CrearGranjaDto dto, Guid userId)
    {
        var granja = new Granjas
        {
            Id          = Guid.NewGuid(),
            OwnerId     = userId,
            Nombre      = dto.Nombre,
            Descripcion = dto.Descripcion,
            Ubicacion   = dto.Ubicacion,
            CreatedAt   = DateTime.UtcNow,
            CreatedBy   = userId
        };

        var creada = await _repo.CreateAsync(granja);
        // El trigger en BD inserta automáticamente al owner en granja_miembros
        return ToDto(creada);
    }

    public async Task<LeerGranjaDto> ActualizarAsync(Guid id, ActualizarGranjaDto dto, Guid userId)
    {
        var granja = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Granja no encontrada.");

        // Solo el owner puede editar
        if (granja.OwnerId != userId)
            throw new UnauthorizedAccessException("Solo el dueño puede editar la granja.");

        granja.Nombre      = dto.Nombre;
        granja.Descripcion = dto.Descripcion;
        granja.Ubicacion   = dto.Ubicacion;

        var actualizada = await _repo.UpdateAsync(granja);
        return ToDto(actualizada);
    }

    public async Task EliminarAsync(Guid id, Guid userId)
    {
        var granja = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Granja no encontrada.");

        if (granja.OwnerId != userId)
            throw new UnauthorizedAccessException("Solo el dueño puede eliminar la granja.");

        await _repo.DeleteAsync(id);
    }

    // ── Mapeo entidad → DTO ──────────────────────────────
    private static LeerGranjaDto ToDto(Granjas g) => new()
    {
        Id          = g.Id,
        Nombre      = g.Nombre,
        Descripcion = g.Descripcion,
        Ubicacion   = g.Ubicacion,
        OwnerId     = g.OwnerId
    };
}