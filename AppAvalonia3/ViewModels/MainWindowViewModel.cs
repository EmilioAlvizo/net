using CommunityToolkit.Mvvm.ComponentModel; // O ReactiveUI
using CommunityToolkit.Mvvm.Input;

namespace AppAvalonia3.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    // Propiedad que se enlaza al ContentControl
    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private bool _isLoggedIn;

    /* public void ShowLogin()
    {
        IsLoggedIn = false;
        CurrentView = new LoginViewModel(this); // Pasamos 'this' para que el login nos avise al terminar
    }

    public MainWindowViewModel()
    {
        // Página inicial por defecto
        CurrentView = new HomeViewModel();
    }

    // Este método lo llamará el LoginViewModel cuando el usuario sea válido
    public void FinishLogin()
    {
        IsLoggedIn = true;
        ShowHome();
    } */

    // Comandos para cambiar de página
    [RelayCommand]
    public void ShowHome() => CurrentView = new HomeViewModel();

    [RelayCommand]
    public void ShowHuevos() => CurrentView = new HuevosViewModel();
}
