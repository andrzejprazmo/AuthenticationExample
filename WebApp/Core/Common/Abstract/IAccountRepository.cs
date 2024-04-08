using WebApp.Domain.Entities;

namespace WebApp.Core.Common.Abstract;

public interface IAccountRepository
{
    Task<AccountEntity?> GetAccountByLogin(string login);
    Task<AccountEntity?> GetAccountById(int userId);
    Task<IEnumerable<AccountEntity>> GetAllAccounts();
    Task<int> CreateAccount(AccountEntity account);
    Task DeleteAccount(int id);
    Task<bool> AccountExists(string login);
    Task<bool> AccountExists(string login, int id);
    Task<int> GetAccountCount();
}
