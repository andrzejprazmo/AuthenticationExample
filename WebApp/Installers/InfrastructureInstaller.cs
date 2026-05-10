using MassTransit;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Notifications;
using WebApp.Infrastructure.Providers;
using WebApp.Infrastructure.Repositories;

namespace WebApp.Installers;
public static class InfrastructureInstaller
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseConnectionProvider, DatabaseConnectionProvider>();
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<ITokenRepository, TokenRepository>();

        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
            x.AddConsumer<UserAuthenticatedEventHandler>();
        });

        return services;
    }
}
