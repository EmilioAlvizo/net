using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksService.Entities;
using TasksService.Models.DTOs.PerfilesDto;
using TasksService.Repositories.Interfaces;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PerfilesController : ControllerBase
{
    private readonly IPerfilRepository _repo;

    public PerfilesController(IPerfilRepository repo) => _repo = repo;

    /// <summary>Obtiene todos los perfiles</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repo.GetAllAsync());

    /// <summary>Obtiene un perfil por ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var perfil = await _repo.GetByIdAsync(id);
        return perfil is null ? NotFound() : Ok(perfil);
    }

    /// <summary>Crea un nuevo perfil</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePerfilDto dto)
    {
        var perfil = new Perfil { Id = Guid.NewGuid(), Nombre = dto.Nombre, AvatarUrl = dto.AvatarUrl };
        var created = await _repo.CreateAsync(perfil);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualiza un perfil existente</summary>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePerfilDto dto)
    {
        var updated = await _repo.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Elimina un perfil</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}