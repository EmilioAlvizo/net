// App.axaml.cs

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using Microsoft.Extensions.DependencyInjection;
using AppAvalonia.Services;
using AppAvalonia.ViewModels;
using AppAvalonia.Views;

namespace AppAvalonia;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // ── Configuración de Supabase ──────────────────────────────────────
        // ⚠️ No hardcodees estas claves en producción.
        // Opciones seguras:
        //   - Android/iOS: usar SecureStorage o Resources
        //   - Desktop:     leer de variable de entorno o archivo .env
        var supabaseConfig = new SupabaseConfig(
            Url: "https://xagnnkqqtdtvadaseubc.supabase.co",   // <-- cambia esto
            AnonKey: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhhZ25ua3FxdGR0dmFkYXNldWJjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQ4MjYzMjEsImV4cCI6MjA5MDQwMjMyMX0.m8V3r4GTutaqvj0agfZig__Itxmtw5m2BKtPjoXpeXA"                       // <-- cambia esto
        );

        // ── DI Container ──────────────────────────────────────────────────
        var services = new ServiceCollection();

        // Servicios
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddGallinasServices(supabaseConfig);

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<GranjasViewModel>();
        services.AddTransient<AvesViewModel>();

        Services = services.BuildServiceProvider();

        // 1. Obtenemos el ShellViewModel (el contenedor principal)
        var shellVM = Services.GetRequiredService<ShellViewModel>();

        // ── Navegación inicial ────────────────────────────────────────────
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Asignamos el LoginPage como el CONTENIDO de la ventana, no solo el DataContext
            var loginPage = new LoginPage
            {
                DataContext = Services.GetRequiredService<LoginViewModel>()
            };

            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;

            // navegación inicial usando el servicio
            var navigation = Services.GetRequiredService<INavigationService>();
            navigation.NavigateToAsync<LoginViewModel>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            // Android / iOS
            singleView.MainView = new LoginPage
            {
                DataContext = Services.GetRequiredService<LoginViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}