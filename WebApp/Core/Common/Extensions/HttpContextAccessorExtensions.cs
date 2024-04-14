namespace WebApp.Core.Common.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        const string RefreshTokenCookieKey = "RefreshToken";
        public static void SetRefreshToken(this IHttpContextAccessor contextAccessor, Guid refreshToken)
        {
            var context = contextAccessor.HttpContext;
            if (context is not null)
            {
                context.Response.Cookies.Append(RefreshTokenCookieKey, refreshToken.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }
        }

        public static Guid? GetRefreshToken(this IHttpContextAccessor contextAccessor)
        {
            var context = contextAccessor.HttpContext;
            if (context is not null && context.Request.Cookies.TryGetValue(RefreshTokenCookieKey, out var refreshToken))
            {
                if (Guid.TryParse(refreshToken, out var refreshTokenId))
                {
                    return refreshTokenId;
                }
            }
            return null;
        }

        public static void RemoveRefreshToken(this IHttpContextAccessor contextAccessor)
        {
            var context = contextAccessor.HttpContext;
            if (context is not null && context.Request.Cookies.ContainsKey(RefreshTokenCookieKey))
            {
                context.Response.Cookies.Delete(RefreshTokenCookieKey);
            }
        }
    }
}
