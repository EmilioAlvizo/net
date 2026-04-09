using TasksService.Entities;
using TasksService.Models.DTOs.GranjasDto;

namespace TasksService.Repositories.Interfaces;

public interface IGranjaRepository
{
    Task<IEnumerable<Granja>> GetAllAsync();
    Task<IEnumerable<Granja>> GetByOwnerAsync(Guid ownerId);
    Task<Granja?> GetByIdAsync(Guid id);
    Task<Granja> CreateAsync(Granja granja);
    Task<Granja?> UpdateAsync(Guid id, UpdateGranjaDto dto);
    Task<bool> DeleteAsync(Guid id);
}