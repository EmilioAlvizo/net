// ViewModels/HomeViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

    [ObservableProperty] private ObservableCollection<Farm> _farms = new();

    [ObservableProperty] private bool _isLoading;

    // Almacena la granja que se encuentra activa/seleccionada actualmente
    [ObservableProperty] private Farm? _selectedFarm;
    
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
    private async Task SelectFarmAsync(Farm farm)
    {
        if (farm == null) return;
        
        SelectedFarm = farm;
        
        // Navegamos a la pantalla de Animales pasándole la granja elegida como parámetro
        await _navigationService.NavigateToAsync<AnimalesViewModel>(farm);
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
        await _navigationService.NavigateToAsync<CollaboratorsViewModel>(selectedFarm);
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
            // AUTO-SELECCIÓN: Si hay granjas, buscamos la que es de su propiedad o la primera por defecto
            if (Farms.Count > 0)
            {
                // Buscamos si es dueño (puedes ajustar esta validación según tus propiedades de Farm)
                var ownerFarm = Farms.FirstOrDefault(f => f.OwnerPerfil != null) ?? Farms.First();
                SelectedFarm = ownerFarm;
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

            if (Farms.Count > 0 && SelectedFarm == null)
            {
                SelectedFarm = Farms.First();
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // En ViewModels/HomeViewModel.cs

    [RelayCommand]
    private async Task DeleteFarmAsync(Farm farm)
    {
        if (farm == null) return;

        IsLoading = true;
        try
        {
            // 1. Llamamos a Supabase para borrarla de la base de datos
            bool eliminadoExitoso = await _farmService.DeleteFarmAsync(farm.Id);

            if (eliminadoExitoso)
            {
                // 2. Si se borró en el backend, la removemos de la UI inmediatamente
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Farms.Remove(farm);
                });
                if (SelectedFarm == farm)
                {
                    SelectedFarm = Farms.FirstOrDefault();
                }

                System.Diagnostics.Debug.WriteLine($"Granja '{farm.Nombre}' eliminada con éxito.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No se pudo eliminar la granja de la base de datos.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en el proceso de borrado: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

}