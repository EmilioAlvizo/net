// ViewModels/CollaboratorsViewModel.cs
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia3.Models;
using AppAvalonia3.Services;
using System.Collections.Generic;

namespace AppAvalonia3.ViewModels;

public partial class CollaboratorsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    //private readonly Supabase.Client _supabase;

    [ObservableProperty] private Farm _currentFarm;
    [ObservableProperty] private bool _isLoading;
    public ObservableCollection<MiembroGranja> Members { get; } = new();

    public CollaboratorsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    // Método para recibir la granja al navegar
    public async Task InitializeAsync(Farm farm)
    {
        //CurrentFarm = farm;
        await LoadMembersAsync();
    }

    [RelayCommand]
    private async Task LoadMembersAsync()
    {
        
        
        
    }

    [RelayCommand]
    private async Task Back() => await _navigationService.NavigateToAsync<HomeViewModel>();
}