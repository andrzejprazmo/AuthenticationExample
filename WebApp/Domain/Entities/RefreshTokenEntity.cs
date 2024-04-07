namespace WebApp.Domain.Entities;

public class RefreshTokenEntity
{
    public int UserId { get; set; }
    public required DateTime Expires { get; set; }
}
