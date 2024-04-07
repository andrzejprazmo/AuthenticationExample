using WebApp.Core.Common.Abstract;
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

        return services;
    }
}
