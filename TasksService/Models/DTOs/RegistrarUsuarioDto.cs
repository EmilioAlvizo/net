// Models/DTOs/RegistrarUsuarioDto.cs
namespace TasksService.Models.DTOs;
public class RegistrarUsuarioDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? AvatarUrl { get; set; }
}