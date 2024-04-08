using FluentValidation;
using MediatR;
using WebApp.Core.Common;
using WebApp.Core.Common.Abstract;

namespace WebApp.Core.Commands.Account.Delete;

public record DeleteAccountRequest(int Id) : IRequest<Result<bool>>;
public class DeleteAccountHandler : IRequestHandler<DeleteAccountRequest, Result<bool>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<DeleteAccountRequest> _validator;

    public DeleteAccountHandler(IAccountRepository accountRepository, IValidator<DeleteAccountRequest> validator)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<bool>> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            await _accountRepository.DeleteAccount(request.Id);
            return true;
        }
        return new Result<bool>(validationResult.Errors);
    }
}
