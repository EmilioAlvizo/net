// ViewModel/AnimalesViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia3.Models;
using Supabase;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AppAvalonia3.ViewModels;

// Debe ser 'partial' para que el generador de código funcione
public partial class AnimalesViewModel : ObservableObject
{
    private readonly Client _supabase;
    private string? _granjaId; // Se puede pasar en la inicialización

    // Listas maestras desde DB
    private List<GrupoAnimal> _allGroups = new();
    private List<LoteAnimal> _allLots = new();

    // UI Properties
    [ObservableProperty] private int _avesVivasCount;
    [ObservableProperty] private int _gruposCount;
    [ObservableProperty] private int _muertesCount;
    [ObservableProperty] private string _selectedFilter = "Todos"; // "Todos", "gallina", "pollo"
    [ObservableProperty] private bool _isFabMenuOpen;

    // Control de Formularios (Modales)
    [ObservableProperty] private bool _isNewTypeOpen;
    [ObservableProperty] private bool _isNewGroupOpen;
    [ObservableProperty] private bool _isAddExemplarOpen;

    // Colecciones enlazadas a la UI
    public ObservableCollection<string> Filtros { get; } = new() { "Todos", "Gallina", "Pollo" };
    public ObservableCollection<GrupoDisplayItem> GroupCards { get; } = new();

    // Campos de captura para Formularios
    [ObservableProperty] private string _newTypeName = string.Empty;
    [ObservableProperty] private string _newTypeDesc = string.Empty;

    [ObservableProperty] private string _newGroupName = string.Empty;
    [ObservableProperty] private string _newGroupDesc = string.Empty;
    [ObservableProperty] private string _selectedTypeForGroup = string.Empty;
    public ObservableCollection<string> AvailableTypes { get; } = new();

    // Campos para Agregar Ejemplar
    [ObservableProperty] private string _selectedTypeForExemplar = "Gallina";
    [ObservableProperty] private string _selectedGroupForExemplar = string.Empty;
    [ObservableProperty] private int _exemplarQuantity;
    [ObservableProperty] private string _exemplarPurpose = "Huevos"; // Huevos, Carne, Reproduccion, Otro
    [ObservableProperty] private string _exemplarAcquisition = "Compra"; // Compra, Cria, Regalo, Otro
    [ObservableProperty] private decimal _exemplarCost;
    [ObservableProperty] private DateTimeOffset _exemplarDate = DateTimeOffset.Now;
    public ObservableCollection<string> GroupsForSelectedType { get; } = new();
    public ObservableCollection<BrazaleteItem> AvailableBrazaletes { get; } = new();

    public AnimalesViewModel(Client supabase)
    {
        _supabase = supabase;
    }

    public async Task InitializeAsync(string granjaId)
    {
        _granjaId = granjaId;

        // Ahora sí, cargamos los datos usando el ID real
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        // En producción cargarías esto desde Supabase filtrando por _granjaId
        // Ejemplo de mock inicial para reproducir fielmente tus capturas de pantalla:
        _allGroups = new List<GrupoAnimal>
        {
            new() { Id = "g1", Nombre = "Gallinero", TipoAnimalId = "gallina" },
            new() { Id = "g2", Nombre = "Pollos engorda", TipoAnimalId = "pollo" }
        };

        _allLots = new List<LoteAnimal>
        {
            new() { Id = "l1", GrupoId = "g1", Cantidad = 8, Proposito = "Huevos", Adquisicion = "Compra", Muertes = 4, Fecha = DateTime.Parse("2025-04-03") },
            new() { Id = "l2", GrupoId = "g1", Cantidad = 4, Proposito = "Huevos", Adquisicion = "Compra", Muertes = 2, Fecha = DateTime.Parse("2025-01-12") },
            new() { Id = "l3", GrupoId = "g1", Cantidad = -4, Proposito = "Huevos", Adquisicion = "Otro", Muertes = 14, Fecha = DateTime.Parse("2024-01-21") }
        };

        ApplyFilterAndCalculateCounters();
        LoadBrazaletes();
    }

    private void ApplyFilterAndCalculateCounters()
    {
        var filteredGroups = SelectedFilter == "Todos"
            ? _allGroups
            : _allGroups.Where(g => g.TipoAnimalId.Equals(SelectedFilter, StringComparison.OrdinalIgnoreCase)).ToList();

        GroupCards.Clear();
        int totalVivas = 0;
        int totalMuertas = 0;

        foreach (var group in filteredGroups)
        {
            var lots = _allLots.Where(l => l.GrupoId == group.Id).ToList();
            int vivasGrupo = lots.Sum(l => l.Cantidad);
            int muertasGrupo = lots.Sum(l => l.Muertes);

            totalVivas += vivasGrupo;
            totalMuertas += muertasGrupo;

            GroupCards.Add(new GrupoDisplayItem
            {
                Nombre = group.Nombre,
                Tipo = group.TipoAnimalId,
                TotalAves = vivasGrupo,
                Lots = new ObservableCollection<LoteAnimal>(lots)
            });
        }

        AvesVivasCount = totalVivas;
        GruposCount = filteredGroups.Count;
        MuertesCount = totalMuertas;
    }

    partial void OnSelectedFilterChanged(string value) => ApplyFilterAndCalculateCounters();

    // Comandos de Interacción del FAB (+)
    [RelayCommand] private void ToggleFabMenu() => IsFabMenuOpen = !IsFabMenuOpen;

    [RelayCommand]
    private void OpenForm(string formType)
    {
        IsFabMenuOpen = false;
        if (formType == "type") IsNewTypeOpen = true;
        if (formType == "group") { IsNewGroupOpen = true; AvailableTypes.Clear(); AvailableTypes.Add("Gallina"); AvailableTypes.Add("Pollo"); }
        if (formType == "exemplar") { IsAddExemplarOpen = true; UpdateExemplarGroups(); }
    }

    [RelayCommand]
    private void CloseAllModals()
    {
        IsNewTypeOpen = false; IsNewGroupOpen = false; IsAddExemplarOpen = false;
    }

    private void LoadBrazaletes()
    {
        AvailableBrazaletes.Clear();
        int[] ids = { 12, 14, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 };
        foreach (var id in ids) AvailableBrazaletes.Add(new BrazaleteItem { Number = $"#{id}" });
    }

    private void UpdateExemplarGroups()
    {
        GroupsForSelectedType.Clear();
        if (SelectedTypeForExemplar == "Gallina") GroupsForSelectedType.Add("Gallinero");
        else GroupsForSelectedType.Add("Corral Norte");
        SelectedGroupForExemplar = GroupsForSelectedType.FirstOrDefault() ?? "";
    }

    partial void OnSelectedTypeForExemplarChanged(string value) => UpdateExemplarGroups();

    [RelayCommand]
    private async Task SaveType() { CloseAllModals(); await Task.CompletedTask; }

    [RelayCommand]
    private async Task SaveGroup() { CloseAllModals(); await Task.CompletedTask; }

    [RelayCommand]
    private async Task SaveExemplar() { CloseAllModals(); await Task.CompletedTask; }
}

// Clases auxiliares para pintado en UI
public class GrupoDisplayItem
{
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // "gallina" o "pollo"
    public int TotalAves { get; set; }
    public bool IsExpanded { get; set; } = true;
    public ObservableCollection<LoteAnimal> Lots { get; set; } = new();
}

public partial class BrazaleteItem : ObservableObject
{
    public string Number { get; set; } = string.Empty;
    [ObservableProperty] private bool _isSelected;
    [RelayCommand] private void Toggle() => IsSelected = !IsSelected;
}
