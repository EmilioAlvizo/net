// GranjasViewModel.cs

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia.Models;
using AppAvalonia.Services;

namespace AppAvalonia.ViewModels;

public partial class GranjasViewModel : ObservableObject
{
    private readonly IGranjasService _granjasService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _errorMessage;

    public ObservableCollection<GranjaCardModel> Granjas { get; } = new();

    public GranjasViewModel(IGranjasService granjasService, INavigationService navigationService)
    {
        _granjasService = granjasService;
        _navigationService = navigationService;
    }

    public async Task InitializeAsync()
    {
        await LoadGranjasAsync();
    }

    [RelayCommand]
    private async Task LoadGranjasAsync()
    {
        IsLoading = true;
        Granjas.Clear();

        try
        {
            var granjas = await _granjasService.GetMisGranjasAsync();
            foreach (var g in granjas)
                Granjas.Add(g);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"No se pudieron cargar las granjas: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void OpenGranja(GranjaCardModel granja)
    {
        // TODO: navegar al detalle cuando esté implementado
    }

    [RelayCommand]
    private void NuevaGranja()
    {
        // TODO: navegar a nueva granja cuando esté implementado
    }

    // Bottom nav
    [RelayCommand] private void OpenSettings() { }
    [RelayCommand] private void NavHome() { }
    [RelayCommand] private void NavAves() { _navigationService.NavigateToAsync<AvesViewModel>(); }
    [RelayCommand] private void NavHuevos() { }
    [RelayCommand] private void NavAlimento() { }
    [RelayCommand] private void NavEstadisticas() { }
}