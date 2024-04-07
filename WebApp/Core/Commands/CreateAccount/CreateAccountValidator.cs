using FluentValidation;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Core.Commands.CreateAccount
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator(IAccountRepository accountRepository)
        {
            RuleFor(x => x.Login).NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .MustAsync(async (data, cancellation) => !await accountRepository.AccountExists(data)).WithErrorCode(ErrorCodes.ACCOUNT_LOGIN_EXISTS);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(50);
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
