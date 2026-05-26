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
    private readonly IMiembrosService _miembrosService;
    //private readonly Supabase.Client _supabase;

    [ObservableProperty] private MiembroGranja _currentMember;
    [ObservableProperty] private bool _isLoading;
    public ObservableCollection<MiembroGranja> Members { get; } = new();

    public CollaboratorsViewModel(INavigationService navigationService, IMiembrosService miembrosService)
    {
        _navigationService = navigationService;
        _miembrosService = miembrosService;

        _ = LoadMembersAsync();
    }

    // Método para recibir la granja al navegar
    /* public async Task InitializeAsync(Farm farm)
    {
        //CurrentFarm = farm;
        await LoadMembersAsync();
    } */


    [RelayCommand]
    public async Task LoadMembersAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Por defecto usará la caché en memoria
            var realMembers = await _miembrosService.GetUserMiembrosAsync(forceRefresh: false);

            Members.Clear();
            foreach (var member in realMembers)
            {
                Members.Add(member);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Este comando lo puedes ligar a un botón con el icono de "flecha de actualizar" en tu XAML
    [RelayCommand]
    public async Task ForceRefreshMembersAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Aquí SÍ obligamos al servicio a saltarse la caché e ir a Supabase
            var realMembers = await _miembrosService.GetUserMiembrosAsync(forceRefresh: true);

            Members.Clear();
            foreach (var member in realMembers)
            {
                Members.Add(member);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Back() => await _navigationService.NavigateToAsync<HomeViewModel>();
}