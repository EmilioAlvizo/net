using TasksService.Entities;
using TasksService.Models.DTOs.PerfilesDto;

namespace TasksService.Repositories.Interfaces;

public interface IPerfilRepository
{
    Task<IEnumerable<Perfil>> GetAllAsync();
    Task<Perfil?> GetByIdAsync(Guid id);
    Task<Perfil> CreateAsync(Perfil perfil);
    Task<Perfil?> UpdateAsync(Guid id, UpdatePerfilDto dto);
    Task<bool> DeleteAsync(Guid id);
}