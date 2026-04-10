namespace TasksService.Models.DTOs.PerfilesDto;

public class PerfilDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

public class CreatePerfilDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

public class UpdatePerfilDto
{
    public string? Nombre { get; set; }
    public string? AvatarUrl { get; set; }
}