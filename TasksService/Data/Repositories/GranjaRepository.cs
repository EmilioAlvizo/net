using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Entities;
using TasksService.Models.DTOs.GranjasDto;
using TasksService.Repositories.Interfaces;

namespace TasksService.Repositories;

public class GranjaRepository : IGranjaRepository
{
    private readonly AppDbContext _ctx;

    public GranjaRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Granja>> GetAllAsync() =>
        await _ctx.Granjas.ToListAsync();

    public async Task<IEnumerable<Granja>> GetByOwnerAsync(Guid ownerId) =>
        await _ctx.Granjas.Where(g => g.OwnerId == ownerId).ToListAsync();

    public async Task<Granja?> GetByIdAsync(Guid id) =>
        await _ctx.Granjas.FindAsync(id);

    public async Task<Granja> CreateAsync(Granja granja)
    {
        _ctx.Granjas.Add(granja);
        await _ctx.SaveChangesAsync();
        return granja;
    }

    public async Task<Granja?> UpdateAsync(Guid id, UpdateGranjaDto dto)
    {
        var granja = await _ctx.Granjas.FindAsync(id);
        if (granja is null) return null;

        if (dto.Nombre is not null) granja.Nombre = dto.Nombre;
        if (dto.Descripcion is not null) granja.Descripcion = dto.Descripcion;
        if (dto.Ubicacion is not null) granja.Ubicacion = dto.Ubicacion;

        await _ctx.SaveChangesAsync();
        return granja;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var granja = await _ctx.Granjas.FindAsync(id);
        if (granja is null) return false;

        _ctx.Granjas.Remove(granja);
        await _ctx.SaveChangesAsync();
        return true;
    }
}