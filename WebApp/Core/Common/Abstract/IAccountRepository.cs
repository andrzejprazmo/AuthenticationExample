using WebApp.Domain.Entities;

namespace WebApp.Core.Common.Abstract;

public interface IAccountRepository
{
    Task<AccountEntity?> GetAccountByLogin(string login);
    Task<AccountEntity?> GetAccountById(int userId);
    Task<int> CreateAccount(AccountEntity account);
    Task<bool> AccountExists(string login);
    Task<bool> AccountExists(string login, int id);
}
