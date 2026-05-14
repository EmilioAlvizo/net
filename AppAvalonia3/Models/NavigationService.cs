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

        // Obtenemos el ViewModel solicitado desde el contenedor de dependencias
        var viewModel = _serviceProvider.GetRequiredService<T>();

        // Si navegamos a cualquier cosa que no sea el Login, activamos las barras
        if (typeof(T) != typeof(LoginViewModel))
        {
            _mainVM.IsLoggedIn = true;
        }

        _mainVM.CurrentView = viewModel;
        await Task.CompletedTask;
    }
}
