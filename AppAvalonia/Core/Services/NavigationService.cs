// /Core/Services/NavigationService.cs

using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Microsoft.Extensions.DependencyInjection;
using AppAvalonia.ViewModels;
using AppAvalonia.Views;

namespace AppAvalonia.Services;

/// <summary>
/// Implementación básica de navegación.
/// Se expande conforme se agreguen más pantallas.
/// </summary>
public class NavigationService : INavigationService
{
    public Task NavigateToAsync<TViewModel>(object? parameter = null)
    {
        // 1. Obtener el ViewModel desde el contenedor
        var viewModel = App.Services.GetRequiredService(typeof(TViewModel));

        // 2. Resolver la Vista (aquí asumimos una convención: GranjasViewModel -> GranjasPage o GranjasView)
        // O puedes hacerlo con un switch simple por ahora:
        Control view = viewModel switch
        {
            // Agrega aquí otras páginas conforme las crees
            GranjasViewModel vm => new GranjasPage { DataContext = vm },
            LoginViewModel vm => new LoginPage { DataContext = vm },
            AvesViewModel vm => new AvesPage { DataContext = vm },
            
            _ => throw new Exception($"No hay una vista definida para {typeof(TViewModel).Name}")
        };

        // 3. Cambiar el contenido según la plataforma
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContentControl!.Content = view;
            }
        }

        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        return Task.CompletedTask;
    }
}