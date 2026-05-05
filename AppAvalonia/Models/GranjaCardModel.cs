using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AppAvalonia.Models;

/// <summary>
/// Model de presentación para las tarjetas de granja en la lista.
/// Los datos vienen del TasksService API.
/// </summary>
public partial class GranjaCardModel : ObservableObject
{
    public required string Id { get; init; }
    public required string Nombre { get; init; }
    public string? OwnerEmail { get; init; }
    public string? Descripcion { get; init; }
    public string? Ubicacion { get; init; }
    public bool HasCoverImage { get; init; } = false;

    // Stats (calculados en el API o consultados por separado)
    public int TotalAves { get; init; }
    public int TotalHuevos { get; init; }
    public decimal Balance { get; init; }

    public string BalanceDisplay =>
        Balance >= 0 ? $"+{Balance:N0} $" : $"{Balance:N0} $";

    public IBrush BalanceBrush =>
        Balance >= 0
            ? new SolidColorBrush(Color.Parse("#4ADE80"))
            : new SolidColorBrush(Color.Parse("#F87171"));
}

/// <summary>
/// Respuesta del API de granjas (mapeo de PostgREST / TasksService)
/// </summary>
public class GranjaDto
{
    public string Id { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Ubicacion { get; set; }
    public DateTime CreatedAt { get; set; }
}