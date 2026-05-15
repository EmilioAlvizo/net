using Supabase;

namespace AppAvalonia3.Services;
public class SupabaseService
{
    public Client Client { get; }

    public SupabaseService()
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };

        Client = new Client(
            "https://xagnnkqqtdtvadaseubc.supabase.co",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhhZ25ua3FxdGR0dmFkYXNldWJjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQ4MjYzMjEsImV4cCI6MjA5MDQwMjMyMX0.m8V3r4GTutaqvj0agfZig__Itxmtw5m2BKtPjoXpeXA",
            options
        );
    }
}