using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Linq;
using Avalonia.Markup.Xaml;
using AppAvalonia3.ViewModels;
using AppAvalonia3.Views;
using AppAvalonia3.Services;

namespace AppAvalonia3;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        // 1. Registrar Configuración y HttpClient
        services.AddSingleton(new SupabaseConfig("https://xagnnkqqtdtvadaseubc.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhhZ25ua3FxdGR0dmFkYXNldWJjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQ4MjYzMjEsImV4cCI6MjA5MDQwMjMyMX0.m8V3r4GTutaqvj0agfZig__Itxmtw5m2BKtPjoXpeXA"));
        services.AddHttpClient<IAuthService, SupabaseAuthService>();

        // 2. Registrar Navegación
        services.AddSingleton<INavigationService, NavigationService>();

        // 3. Registrar ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>(); // Transient = se crea uno nuevo cada vez
        services.AddTransient<HomeViewModel>();
        services.AddTransient<HuevosViewModel>();

        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainVM = serviceProvider.GetRequiredService<MainWindowViewModel>();
            var navService = serviceProvider.GetRequiredService<INavigationService>();

            // Conectar el servicio con el MainVM
            navService.SetMainViewModel(mainVM);

            // Iniciar en el Login
            navService.NavigateToAsync<LoginViewModel>();

            desktop.MainWindow = new MainWindow { DataContext = mainVM };
        }

        base.OnFrameworkInitializationCompleted();
    }

}