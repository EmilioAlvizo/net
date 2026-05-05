using CommunityToolkit.Mvvm.ComponentModel;

namespace AppAvalonia.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private object? _currentView;
}