using MassTransit;
using WebApp.Domain.Notifications;

namespace WebApp.Core.Notifications;

public class UserAuthenticatedEventHandler : IConsumer<UserAuthenticatedEvent>
{
    public Task Consume(ConsumeContext<UserAuthenticatedEvent> context)
    {
        Console.WriteLine($"User authenticated: {context.Message.Login} at {context.Message.Timestamp}");
        return Task.CompletedTask;
    }
}
