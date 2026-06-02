// ViewModel/AnimalesFormViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia3.Models;
using AppAvalonia3.Services;
using Supabase;

namespace AppAvalonia3.ViewModels;

public partial class AnimalesFormViewModel : ObservableObject
{
    private readonly Client _supabase;
    private readonly INavigationService _navigationService;

    private string _granjaId = string.Empty;

    // Qué formulario está activo: "type", "group", "exemplar"
    [ObservableProperty] private string _activeForm = string.Empty;

    // Visibilidad de cada formulario
    public bool IsNewTypeOpen => ActiveForm == "type";
    public bool IsNewGroupOpen => ActiveForm == "group";
    public bool IsAddExemplarOpen => ActiveForm == "exemplar";

    public string FormTitle => ActiveForm switch
    {
        "type" => "Nuevo tipo de animal",
        "group" => "Nuevo grupo",
        "exemplar" => "Agregar ejemplares",
        _ => string.Empty
    };

    // ── Formulario: Nuevo tipo de animal ────────────────────────────────
    [ObservableProperty] private string _newTypeName = string.Empty;
    [ObservableProperty] private string _newTypeDesc = string.Empty;

    // ── Formulario: Nuevo grupo ──────────────────────────────────────────
    [ObservableProperty] private string _newGroupName = string.Empty;
    [ObservableProperty] private string _newGroupDesc = string.Empty;
    [ObservableProperty] private string _selectedTypeForGroup = string.Empty;
    public ObservableCollection<string> AvailableTypes { get; } = new();

    // ── Formulario: Agregar ejemplares ───────────────────────────────────
    [ObservableProperty] private string _selectedTypeForExemplar = "Gallina";
    [ObservableProperty] private string _selectedGroupForExemplar = string.Empty;
    [ObservableProperty] private int _exemplarQuantity;
    [ObservableProperty] private string _exemplarPurpose = "Huevos";
    [ObservableProperty] private string _exemplarAcquisition = "Compra";
    [ObservableProperty] private decimal _exemplarCost;
    [ObservableProperty] private DateTimeOffset _exemplarDate = DateTimeOffset.Now;
    public ObservableCollection<string> GroupsForSelectedType { get; } = new();
    public ObservableCollection<BrazaleteItem> AvailableBrazaletes { get; } = new();

    // ── Error / Loading ──────────────────────────────────────────────────
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _hasError;

    public AnimalesFormViewModel(Client supabase, INavigationService navigationService)
    {
        _supabase = supabase;
        _navigationService = navigationService;
    }

    /// <summary>
    /// AnimalesViewModel llama esto al navegar:
    /// await _navigationService.NavigateToAsync&lt;AnimalesFormViewModel&gt;("exemplar|granjaId")
    /// </summary>
    public void Initialize(string formType, string granjaId)
    {
        Console.WriteLine($">>> Initialize llamado con formType: '{formType}', granjaId: '{granjaId}'");
        _granjaId = granjaId;
        ActiveForm = formType;

        if (formType == "group")
        {
            AvailableTypes.Clear();
            // TODO: cargar tipos reales desde Supabase
            AvailableTypes.Add("Gallina");
            AvailableTypes.Add("Pollo");
        }

        if (formType == "exemplar")
        {
            LoadBrazaletes();
            UpdateExemplarGroups();
        }
    }

    // Notifica a la UI cuando cambia el formulario activo
    partial void OnActiveFormChanged(string value)
    {
        Console.WriteLine($">>> ActiveForm cambiado a: '{value}'");
        Console.WriteLine($">>> IsNewTypeOpen: {IsNewTypeOpen}");
        Console.WriteLine($">>> IsNewGroupOpen: {IsNewGroupOpen}");
        Console.WriteLine($">>> IsAddExemplarOpen: {IsAddExemplarOpen}");
        OnPropertyChanged(nameof(IsNewTypeOpen));
        OnPropertyChanged(nameof(IsNewGroupOpen));
        OnPropertyChanged(nameof(IsAddExemplarOpen));
        OnPropertyChanged(nameof(FormTitle));
    }

    partial void OnSelectedTypeForExemplarChanged(string value) => UpdateExemplarGroups();

    private void UpdateExemplarGroups()
    {
        GroupsForSelectedType.Clear();
        // TODO: filtrar grupos reales por tipo desde Supabase
        if (SelectedTypeForExemplar == "Gallina")
            GroupsForSelectedType.Add("Gallinero");
        else
            GroupsForSelectedType.Add("Corral Norte");

        SelectedGroupForExemplar = GroupsForSelectedType.Count > 0
            ? GroupsForSelectedType[0]
            : string.Empty;
    }

    private void LoadBrazaletes()
    {
        AvailableBrazaletes.Clear();
        int[] ids = { 12, 14, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 };
        foreach (var id in ids)
            AvailableBrazaletes.Add(new BrazaleteItem { Number = $"#{id}" });
    }

    // ── Guardar tipo ─────────────────────────────────────────────────────
    [RelayCommand]
    private async Task SaveTypeAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTypeName))
        {
            ShowError("El nombre es obligatorio.");
            return;
        }

        IsLoading = true;
        HasError = false;
        try
        {
            var nuevoTipo = new TipoAnimal
            {
                Nombre = NewTypeName,
                Descripcion = string.IsNullOrWhiteSpace(NewTypeDesc) ? null : NewTypeDesc.Trim(),
                GranjaId = _granjaId,
                CreatedBy = _supabase.Auth.CurrentUser.Id
            };
            
            await _supabase.From<TipoAnimal>().Insert(nuevoTipo);

            //await Task.Delay(300); // simular red
            await GoBackAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Error al guardar: {ex.Message}");
        }
        finally { IsLoading = false; }
    }

    // ── Guardar grupo ─────────────────────────────────────────────────────
    [RelayCommand]
    private async Task SaveGroupAsync()
    {
        if (string.IsNullOrWhiteSpace(NewGroupName))
        {
            ShowError("El nombre del grupo es obligatorio.");
            return;
        }

        IsLoading = true;
        HasError = false;
        try
        {
            // TODO: insertar en tabla grupos
            await Task.Delay(300);
            await GoBackAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Error al guardar: {ex.Message}");
        }
        finally { IsLoading = false; }
    }

    // ── Guardar ejemplares ───────────────────────────────────────────────
    [RelayCommand]
    private async Task SaveExemplarAsync()
    {
        if (ExemplarQuantity <= 0)
        {
            ShowError("La cantidad debe ser mayor a 0.");
            return;
        }

        IsLoading = true;
        HasError = false;
        try
        {
            // TODO: insertar en tabla ejemplares
            await Task.Delay(300);
            await GoBackAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Error al guardar: {ex.Message}");
        }
        finally { IsLoading = false; }
    }

    // ── Navegación ───────────────────────────────────────────────────────
    [RelayCommand]
    private async Task GoBackAsync()
    {
        await _navigationService.NavigateToAsync<AnimalesViewModel>(_granjaId);
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = true;
    }


}