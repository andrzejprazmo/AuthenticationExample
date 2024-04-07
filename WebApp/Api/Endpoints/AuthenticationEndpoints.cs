﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.Authenticate;

namespace WebApp.Api.Endpoints;
public class AuthenticationEndpoints: IEndpointsModule
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", handler: Authenticate).WithName("Authenticate");
    }

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> Authenticate(HttpContext context, [FromServices] IMediator mediator, [FromBody] AuthenticateRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match((data) =>
        {
            //SetRefreshTokenCookie(context, data.RefreshToken);
            return Results.Ok(data.Token);
        }, (errors) => Results.BadRequest(errors));
    }

}
