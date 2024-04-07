using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Response;

namespace WebApp.Core.Commands.Authenticate
{
    public record AuthenticateRequest(string EmailAddress, string Password) : IRequest<Result<TokenDto>>;

    public class AuthenticateHandler
    {
    }
}
