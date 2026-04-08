// controllers/ejemplarescontroller.cs
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
    public async Task<IActionResult> Registrar([FromBody] CrearEjemplarDto dto)
    {
        var ejemplar = new Ejemplares
        {
            GranjaId = dto.GranjaId,
            TipoAnimalId = dto.TipoAnimalId,
            GrupoId = dto.GrupoId,
            Brazalete = dto.Brazalete,
            PropositoId = dto.PropositoId,
            TipoAdquisicionId = dto.TipoAdquisicionId,
            FechaAdquisicion = dto.FechaAdquisicion,
            CostoAdquisicion = dto.CostoAdquisicion,
            Notas = dto.Notas,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001") // temporal
        };

        try
        {
            var resultado = await _service.RegistrarAsync(ejemplar);
            return CreatedAtAction(nameof(GetByGrupo), new { grupoId = resultado.GrupoId }, resultado.Id);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
    /* public async Task<IActionResult> Registrar([FromBody] Ejemplares ejemplar)
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
    } */

    // POST api/ejemplares/{id}/baja
    [HttpPost("{id}/baja")]
    public async Task<IActionResult> DarDeBaja(Guid id, [FromBody] DarDeBajaDto dto)
    {
        try
        {
            var baja = new BajasEjemplares
            {
                EjemplarId = id,
                RazonBajaId = dto.RazonBajaId,
                ImporteVenta = dto.ImporteVenta,
                FechaBaja = dto.FechaBaja,
                Notas = dto.Notas,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001")
            };

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
