using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Data.Entities;
using TasksService.Services;
using TasksService.Models.DTOs;

namespace TasksService.Controllers;

[ApiController]
[Route("api/gallinas/[controller]")]
public class GranjasController : ControllerBase
{
    private readonly IGranjaService _service;

    // TEMPORAL: mientras no hay JWT, usamos un userId fijo de prueba
    // Cuando implementes autenticación reemplaza esto por:
    // Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
    private static readonly Guid UsuarioTemporal =
        Guid.Parse("bb042bda-3c6c-4b7b-b9e6-9b8f9795d847");

    public GranjasController(IGranjaService service)
    {
        _service = service;
    }

    // GET api/granjas
    // Devuelve todas las granjas donde el usuario es miembro
    [HttpGet]
    public async Task<IActionResult> GetMisGranjas()
    {
        var granjas = await _service.GetByUsuarioAsync(UsuarioTemporal);
        return Ok(granjas);
    }

    // GET api/granjas/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var granja = await _service.GetByIdAsync(id);
        return granja is null ? NotFound() : Ok(granja);
    }

    // POST api/granjas
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearGranjaDto dto)
    {
        try
        {
            var creada = await _service.CrearAsync(dto, UsuarioTemporal);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = ex.Message });
        }
    }

    // PUT api/granjas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarGranjaDto dto)
    {
        try
        {
            var actualizada = await _service.ActualizarAsync(id, dto, UsuarioTemporal);
            return Ok(actualizada);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    // DELETE api/granjas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        try
        {
            await _service.EliminarAsync(id, UsuarioTemporal);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}