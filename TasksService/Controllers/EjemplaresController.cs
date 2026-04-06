using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data.Entities;
using TasksService.Models;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EjemplaresController : ControllerBase
{
    private readonly IEjemplarService _service;

    public EjemplaresController(IEjemplarService service)
    {
        _service = service;
    }

    // GET api/ejemplares/grupo/{grupoId}
    [HttpGet("grupo/{grupoId}")]
    public async Task<IActionResult> GetByGrupo(Guid grupoId)
    {
        var ejemplares = await _service.GetActivosByGrupoAsync(grupoId);
        return Ok(ejemplares);
    }

    // GET api/ejemplares/brazaletes-disponibles?granjaId=...&tipoAnimalId=...
    [HttpGet("brazaletes-disponibles")]
    public async Task<IActionResult> GetBrazoletesDisponibles(
        [FromQuery] Guid granjaId,
        [FromQuery] Guid tipoAnimalId)
    {
        var disponibles = await _service.GetBrazoletesDisponiblesAsync(granjaId, tipoAnimalId);
        return Ok(disponibles);
    }

    // POST api/ejemplares
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Ejemplares ejemplar)
    {
        try
        {
            var resultado = await _service.RegistrarAsync(ejemplar);
            return CreatedAtAction(nameof(GetByGrupo),
                new { grupoId = resultado.GrupoId }, resultado);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // POST api/ejemplares/{id}/baja
    [HttpPost("{id}/baja")]
    public async Task<IActionResult> DarDeBaja(Guid id, [FromBody] BajasEjemplares baja)
    {
        try
        {
            await _service.DarDeBajaAsync(id, baja);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
