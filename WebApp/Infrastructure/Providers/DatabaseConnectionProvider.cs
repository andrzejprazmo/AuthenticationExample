using Microsoft.Data.Sqlite;

namespace WebApp.Infrastructure.Providers;

public interface IDatabaseConnectionProvider
{
    SqliteConnection GetConnection();
}
public class DatabaseConnectionProvider : IDatabaseConnectionProvider
{
    private readonly IConfiguration _configuration;

    public DatabaseConnectionProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(_configuration.GetConnectionString("Database"));
    }
}
