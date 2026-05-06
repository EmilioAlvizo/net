// MainWindow.axaml.cs
using Avalonia.Controls;
using AppAvalonia2.ViewModels;

namespace AppAvalonia2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new LayoutViewModel();
    }
}