// SupabaseAuthService.cs
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System;

namespace AppAvalonia3.Services;

public record LoginResult(bool Success, string? Token, string? UserId, string? ErrorMessage);

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string email, string password);
    Task LogoutAsync(); // 👈 Actualizado a asíncrono
    string? GetToken();
    string? GetUserId();
}

public class SupabaseAuthService : IAuthService
{
    private readonly Supabase.Client _supabaseClient;
    private readonly ISessionContext _session;

    // 1. Inyectamos Supabase.Client y tu SessionContext
    public SupabaseAuthService(Supabase.Client supabaseClient, ISessionContext session)
    {
        _supabaseClient = supabaseClient;
        _session = session;
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        try
        {
            // 2. Usamos el SDK para iniciar sesión
            var session = await _supabaseClient.Auth.SignIn(email, password);

            if (session?.User != null)
            {
                // 3. Guardamos en el Singleton de sesión (como lo tenías antes)
                _session.Token = session.AccessToken;
                _session.UserId = session.User.Id;

                return new LoginResult(true, _session.Token, _session.UserId, null);
            }

            return new LoginResult(false, null, null, "Error desconocido al intentar iniciar sesión.");
        }
        catch (Exception ex)
        {
            // El SDK de Supabase lanza excepciones (GotrueException) cuando falla el login.
            // La propiedad ex.Message contendrá el texto "Invalid login credentials" u otros errores.
            return new LoginResult(false, null, null, ex.Message);
        }
    }

    public async Task LogoutAsync()
    {
        // 4. Cerramos sesión en el servidor y limpiamos contexto local
        await _supabaseClient.Auth.SignOut();
        _session.Clear();
    }

    /* public async Task Register()
    {
        try
        {
            var session = await _supabaseClient.Auth.SignUp();

            if (session?.User != null)
            {
                // 3. Guardamos en el Singleton de sesión (como lo tenías antes)
                _session.Token = session.AccessToken;
                _session.UserId = session.User.Id;

            }

        }
        catch (Exception ex)
        {
            return 
        }
    } */

    public string? GetToken() => _session.Token; 
    public string? GetUserId() => _session.UserId; 
}
