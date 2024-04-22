using FluentValidation;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;

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
                await _accountRepository.SetPassword(request.Id, request.Password);
                return true;
            }
            return new Result<bool>(validationResult.Errors);
        }
    }
}
