using WebApp.Domain.Entities;

namespace WebApp.Core.Common.Abstract;

public interface IAccountRepository
{
    Task<AccountEntity?> GetAccountByLogin(string login, CancellationToken cancellationToken);
    Task<AccountEntity?> GetAccountById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<AccountEntity>> GetAllAccounts(CancellationToken cancellationToken);
    Task<int> CreateAccount(AccountEntity account, CancellationToken cancellationToken);
    Task<AccountEntity> EditAccount(int id, CancellationToken cancellationToken);
    Task UpdateAccount(AccountEntity account, CancellationToken cancellationToken);
    Task DeleteAccount(int id, CancellationToken cancellationToken);
    Task<bool> AccountExists(string login, CancellationToken cancellationToken);
    Task<bool> AccountExists(string login, int id, CancellationToken cancellationToken);
    Task<int> GetAccountCount(CancellationToken cancellationToken);
    Task SetPassword(int id, string password, CancellationToken cancellationToken);
    Task<int> ExecuteLongTermQuery(CancellationToken cancellationToken);
}
