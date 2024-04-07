using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.Authenticate;
using WebApp.Core.Common.Const;
using WebApp.Core.Queries.RefreshToken;

namespace WebApp.Api.Endpoints;
public class AuthenticationEndpoints: IEndpointsModule
{
    const string RefreshTokenCookieKey = "RefreshToken";

    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", handler: Login).WithName("Authenticate");
        app.MapPost("/api/auth/logout", handler: Logout).WithName("Logout");
        app.MapGet("/api/auth/refresh-token/{token}", handler: RefreshToken).WithName("RefreshToken");
    }

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> Login(HttpContext context, [FromServices] IMediator mediator, [FromBody] AuthenticateRequest request)
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

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> RefreshToken(HttpContext context, [FromServices] IMediator mediator, [FromRoute] string token)
    {
        var refreshToken = GetRefreshTokenCookie(context);
        if (refreshToken == null || string.IsNullOrWhiteSpace(token))
        {
            return Results.Unauthorized();
        }
        var result = await mediator.Send(new RefreshTokenRequest(token, refreshToken.Value));
        return result.Match(data =>
        {
            SetRefreshTokenCookie(context, data.RefreshToken);
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

    private Guid? GetRefreshTokenCookie(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(RefreshTokenCookieKey, out var refreshToken))
        {
            if (Guid.TryParse(refreshToken, out var refreshTokenId))
            {
                return refreshTokenId;
            }
        }
        return null;
    }
}
