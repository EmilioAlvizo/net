// Models/Farm.cs
using System.Text.Json.Serialization;

namespace AppAvalonia3.Models;

public record Farm(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("nombre")] string Nombre,
    [property: JsonPropertyName("ubicacion")] string? Ubicacion,
    [property: JsonPropertyName("descripcion")] string? Descripcion,
    [property: JsonPropertyName("owner_id")] string OwnerId
);