using FluentValidation;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Helpers;

namespace WebApp.Core.Commands.Account.Create;

public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountValidator(IAccountRepository accountRepository)
    {
        RuleFor(x => x.Login).NotEmpty()
        .MinimumLength(3)
        .MaximumLength(50)
        .MustAsync(async (data, cancellation) => !await accountRepository.AccountExists(data)).WithErrorCode(ErrorCodes.ACCOUNT_LOGIN_EXISTS);

        RuleFor(x => x.Password).NotEmpty()
            .MaximumLength(50)
            .Must(PasswordHelper.IsStrong)
            .WithErrorCode(ErrorCodes.ACCOUNT_PASSWORD_IS_WEAK);
        
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}
