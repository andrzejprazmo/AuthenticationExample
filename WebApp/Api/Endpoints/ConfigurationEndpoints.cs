
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Common.Response;

namespace WebApp.Api.Endpoints
{
    public class ConfigurationEndpoints : IEndpointsModule
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/config", handler: GetConfiguration).WithName("GlobalConfiguration");
        }

        [ProducesResponseType(200, Type = typeof(ConfigurationDto))]
        private IResult GetConfiguration(HttpContext context)
        {
            return Results.Ok(new ConfigurationDto
            {
                BaseDomain = context.Request.Host.Host,
            });

        }
    }
}
