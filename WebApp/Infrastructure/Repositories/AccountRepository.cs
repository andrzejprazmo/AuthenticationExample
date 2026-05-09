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

    public async Task<AccountEntity?> GetAccountById(int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts WHERE id = @UserId";
        var commandDefinition = new CommandDefinition(sql, new
        {
            UserId = id,
        }, cancellationToken: cancellationToken);
        var result = await connection.QuerySingleOrDefaultAsync<AccountRecord>(commandDefinition);
        return result?.ToAccountEntity();
    }

    public async Task<AccountEntity?> GetAccountByLogin(string login, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts WHERE login = @Login";
        var commandDefinition = new CommandDefinition(sql, new
        {
            Login = login,
        }, cancellationToken: cancellationToken);
        var result = await connection.QuerySingleOrDefaultAsync<AccountRecord>(commandDefinition);
        return result?.ToAccountEntity();
    }

    public async Task<IEnumerable<AccountEntity>> GetAllAccounts(CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, password, first_name, last_name FROM accounts";
        var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var list = await connection.QueryAsync<AccountRecord>(commandDefinition);
        return list.Select(x => x.ToAccountEntity());
    }

    public async Task<int> CreateAccount(AccountEntity account, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"INSERT INTO accounts (login, password, first_name, last_name) VALUES (@Login, @Password, @FirstName, @LastName) RETURNING id";
        var commandDefinition = new CommandDefinition(sql, account, cancellationToken: cancellationToken);
        var result = await connection.QuerySingleAsync<int>(commandDefinition);
        return result;
    }

    public async Task<AccountEntity> EditAccount(int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT id, login, first_name, last_name FROM accounts WHERE id = @Id";
        var commandDefinition = new CommandDefinition(sql, new
        {
            Id = id,
        }, cancellationToken: cancellationToken);
        var result = await connection.QuerySingleAsync<AccountRecord>(commandDefinition);
        return result.ToAccountEntity();
    }

    public async Task UpdateAccount(AccountEntity account, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"UPDATE accounts SET login = @Login, first_name = @FirstName, last_name = @LastName WHERE id = @Id";
        var commandDefinition = new CommandDefinition(sql, account, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(commandDefinition);
    }

    public async Task DeleteAccount(int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"DELETE FROM accounts WHERE id = @Id";
        var commandDefinition = new CommandDefinition(sql, new
        {
            Id = id,
        }, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(commandDefinition);
    }

    public async Task SetPassword(int id, string password, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"UPDATE accounts SET password = @Password WHERE id = @Id";
        var commandDefinition = new CommandDefinition(sql, new
        {
            Id = id,
            Password = password,
        }, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(commandDefinition);
    }

    public async Task<bool> AccountExists(string login, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT EXISTS(SELECT 1 FROM accounts WHERE login = @Login)";
        var commandDefinition = new CommandDefinition(sql, new
        {
            Login = login,
        }, cancellationToken: cancellationToken);
        var result = await connection.QuerySingleAsync<bool>(commandDefinition);
        return result;
    }

    public async Task<bool> AccountExists(string login, int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT EXISTS(SELECT 1 FROM accounts WHERE login = @Login AND id <> @Id)";
        var commandDefinition = new CommandDefinition(sql, new
        {
            Login = login,
            Id = id,
        }, cancellationToken: cancellationToken);
        var result = await connection.QuerySingleAsync<bool>(commandDefinition);
        return result;
    }

    public async Task<int> GetAccountCount(CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"SELECT COUNT(*) FROM accounts";
        var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<int>(commandDefinition);
    }

    public async Task<int> ExecuteLongTermQuery(CancellationToken cancellationToken)
    {
        using var connection = _connectionProvider.GetConnection();
        string sql = @"WITH RECURSIVE r(i) AS (
                VALUES (0)
    
                UNION ALL
    
                SELECT i + 1
                FROM r
                WHERE i < 30000000
            )
            SELECT i
            FROM r
            WHERE i = 1;";
        var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<int>(commandDefinition);
    }
}
