using MediatR;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Response;

namespace WebApp.Core.Queries.GetAllAccounts;

public record GetAllAccountsRequest : IRequest<IEnumerable<AccountDto>>;
public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsRequest, IEnumerable<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAllAccountsHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsRequest request, CancellationToken cancellationToken)
    {
        var list = await _accountRepository.GetAllAccounts();
        return list.Select(x => new AccountDto
        {
            Id = x.Id,
            Login = x.Login,
            FirstName = x.FirstName,
            LastName = x.LastName,
        });
    }
}
