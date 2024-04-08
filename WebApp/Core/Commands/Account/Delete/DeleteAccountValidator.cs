using FluentValidation;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Const;

namespace WebApp.Core.Commands.Account.Delete;

public class DeleteAccountValidator : AbstractValidator<DeleteAccountRequest>
{
    public DeleteAccountValidator(IAccountRepository accountRepository)
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0).MustAsync(async (data, cancellation) =>
        {
            var count = await accountRepository.GetAccountCount();
            return count > 1;
        }).WithErrorCode(ErrorCodes.ACCOUNT_CANNOT_DELETE_LAST);
    }
}
