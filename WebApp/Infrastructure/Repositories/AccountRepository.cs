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

    public async Task<AccountEntity?> GetAccountById(int id)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts WHERE id = @UserId";
        var result = await connection.QuerySingleOrDefaultAsync<AccountRecord>(sql, new
        {
            UserId = id,
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

    public async Task<IEnumerable<AccountEntity>> GetAllAccounts()
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts";
        var list = await connection.QueryAsync<AccountRecord>(sql);
        return list.Select(x => x.ToAccountEntity());
    }

    public async Task<int> CreateAccount(AccountEntity account)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"INSERT INTO accounts (login, password, first_name, last_name) VALUES (@Login, @Password, @FirstName, @LastName) RETURNING id";
        var result = await connection.QuerySingleAsync<int>(sql, account);
        return result;
    }

    public async Task<AccountEntity> EditAccount(int id)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, first_name, last_name FROM accounts WHERE id = @Id";
        var result = await connection.QuerySingleAsync<AccountRecord>(sql, new
        {
            Id = id,
        });
        return result.ToAccountEntity();
    }

    public async Task UpdateAccount(AccountEntity account)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"UPDATE accounts SET login = @Login, first_name = @FirstName, last_name = @LastName WHERE id = @Id";
        await connection.ExecuteAsync(sql, account);
    }

    public Task DeleteAccount(int id)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"DELETE FROM accounts WHERE id = @Id";
        return connection.ExecuteAsync(sql, new
        {
            Id = id,
        });
    }

    public async Task SetPassword(int id, string password)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"UPDATE accounts SET password = @Password WHERE id = @Id";
        await connection.ExecuteAsync(sql, new
        {
            Id = id,
            Password = password,
        });
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

    public async Task<int> GetAccountCount()
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT COUNT(*) FROM accounts";
        return await connection.QuerySingleAsync<int>(sql);
    }
}
