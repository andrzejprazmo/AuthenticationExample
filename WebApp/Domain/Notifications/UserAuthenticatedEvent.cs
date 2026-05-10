namespace WebApp.Domain.Notifications;

public class UserAuthenticatedEvent
{
    public string Login { get; set; }
    public DateTime Timestamp { get; set; }
}
