using FluentValidation;
using MediatR;
using System.Reflection;
using WebApp.Api;
using WebApp.Core.Common;
using WebApp.Core.Common.Strategy;

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

        services.AddTransient<IStrategyFactory, StrategyFactory>();

        var strategies = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && t.IsClass && typeof(IStrategy<StrategyType>).IsAssignableFrom(t));
            
        foreach (var strategy in strategies)
        {
            services.Add(new ServiceDescriptor(typeof(IStrategy<StrategyType>), strategy, ServiceLifetime.Transient));
        }
        return services;
    }
}
