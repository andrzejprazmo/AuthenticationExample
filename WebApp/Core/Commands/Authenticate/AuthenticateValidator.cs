using FluentValidation;

namespace WebApp.Core.Commands.Authenticate;

public class AuthenticateValidator : AbstractValidator<AuthenticateRequest>
{
    public AuthenticateValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
