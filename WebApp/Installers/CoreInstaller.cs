using FluentValidation;
using MediatR;
using WebApp.Core.Common;

namespace WebApp.Installers;
public static class CoreInstaller
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(CoreInstaller).Assembly);
        }).AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        services.AddValidatorsFromAssembly(typeof(CoreInstaller).Assembly);

        return services;
    }
}
