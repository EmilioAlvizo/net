using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using AppAvalonia.ViewModels;

namespace AppAvalonia.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGallinasServices(
        this IServiceCollection services,
        SupabaseConfig config)
    {
        // Config
        services.AddSingleton(config);

        // HttpClient compartido
        services.AddSingleton<HttpClient>(_ => new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        });

        // Auth (singleton para mantener el token en memoria)
        services.AddSingleton<IAuthService>(sp =>
            new SupabaseAuthService(
                sp.GetRequiredService<HttpClient>(),
                config
            ));

        // Cliente REST base
        services.AddSingleton<SupabaseRestClient>(sp =>
            new SupabaseRestClient(
                sp.GetRequiredService<HttpClient>(),
                config,
                sp.GetRequiredService<IAuthService>()
            ));

        // Servicios de datos
        services.AddTransient<IGranjasService>(sp =>
            new SupabaseGranjasService(
                sp.GetRequiredService<SupabaseRestClient>(),
                sp.GetRequiredService<IAuthService>()
            ));

        // Navigation
        services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<GranjasViewModel>();

        return services;
    }
}