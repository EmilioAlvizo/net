// ViewModel/MainWindowViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia3.Services; // Asegúrate de incluir el namespace de tus servicios
using System.Threading.Tasks;

namespace AppAvalonia3.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private bool _isLoggedIn;

    // 1. Inyectamos el servicio de navegación en el constructor
    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        // 2. IMPORTANTE: El servicio de navegación necesita referencia a este ViewModel
        // para poder cambiar la propiedad 'CurrentView'.
        _navigationService.SetMainViewModel(this);
    }

    // 3. Usamos el servicio en lugar de crear la instancia con 'new'
    [RelayCommand]
    public async Task ShowHome()
    {
        await _navigationService.NavigateToAsync<HomeViewModel>();
    }

    [RelayCommand]
    public async Task ShowHuevos()
    {
        await _navigationService.NavigateToAsync<HuevosViewModel>();
    }

    // Si necesitas una función para terminar el login (como tenías en comentarios)
    public async Task FinishLogin()
    {
        IsLoggedIn = true;
        await ShowHome();
    }
}