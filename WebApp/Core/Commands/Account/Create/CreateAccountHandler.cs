using FluentValidation;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Helpers;
using WebApp.Domain.Entities;

namespace WebApp.Core.Commands.Account.Create;

public record CreateAccountRequest(string Login, string Password, string FirstName, string LastName) : IRequest<Result<int>>;
public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, Result<int>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<CreateAccountRequest> _validator;

    public CreateAccountHandler(IAccountRepository accountRepository, IValidator<CreateAccountRequest> validator)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<int>> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            var accountId = await _accountRepository.CreateAccount(new AccountEntity
            {
                Login = request.Login,
                Password = PasswordHelper.EncryptSSHA512(request.Login, request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
            });
            return accountId;
        }
        return new Result<int>(validationResult.Errors);
    }
}
