using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.Data;
using TasksService.Data.Entities;
using TasksService.Services;
using TasksService.Models.DTOs;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupabaseAuthController : ControllerBase
{
    private readonly GranjaDbContext _db;
    private readonly SupabaseAuthService _supabaseAuth;

    public SupabaseAuthController(GranjaDbContext db, SupabaseAuthService supabaseAuth)
    {
        _db = db;
        _supabaseAuth = supabaseAuth;
    }

    // POST api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrarUsuarioDto dto)
    {
        // 1. Verificar si el email ya existe en perfiles (opcional pero útil)
        // Nota: no podemos consultar auth.users directamente desde EF,
        // pero si el registro en Supabase falla, su error nos lo indica

        Guid userId;
        try
        {
            // 2. Crear usuario en Supabase Auth
            userId = await _supabaseAuth.RegisterUserAsync(dto.Email, dto.Password, dto.Nombre);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }

        // 3. Crear perfil en nuestra tabla perfiles con el mismo UUID
        var perfil = new Perfiles
        {
            Id = userId,          // mismo UUID que auth.users
            Nombre = dto.Nombre,
            AvatarUrl = dto.AvatarUrl,
            CreatedAt = DateTime.UtcNow
        };

        /* _db.Perfiles.Add(perfil);
        await _db.SaveChangesAsync(); */

        // 4. Devolver el perfil creado
        return CreatedAtAction(nameof(Register), new
        {
            id = perfil.Id,
            nombre = perfil.Nombre,
            avatarUrl = perfil.AvatarUrl,
            createdAt = perfil.CreatedAt
        });
    }
}