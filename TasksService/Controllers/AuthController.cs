using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TasksService.Models;

namespace TasksService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    private readonly IConfiguration _config = config;

    // POST api/auth/login
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        // 1. Verificar credenciales
        // Por ahora usamos usuario hardcodeado — después vendría de la BD
        if (request.Username != "admin" || request.Password != "1234")
            return Unauthorized("Usuario o contraseña incorrectos");

        // 2. Generar el token
        var token = GenerarToken(request.Username);

        return Ok(new { token });
    }

    private string GenerarToken(string username)
    {
        // Leer configuración
        var secretKey = _config["Jwt:SecretKey"]!;
        var issuer    = _config["Jwt:Issuer"]!;
        var audience  = _config["Jwt:Audience"]!;
        var horas     = int.Parse(_config["Jwt:ExpirationHours"]!);

        // Crear la firma con la clave secreta
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims = datos que van DENTRO del token
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("app", "TaskFlowAPI")
        };

        // Construir el token
        var token = new JwtSecurityToken(
            issuer:             issuer,
            audience:           audience,
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(horas),
            signingCredentials: creds
        );

        // Convertirlo a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}