using FluentValidation;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;

namespace WebApp.Core.Commands.Account.Update
{
    public class UpdateAccountValidator : AbstractValidator<UpdateAccountRequest>
    {
        public UpdateAccountValidator(IAccountRepository accountRepository)
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Login).NotEmpty().MaximumLength(50).CustomAsync(async (data, context, cancellation) =>
            {
                if (await accountRepository.AccountExists(data, context.InstanceToValidate.Id))
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure
                    {
                        ErrorCode = ErrorCodes.ACCOUNT_LOGIN_EXISTS,
                    });
                }
            });
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        }
    }
}
