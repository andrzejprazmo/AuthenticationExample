using FluentValidation;
using WebApp.Core.Common.Const;
using WebApp.Core.Common.Helpers;

namespace WebApp.Core.Commands.Account.Password
{
    public class SetPasswordValidator : AbstractValidator<SetPasswordRequest>
    {
        public SetPasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty()
            .MaximumLength(50)
            .Must(PasswordHelper.IsStrong)
            .WithErrorCode(ErrorCodes.ACCOUNT_PASSWORD_IS_WEAK);
        }
    }
}
