using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Entities;
using TasksService.Models.DTOs.PerfilesDto;
using TasksService.Repositories.Interfaces;

namespace TasksService.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly AppDbContext _ctx;

    public PerfilRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Perfil>> GetAllAsync() =>
        await _ctx.Perfiles.ToListAsync();

    public async Task<Perfil?> GetByIdAsync(Guid id) =>
        await _ctx.Perfiles.FindAsync(id);

    public async Task<Perfil> CreateAsync(Perfil perfil)
    {
        _ctx.Perfiles.Add(perfil);
        await _ctx.SaveChangesAsync();
        return perfil;
    }

    public async Task<Perfil?> UpdateAsync(Guid id, UpdatePerfilDto dto)
    {
        var perfil = await _ctx.Perfiles.FindAsync(id);
        if (perfil is null) return null;

        if (dto.Nombre is not null) perfil.Nombre = dto.Nombre;
        if (dto.AvatarUrl is not null) perfil.AvatarUrl = dto.AvatarUrl;

        await _ctx.SaveChangesAsync();
        return perfil;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var perfil = await _ctx.Perfiles.FindAsync(id);
        if (perfil is null) return false;

        _ctx.Perfiles.Remove(perfil);
        await _ctx.SaveChangesAsync();
        return true;
    }
}