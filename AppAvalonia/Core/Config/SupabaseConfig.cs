namespace AppAvalonia.Services;

/// <summary>
/// Configuración de Supabase. 
/// Obtén estos valores en: Supabase Dashboard → Settings → API
/// </summary>
public record SupabaseConfig(
    string Url,        // https://xxxx.supabase.co
    string AnonKey    // eyJh... (anon/public key)
);