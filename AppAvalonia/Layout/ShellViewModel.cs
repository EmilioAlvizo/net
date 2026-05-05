//  Layout/ShellViewModel.cs
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia.Services;
using AppAvalonia.Views;
using AppAvalonia;
using AppAvalonia.ViewModels;

namespace AppAvalonia.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private object? _currentView;

    // Agrega esta propiedad 👇
    [ObservableProperty]
    private string _title = "Granjas";

    public ShellViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }

    // navegación global 👇
    [RelayCommand]
    private async Task NavHome() =>
        await _navigation.NavigateToAsync<GranjasViewModel>();

    [RelayCommand]
    private async Task NavAves() =>
        await _navigation.NavigateToAsync<AvesViewModel>();

    /* [RelayCommand]
    private async Task NavHuevos() =>
        await _navigation.NavigateToAsync<HuevosViewModel>();

    [RelayCommand]
    private async Task NavAlimento() =>
        await _navigation.NavigateToAsync<AlimentoViewModel>();

    [RelayCommand]
    private async Task NavEstadisticas() =>
        await _navigation.NavigateToAsync<EstadisticasViewModel>(); */
}