// Features/Auth/LoginViewModel.cs

using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia.Services;

namespace AppAvalonia.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public LoginViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        Console.WriteLine(">>> LoginAsync ejecutado");  // ← agrega esta línea
        Console.WriteLine($">>> Email: {Email}");

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ShowError("Por favor ingresa tu correo y contraseña.");
            return;
        }

        IsLoading = true;
        HasError = false;

        try
        {
            var result = await _authService.LoginAsync(Email.Trim(), Password);

            if (result.Success)
            {
                await _navigationService.NavigateToAsync<GranjasViewModel>();
            }
            else
            {
                // Muestra el error exacto que regresa Supabase
                ShowError(result.ErrorMessage ?? "Credenciales incorrectas.");
            }
        }
        catch (Exception ex)
        {
            // Muestra el error completo para debug
            ShowError($"Error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void ForgotPassword()
    {
        // TODO: navegar a recuperar contraseña
    }

    [RelayCommand]
    private void Register()
    {
        // TODO: navegar a registro
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = true;
    }

    public bool IsNotLoading => !IsLoading;

    partial void OnIsLoadingChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotLoading));
    }
}