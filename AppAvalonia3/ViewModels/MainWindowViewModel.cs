using CommunityToolkit.Mvvm.ComponentModel; // O ReactiveUI
using CommunityToolkit.Mvvm.Input;

namespace AppAvalonia3.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    // Propiedad que se enlaza al ContentControl
    [ObservableProperty]
    private object? _currentView;

    public MainWindowViewModel()
    {
        // Página inicial por defecto
        CurrentView = new HomeViewModel();
    }

    // Comandos para cambiar de página
    [RelayCommand]
    public void ShowHome() => CurrentView = new HomeViewModel();

    [RelayCommand]
    public void ShowSettings() => CurrentView = new SettingsViewModel();
}
