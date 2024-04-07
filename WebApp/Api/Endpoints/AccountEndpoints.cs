
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.CreateAccount;

namespace WebApp.Api.Endpoints
{
    public class AccountEndpoints : IEndpointsModule
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/account/create", handler: Create).WithName("Create");

        }

        [ProducesResponseType(200, Type = typeof(string))]
        private async Task<IResult> Create(HttpContext context, [FromServices] IMediator mediator, [FromBody] CreateAccountRequest request)
        {
            var result = await mediator.Send(request);
            return result.Match((data) =>
            {
                return Results.Ok(data);
            }, (errors) => Results.BadRequest(errors));
        }
    }
}
