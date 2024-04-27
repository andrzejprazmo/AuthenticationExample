using FluentValidation;
using FluentValidation.Results;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Helpers;

namespace WebApp.Core.Commands.Account.Password
{
    public record SetPasswordRequest(int Id, string Password) : IRequest<Result<bool>>;
    public class SetPasswordHandler : IRequestHandler<SetPasswordRequest, Result<bool>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<SetPasswordRequest> _validator;

        public SetPasswordHandler(IValidator<SetPasswordRequest> validator, IAccountRepository accountRepository)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<Result<bool>> Handle(SetPasswordRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (validationResult.IsValid)
            {
                var account = await _accountRepository.GetAccountById(request.Id);
                if (account != null)
                {
                    var encryptedPassword = PasswordHelper.EncryptSSHA512(account.Login, request.Password);
                    await _accountRepository.SetPassword(request.Id, encryptedPassword);
                    return true;
                }
                return new Result<bool>(new List<ValidationFailure> { new ValidationFailure(string.Empty, "Cannot find id") });
            }
            return new Result<bool>(validationResult.Errors);
        }
    }
}
