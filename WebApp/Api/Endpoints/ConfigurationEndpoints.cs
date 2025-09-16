
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Common.Response;
using WebApp.Core.Common.Strategy;

namespace WebApp.Api.Endpoints
{
    public class ConfigurationEndpoints : IEndpointsModule
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/config", handler: GetConfiguration).WithName("GlobalConfiguration");
            app.MapGet("/api/strategy/{strategyId:int}", handler: GetStrategy).WithName("GetStrategy");
        }

        [ProducesResponseType(200, Type = typeof(ConfigurationDto))]
        private IResult GetConfiguration(HttpContext context)
        {
            return Results.Ok(new ConfigurationDto
            {
                BaseDomain = context.Request.Host.Host,
            });

        }

        [ProducesResponseType(200, Type = typeof(void))]
        private IResult GetStrategy([FromServices]IStrategyFactory strategyFactory, int strategyId)
        {
            var strategy = strategyFactory.GetStrategy((StrategyType)strategyId);
            return Results.Ok();
        }
    }
}
