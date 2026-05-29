// ViewModels/CollaboratorsViewModel.cs
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppAvalonia3.Models;
using AppAvalonia3.Services;
using System.Collections.Generic;
using System;
using Supabase;

namespace AppAvalonia3.ViewModels;

public partial class CollaboratorsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IMiembrosService _miembrosService;
    private readonly Client _supabase;

    [ObservableProperty] private MiembroGranja _currentMember;
    [ObservableProperty] private bool _isLoading;
    // Propiedad para almacenar la granja actual de la vista
    [ObservableProperty] private Farm _currentFarm;
    // Propiedades para controlar el panel flotante de edición
    [ObservableProperty] private bool _isEditRoleOpen;
    [ObservableProperty] private MiembroGranja? _selectedMemberForEdit;
    [ObservableProperty] private bool _isAddMemberOpen;
    [ObservableProperty] private string _inviteEmail = string.Empty;
    public ObservableCollection<MiembroGranja> Members { get; } = new();

    public CollaboratorsViewModel(INavigationService navigationService, IMiembrosService miembrosService, Client supabase)
    {
        _navigationService = navigationService;
        _miembrosService = miembrosService;
        _supabase = supabase;

        //_ = LoadMembersAsync(Farm farm);
    }

    // El NavigationService llamará a este método de forma automática al navegar
    public async Task InitializeAsync(Farm farm)
    {
        if (farm == null) return;

        CurrentFarm = farm;

        // Ejecuta inmediatamente la recarga forzada pasando la granja de la card elegida
        await ForceRefreshMembersAsync(CurrentFarm);
    }

    [RelayCommand]
    public async Task LoadMembersAsync(Farm farm)
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Por defecto usará la caché en memoria
            var realMembers = await _miembrosService.GetUserMiembrosAsync(farm.Id, forceRefresh: false);

            Members.Clear();
            foreach (var member in realMembers)
            {
                Members.Add(member);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Este comando lo puedes ligar a un botón con el icono de "flecha de actualizar" en tu XAML
    [RelayCommand]
    public async Task ForceRefreshMembersAsync(Farm farm)
    {
        /* if (IsLoading) return;

        IsLoading = true; */
        try
        {
            // Aquí SÍ obligamos al servicio a saltarse la caché e ir a Supabase
            var realMembers = await _miembrosService.GetUserMiembrosAsync(farm.Id, forceRefresh: true);

            Members.Clear();
            foreach (var member in realMembers)
            {
                Members.Add(member);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Comando que se ejecuta al hacer clic en una tarjeta de la lista
    [RelayCommand]
    public void OpenEditRole(MiembroGranja miembro)
    {
        SelectedMemberForEdit = miembro;
        IsEditRoleOpen = true;
        /* InviteSelectedRol = miembro.Rol?.ToLower() == "editor" ? "editor" : "viewer";
        switch (InviteSelectedRol)
        {
            case "editor":
                OnPropertyChanged(nameof(IsEditVisorSelected));
                break;
            case "viewer":
                OnPropertyChanged(nameof(IsEditAdminSelected));
                break;
        }

        var f=IsEditAdminSelected;
        var f2=IsEditVisorSelected; */

        System.Diagnostics.Debug.WriteLine($"SelectedMemberForEdit: {SelectedMemberForEdit}");

    }

    [RelayCommand]
    private void CloseEditRole()
    {
        IsEditRoleOpen = false;
        SelectedMemberForEdit = null;
    }

    // Comando para cambiar el rol (puedes pasarle "editor" o "viewer" como parámetro)
    [RelayCommand]
    private async Task ChangeRoleAsync(string nuevoRol)
    {
        if (SelectedMemberForEdit == null || IsLoading) return;

        IsLoading = true;
        try
        {
            string rolDb = nuevoRol.ToLower();

            // Aquí irá tu llamada de actualización al servicio en Supabase cuando la descomentes
            // bool exito = await _miembrosService.UpdateMemberRoleAsync(SelectedMemberForEdit.Id, rolDb);

            // Actualizamos la UI localmente
            SelectedMemberForEdit.Rol = nuevoRol;

            // Forzamos la actualización de los estilos de las tarjetas en el modal
            OnPropertyChanged(nameof(IsEditVisorSelected));
            OnPropertyChanged(nameof(IsEditAdminSelected));

            IsEditRoleOpen = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al actualizar rol: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Comando para el botón rojo "Revocar acceso"
    [RelayCommand]
    private async Task RevokeAccessAsync()
    {
        if (SelectedMemberForEdit == null || IsLoading) return;

        IsLoading = true;

        var f = SelectedMemberForEdit;

        System.Diagnostics.Debug.WriteLine($"SelectedMemberForEdit: {SelectedMemberForEdit}");
        System.Diagnostics.Debug.WriteLine($"SelectedMemberForEdit.Id: {SelectedMemberForEdit.UserId}");
        System.Diagnostics.Debug.WriteLine($"SelectedMemberForEdit.GranjaId: {SelectedMemberForEdit.GranjaId}");
        System.Diagnostics.Debug.WriteLine($"SelectedMemberForEdit.UserId: {SelectedMemberForEdit.UserId}");

        //System.Diagnostics.Debug.WriteLine($"miembro: {miembro.Id}");
        try
        {

            // 1. Borrar de la tabla 'miembros_granja' en Supabase
            await _supabase.From<MiembroGranjaWrite>()
            .Where(m => m.GranjaId == SelectedMemberForEdit.GranjaId)
            .Where(m => m.UserId == SelectedMemberForEdit.UserId)
            .Delete();

            // 2. Remover de la colección local (ObservableCollection) para que desaparezca de la lista al instante
            Members.Remove(SelectedMemberForEdit);

            // 3. Cerrar el panel flotante
            IsEditRoleOpen = false;
            SelectedMemberForEdit = null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al revocar acceso: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // 1. Añadimos propiedades para notificar los cambios calculados a la UI
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVisorSelected))]
    [NotifyPropertyChangedFor(nameof(IsAdminSelected))]
    [NotifyPropertyChangedFor(nameof(IsEditVisorSelected))]
    [NotifyPropertyChangedFor(nameof(IsEditAdminSelected))]
    private string _inviteSelectedRol = "viewer";

    // Si el rol es "viewer", esto devuelve true
    public bool IsVisorSelected => InviteSelectedRol == "viewer";

    // Si el rol es "editor", esto devuelve true
    public bool IsAdminSelected => InviteSelectedRol == "editor";

    // Propiedades calculadas para el estado del rol del miembro seleccionado
    public bool IsEditVisorSelected => SelectedMemberForEdit?.Rol?.ToLower() == "viewer";
    public bool IsEditAdminSelected => SelectedMemberForEdit?.Rol?.ToLower() == "editor";

    [RelayCommand]
    private void OpenAddMember()
    {
        InviteEmail = string.Empty;
        InviteSelectedRol = "viewer"; // Reseteamos a viewer por defecto al abrir
        IsAddMemberOpen = true;
    }

    [RelayCommand]
    private void CloseAddMember() => IsAddMemberOpen = false;

    // Comando para seleccionar el rol desde la UI
    [RelayCommand]
    private void SelectInviteRole(string role)
    {
        InviteSelectedRol = role.ToLower();
    }

    [RelayCommand]
    private async Task ExecuteAddMemberAsync()
    {
        if (string.IsNullOrWhiteSpace(InviteEmail) || CurrentFarm == null || IsLoading) return;

        IsLoading = true;



        try
        {
            // 1. PASO CLAVE: Buscar el uid en la tabla 'perfiles' que coincida con el email.
            // Nota: Asegúrate de tener expuesto el email en la tabla 'perfiles' o usar una Edge Function de Supabase.
            // Si tienes acceso directo, la lógica de tu MiembrosService debería verse similar a esto:

            var perfilDestino = await _supabase.From<Perfil>()
                .Where(x => x.Email == InviteEmail)
                .Single();


            if (perfilDestino == null)
            {
                // Manejar error de usuario no encontrado
                return;
            }

            System.Diagnostics.Debug.WriteLine($"InviteEmail: {InviteEmail}");
            System.Diagnostics.Debug.WriteLine($"CurrentFarm.Id: {CurrentFarm.Id}");
            System.Diagnostics.Debug.WriteLine($"perfilDestino.Id: {perfilDestino.Id}");
            System.Diagnostics.Debug.WriteLine($"InviteSelectedRol: {InviteSelectedRol}");
            System.Diagnostics.Debug.WriteLine($"_supabase.Auth.CurrentUser.Id: {_supabase.Auth.CurrentUser.Id}");
            // 2. Insertar en la tabla 'miembros_granja'
            var nuevoMiembroDb = new MiembroGranja
            {
                GranjaId = CurrentFarm.Id,
                UserId = perfilDestino.Id,
                Rol = InviteSelectedRol,
                InvitedBy = _supabase.Auth.CurrentUser.Id // Id del usuario loggeado actual
            };

            await _supabase.From<MiembroGranja>().Insert(nuevoMiembroDb);


            // 3. Si todo sale bien, refrescamos la lista automáticamente para ver al nuevo miembro
            CloseAddMember();

            await ForceRefreshMembersAsync(CurrentFarm);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al invitar miembro: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Back() => await _navigationService.NavigateToAsync<HomeViewModel>();
}