// ViewModels/HomeViewModel.cs
using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using AppAvalonia3.Models;
using AppAvalonia3.Services;

namespace AppAvalonia3.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IFarmService _farmService;

    [ObservableProperty]
    private ObservableCollection<Farm> _farms = new();

    [ObservableProperty]
    private bool _isLoading;

    public bool IsDesktop => !OperatingSystem.IsAndroid() && !OperatingSystem.IsIOS();

    public HomeViewModel(INavigationService navigationService, IFarmService farmService)
    {
        _navigationService = navigationService;
        _farmService = farmService;

        // Datos de prueba para que veas algo en la pantalla
        //Farms.Add(new Farm(null, "Granja de Prueba", "Ubicación", "Notas", "ID"));

        // Disparamos la carga inicial de datos de manera segura y asíncrona
        _ = LoadFarmsAsync();
    }

    [RelayCommand]
    private async Task GoToAddFarm()
    {
        await _navigationService.NavigateToAsync<AddFarmViewModel>();
    }

    [RelayCommand]
    private async Task GoToCollaboratorsAsync(Farm selectedFarm)
    {
        if (selectedFarm == null) return;

        // Aquí navegas a la nueva pantalla pasándole el ID de la granja
        // Necesitarás crear CollaboratorsViewModel posteriormente
        await _navigationService.NavigateToAsync<CollaboratorsViewModel>();
    }

    [RelayCommand]
    public async Task LoadFarmsAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Por defecto usará la caché en memoria
            var realFarms = await _farmService.GetUserFarmsAsync(forceRefresh: false);

            Farms.Clear();
            foreach (var farm in realFarms)
            {
                Farms.Add(farm);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Este comando lo puedes ligar a un botón con el icono de "flecha de actualizar" en tu XAML
    [RelayCommand]
    public async Task ForceRefreshFarmsAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Aquí SÍ obligamos al servicio a saltarse la caché e ir a Supabase
            var realFarms = await _farmService.GetUserFarmsAsync(forceRefresh: true);

            Farms.Clear();
            foreach (var farm in realFarms)
            {
                Farms.Add(farm);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

}