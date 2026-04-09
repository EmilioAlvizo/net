using Microsoft.AspNetCore.Mvc;
using TasksService.Services;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SupabaseAuthService _auth;

    public AuthController(SupabaseAuthService auth) => _auth = auth;

    /// <summary>Registra un nuevo usuario en Supabase Auth</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest req)
    {
        var ok = await _auth.RegisterAsync(req.Email, req.Password);
        return ok ? Ok(new { message = "Usuario registrado. Revisa tu email." })
                  : BadRequest(new { message = "No se pudo registrar el usuario." });
    }

    /// <summary>Inicia sesión y retorna el JWT de Supabase</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest req)
    {
        var token = await _auth.LoginAsync(req.Email, req.Password);
        return token is not null ? Ok(new { token })
                                 : Unauthorized(new { message = "Credenciales inválidas." });
    }
}

public record AuthRequest(string Email, string Password);