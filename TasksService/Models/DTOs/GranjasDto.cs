namespace TasksService.Models.DTOs.GranjasDto;

public record CreateGranjaDto(
    string Nombre,
    string? Descripcion,
    string? Ubicacion
);

public record UpdateGranjaDto(
    string? Nombre,
    string? Descripcion,
    string? Ubicacion
);