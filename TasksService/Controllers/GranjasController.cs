using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksService.Entities;
using TasksService.Models.DTOs.GranjasDto;
using TasksService.Repositories.Interfaces;
using System.Security.Claims;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GranjasController : ControllerBase
{
    private readonly IGranjaRepository _repo;

    public GranjasController(IGranjaRepository repo) => _repo = repo;

    /// <summary>Obtiene todas las granjas</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repo.GetAllAsync());

    /// <summary>Obtiene las granjas del usuario autenticado</summary>
    [HttpGet("mis-granjas")]
    public async Task<IActionResult> GetMias()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")!);
        return Ok(await _repo.GetByOwnerAsync(userId));
    }

    /// <summary>Obtiene una granja por ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var granja = await _repo.GetByIdAsync(id);
        return granja is null ? NotFound() : Ok(granja);
    }

    /// <summary>Crea una nueva granja</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGranjaDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")!);

        var granja = new Granja
        {
            Id = Guid.NewGuid(),
            OwnerId = userId,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Ubicacion = dto.Ubicacion,
            CreatedBy = userId
        };

        var created = await _repo.CreateAsync(granja);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualiza una granja existente</summary>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGranjaDto dto)
    {
        var updated = await _repo.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Elimina una granja</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}