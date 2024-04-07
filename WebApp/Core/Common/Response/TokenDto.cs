namespace WebApp.Core.Common.Response
{
    public class TokenDto
    {
        public required string Token { get; set; }
        public required Guid RefreshToken { get; set; }
    }
}
