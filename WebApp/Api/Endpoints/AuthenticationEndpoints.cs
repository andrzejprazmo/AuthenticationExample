using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.Authenticate;

namespace WebApp.Api.Endpoints;
public class AuthenticationEndpoints: IEndpointsModule
{
    const string RefreshTokenCookieKey = "RefreshToken";

    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", handler: Authenticate).WithName("Authenticate");
        app.MapPost("/api/auth/logout", handler: Logout).WithName("Logout");
    }

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> Authenticate(HttpContext context, [FromServices] IMediator mediator, [FromBody] AuthenticateRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match((data) =>
        {
            SetRefreshTokenCookie(context, data.RefreshToken);
            return Results.Ok(data.Token);
        }, (errors) => Results.BadRequest(errors));
    }

    [ProducesResponseType(200)]
    private IResult Logout(HttpContext context)
    {
        RemoveRefreshTokenCookie(context);
        return Results.Ok();
    }


    private void SetRefreshTokenCookie(HttpContext context, Guid refreshToken)
    {
        context.Response.Cookies.Append(RefreshTokenCookieKey, refreshToken.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    private void RemoveRefreshTokenCookie(HttpContext context)
    {
        context.Response.Cookies.Delete(RefreshTokenCookieKey);
    }
}
