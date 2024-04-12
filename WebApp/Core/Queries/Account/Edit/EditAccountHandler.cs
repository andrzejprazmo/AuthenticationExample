using MediatR;
using WebApp.Core.Common.Abstract;
using WebApp.Core.Common.Response;

namespace WebApp.Core.Queries.Account.Edit
{
    public record EditAccountRequest(int Id) : IRequest<AccountDto>;
    public class EditAccountHandler : IRequestHandler<EditAccountRequest, AccountDto>
    {
        private readonly IAccountRepository _accountRepository;

        public EditAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<AccountDto> Handle(EditAccountRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.EditAccount(request.Id);
            return new AccountDto
            {
                Id = account.Id,
                Login = account.Login,
                FirstName = account.FirstName,
                LastName = account.LastName
            };
        }
    }
}
