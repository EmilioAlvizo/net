namespace TasksService.Models.DTOs.PerfilesDto;

public record CreatePerfilDto(string Nombre, string? AvatarUrl);
public record UpdatePerfilDto(string? Nombre, string? AvatarUrl);