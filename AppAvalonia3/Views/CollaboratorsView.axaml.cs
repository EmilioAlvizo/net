// Views/CollaboratorsView.axaml.cs
using Avalonia.Controls;
using Avalonia.Input;
using AppAvalonia3.ViewModels;
using AppAvalonia3.Models;

namespace AppAvalonia3.Views;

public partial class CollaboratorsView : UserControl
{
    public CollaboratorsView()
    {
        InitializeComponent();
    }

    private void OnMemberCardPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is MiembroGranja miembro)
        {
            if (DataContext is CollaboratorsViewModel vm)
            {
                vm.OpenEditRole(miembro);
            }
        }
    }
}