// ViewModels/AddFarmViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia3.Services;
using System.Threading.Tasks;

namespace AppAvalonia3.ViewModels;

public partial class AddFarmViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IFarmService _farmService;

    [ObservableProperty] private string _farmName = string.Empty;
    [ObservableProperty] private string _location = string.Empty;
    [ObservableProperty] private string _notes = string.Empty;
    [ObservableProperty] private bool _isSaving;

    public AddFarmViewModel(INavigationService navigationService, IFarmService farmService)
    {
        _navigationService = navigationService;
        _farmService = farmService;
    }

    [RelayCommand]
    private async Task Back() => await _navigationService.NavigateToAsync<HomeViewModel>();

    [RelayCommand]
    private async Task CreateFarm()
    {
        if (string.IsNullOrWhiteSpace(FarmName)) return;

        IsSaving = true;
        var success = await _farmService.CreateFarmAsync(FarmName, Location, Notes);
        IsSaving = false;

        if (success)
        {
            await _navigationService.NavigateToAsync<HomeViewModel>();

        }
    }
}