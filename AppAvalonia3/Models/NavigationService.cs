// NavigationService.cs
using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Microsoft.Extensions.DependencyInjection;
using AppAvalonia3.ViewModels;
using AppAvalonia3.Views;

namespace AppAvalonia3.Services;

public interface INavigationService
{
    Task NavigateToAsync<T>() where T : class;
    
    // NUEVO: Sobrecarga que permite enviar un parámetro a la vista destino
    Task NavigateToAsync<T>(object parameter) where T : class;
    
    void SetMainViewModel(object mainVM);
}

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private MainWindowViewModel? _mainVM;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SetMainViewModel(object mainVM)
    {
        _mainVM = mainVM as MainWindowViewModel;
    }

    public async Task NavigateToAsync<T>() where T : class
    {
        if (_mainVM == null) return;

        var viewModel = _serviceProvider.GetRequiredService<T>();

        if (typeof(T) != typeof(LoginViewModel))
        {
            _mainVM.IsLoggedIn = true;
        }

        _mainVM.CurrentView = viewModel;
        await Task.CompletedTask;
    }

    // NUEVO: Implementación de la navegación con parámetros
    public async Task NavigateToAsync<T>(object parameter) where T : class
    {
        if (_mainVM == null) return;

        // 1. Obtenemos el ViewModel destino
        var viewModel = _serviceProvider.GetRequiredService<T>();

        // 2. Si el ViewModel tiene la capacidad de inicializarse con datos, se los pasamos
        if (viewModel is CollaboratorsViewModel collaboratorsVM && parameter is Models.Farm farm)
        {
            // Ejecutamos su inicialización (que internamente llamará al ForceRefresh)
            // Usamos _ = Task.Run o simplemente await si queremos esperar a que cargue antes de mostrar la pantalla
            _ = collaboratorsVM.InitializeAsync(farm);
        }

        // Puedes añadir más "cases" o ifs en el futuro si otros ViewModels reciben otros parámetros

        if (typeof(T) != typeof(LoginViewModel))
        {
            _mainVM.IsLoggedIn = true;
        }

        // 3. Cambiamos la vista activa
        _mainVM.CurrentView = viewModel;
        await Task.CompletedTask;
    }
}
