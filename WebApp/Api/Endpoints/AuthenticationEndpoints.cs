using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.Authenticate;
using WebApp.Core.Commands.RefreshToken;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Extensions;

namespace WebApp.Api.Endpoints;
public class AuthenticationEndpoints : IEndpointsModule
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", handler: Login).WithName("Authenticate");
        app.MapPost("/api/auth/logout", handler: Logout).WithName("Logout");
        app.MapPost("/api/auth/refresh-token", handler: RefreshToken).WithName("RefreshToken");
    }

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> Login(HttpContext context, [FromServices] IMediator mediator, [FromBody] AuthenticateRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match((data) =>
        {
            return Results.Ok(data.Token);
        }, (errors) => Results.BadRequest(errors));
    }

    [ProducesResponseType(200)]
    private IResult Logout([FromServices] IHttpContextAccessor contextAccessor)
    {
        contextAccessor.RemoveRefreshToken();
        return Results.Ok();
    }

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> RefreshToken(HttpContext context, [FromServices] IMediator mediator, [FromBody] RefreshTokenRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match(data =>
        {
            return Results.Ok(data.Token);
        }, (errors) =>
        {
            if (errors.Any(e => e.ErrorCode == ErrorCodes.AUTHENTICATE_REFRESHTOKEN_EXPIRED))
            {
                return Results.Unauthorized();
            }
            return Results.StatusCode(500);
        });

    }
}
