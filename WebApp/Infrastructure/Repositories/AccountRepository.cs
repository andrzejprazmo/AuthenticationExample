using Dapper;
using WebApp.Core.Abstract;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Providers;
using WebApp.Infrastructure.Mappers;
using WebApp.Infrastructure.Records;

namespace WebApp.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDatabaseConnectionProvider _connectionProvider;

    public AccountRepository(IDatabaseConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
    }

    public async Task<AccountEntity?> GetAccountByLogin(string login)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts WHERE login = @Login";
        var result = await connection.QuerySingleOrDefaultAsync<AccountRecord>(sql, new
        {
            Login = login,
        });
        return result?.ToAccountEntity();
    }
}
