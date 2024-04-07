using Dapper;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Providers;
using WebApp.Infrastructure.Mappers;
using WebApp.Infrastructure.Records;
using WebApp.Core.Common.Abstract;

namespace WebApp.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDatabaseConnectionProvider _connectionProvider;

    public AccountRepository(IDatabaseConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
    }

    public async Task<AccountEntity?> GetAccountById(int userId)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts WHERE id = @UserId";
        var result = await connection.QuerySingleOrDefaultAsync<AccountRecord>(sql, new
        {
            UserId = userId,
        });
        return result?.ToAccountEntity();
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

    public async Task<int> CreateAccount(AccountEntity account)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"INSERT INTO accounts (login, password, first_name, last_name) VALUES (@Login, @Password, @FirstName, @LastName) RETURNING id";
        var result = await connection.QuerySingleAsync<int>(sql, account);
        return result;
    }

    public async Task<bool> AccountExists(string login)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT EXISTS(SELECT 1 FROM accounts WHERE login = @Login)";
        var result = await connection.QuerySingleAsync<bool>(sql, new
        {
            Login = login,
        });
        return result;
    }

    public async Task<bool> AccountExists(string login, int id)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT EXISTS(SELECT 1 FROM accounts WHERE login = @Login AND id <> @Id)";
        var result = await connection.QuerySingleAsync<bool>(sql, new
        {
            Login = login,
            Id = id,
        });
        return result;
    }
}
