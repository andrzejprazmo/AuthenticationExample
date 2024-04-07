using WebApp.Domain.Entities;

namespace WebApp.Core.Abstract;

public interface IAccountRepository
{
    Task<AccountEntity?> GetAccountByLogin(string login);
}
