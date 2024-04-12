using FluentValidation;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;
using WebApp.Domain.Entities;

namespace WebApp.Core.Commands.Account.Update
{
    public record UpdateAccountRequest(int Id, string Login, string FirstName, string LastName) : IRequest<Result<bool>>;
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountRequest, Result<bool>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<UpdateAccountRequest> _validator;

        public UpdateAccountHandler(IAccountRepository accountRepository, IValidator<UpdateAccountRequest> validator)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<Result<bool>> Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                await _accountRepository.UpdateAccount(new AccountEntity
                {
                    Id = request.Id,
                    Login = request.Login,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                });
                return true;
            }
            return new Result<bool>(validationResult.Errors);
        }
    }
}
