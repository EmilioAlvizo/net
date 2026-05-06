using CommunityToolkit.Mvvm.ComponentModel;

namespace AppAvalonia3.ViewModels;

// Debe ser 'partial' para que el generador de código funcione
public partial class SettingsViewModel : ObservableObject 
{
    [ObservableProperty]
    private string _welcomeMessage = "Hola desde el SettingsViewModel";

    public SettingsViewModel()
    {
        // Aquí puedes inicializar datos o servicios
    }
}
