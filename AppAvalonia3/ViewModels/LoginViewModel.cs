using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppAvalonia3.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly MainWindowViewModel _mainVM;

    public LoginViewModel(MainWindowViewModel mainVM)
    {
        _mainVM = mainVM;
    }

    [RelayCommand]
    public async Task Login()
    {
        // Lógica de autenticación aquí...
        await Task.Delay(1000); // Simulando red
        
        // Si es exitoso:
        _mainVM.FinishLogin();
    }
}
