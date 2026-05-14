namespace AppAvalonia3.Services;

public interface ISessionContext
{
    string? Token { get; set; }
    string? UserId { get; set; }
    void Clear();
}

public class SessionContext : ISessionContext
{
    public string? Token { get; set; }
    public string? UserId { get; set; }

    public void Clear()
    {
        Token = null;
        UserId = null;
    }
}
