using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using WebApp.Api;
using WebApp.Domain.Configuration;

namespace WebApp.Installers;
public static class ApiInstaller
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        var rules = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && t.IsClass && typeof(IEndpointsModule).IsAssignableFrom(t));

        foreach (var rule in rules)
        {
            services.Add(new ServiceDescriptor(typeof(IEndpointsModule), rule, ServiceLifetime.Singleton));
        }

        return services;
    }

    public static void UseApi(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IEndpointsModule>();
        foreach (var endpoint in endpoints)
        {
            if (endpoint != null)
            {
                endpoint.RegisterEndpoints(app);
            }
        }
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("Cannot find JWT configuration");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        return services;
    }
}