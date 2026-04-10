using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksService.Entities;
using TasksService.Models.DTOs.PerfilesDto;
using TasksService.Repositories.Interfaces;
using TasksService.Services;

namespace TasksService.Controllers;

/// <summary>
/// Gestión de perfiles de usuario.
/// Requiere autenticación JWT (Bearer token de Supabase).
/// </summary>
[ApiController]
[Route("api/[controller]")]
//[Authorize]
[Produces("application/json")]
[Tags("Perfiles")]
public class PerfilesController : ControllerBase
{
    private readonly PerfilService _service;

    public PerfilesController(PerfilService service)
    {
        _service = service;
    }

    private string GetToken() =>
        HttpContext.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

    /// <summary>Obtiene un perfil por ID</summary>
    /// <param name="id">UUID del perfil</param>
    /// <response code="200">Perfil encontrado</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Perfil no encontrado</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(Guid id)
    {
        var perfil = await _service.GetById(id, GetToken());
        return perfil is null ? NotFound(new { message = "Perfil no encontrado." }) : Ok(perfil);
    }

    /// <summary>Obtiene todos los perfiles</summary>
    /// <response code="200">Lista de perfiles</response>
    /// <response code="401">No autenticado</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PerfilDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var perfiles = await _service.GetAll(GetToken());
        return Ok(perfiles);
    }

    /// <summary>Crea un nuevo perfil</summary>
    /// <param name="dto">Datos del nuevo perfil</param>
    /// <response code="201">Perfil creado correctamente</response>
    /// <response code="400">Datos inválidos o error al crear</response>
    /// <response code="401">No autenticado</response>
    [HttpPost]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreatePerfilDto dto)
    {
        try
        {
            var created = await _service.Create(dto, GetToken());
            if (created is null) return BadRequest(new { message = "No se pudo crear el perfil." });

            var id = created.Value.GetProperty("id").GetString();
            return CreatedAtAction(nameof(Get), new { id = Guid.Parse(id!) }, created);
        }
        catch (Exception ex)
        {
            Console.WriteLine("🔥 ERROR Create Perfil: " + ex);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Actualiza parcialmente un perfil existente</summary>
    /// <param name="id">UUID del perfil a actualizar</param>
    /// <param name="dto">Campos a actualizar</param>
    /// <response code="200">Perfil actualizado correctamente</response>
    /// <response code="400">No se pudo actualizar</response>
    /// <response code="401">No autenticado</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePerfilDto dto)
    {
        try
        {
            var ok = await _service.Update(id, dto, GetToken());
            return ok
                ? Ok(new { message = "Perfil actualizado." })
                : BadRequest(new { message = "No se pudo actualizar el perfil." });
        }
        catch (Exception ex)
        {
            Console.WriteLine("🔥 ERROR Update Perfil: " + ex);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Elimina un perfil por ID</summary>
    /// <param name="id">UUID del perfil a eliminar</param>
    /// <response code="204">Perfil eliminado correctamente</response>
    /// <response code="404">Perfil no encontrado</response>
    /// <response code="401">No autenticado</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var ok = await _service.Delete(id, GetToken());
            return ok ? NoContent() : NotFound(new { message = "Perfil no encontrado." });
        }
        catch (Exception ex)
        {
            Console.WriteLine("🔥 ERROR Delete Perfil: " + ex);
            return BadRequest(new { message = ex.Message });
        }
    }
}