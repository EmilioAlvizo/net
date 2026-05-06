// LayoutViewModel.cs
using ReactiveUI;
using System.Windows.Input;
using AppAvalonia2.Views;

namespace AppAvalonia2.ViewModels;

public class LayoutViewModel : ViewModelBase
{
    private object _currentView;
    public object CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public ICommand GoHome { get; }
    public ICommand GoAves { get; }
    public ICommand GoHuevos { get; }
    public ICommand GoComida { get; }
    public ICommand GoGraficas { get; }

    public LayoutViewModel()
    {
        GoHome = ReactiveCommand.Create(() => CurrentView = new HomeViewModel());
        GoAves = ReactiveCommand.Create(() => CurrentView = new object());
        GoHuevos = ReactiveCommand.Create(() => CurrentView = new HuevosViewModel());
        GoComida = ReactiveCommand.Create(() => CurrentView = new object());
        GoGraficas = ReactiveCommand.Create(() => CurrentView = new object());

        CurrentView = new HomeViewModel();
    }
}
