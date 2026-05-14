// Home.axaml.cs
using Avalonia.Controls;
using AppAvalonia3.ViewModels;

namespace AppAvalonia3.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    // Este método se ejecutará cuando el usuario arrastre la pantalla hacia abajo en el celular
    private async void OnRefreshRequested(object? sender, RefreshRequestedEventArgs e)
    {
        // Obtenemos el "deferral" para controlar la animación visual de carga
        var deferral = e.GetDeferral();
        
        if (DataContext is HomeViewModel viewModel)
        {
            // Llamamos a tu comando de refresco real en Supabase
            await viewModel.ForceRefreshFarmsAsync();
        }
        
        // Le avisamos a Avalonia que la descarga terminó para que oculte el spinner
        deferral.Complete();
    }
}