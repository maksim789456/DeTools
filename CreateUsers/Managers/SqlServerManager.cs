using DotNetEnv;
using Microsoft.Data.SqlClient;

namespace CreateUsers.Managers;

public class SqlServerManager : IDatabaseManager, IDisposable, IAsyncDisposable
{
    private readonly string _sqlServerConnectionString = Env.GetString("SQLSERVER_CONNECTION_STRING");
    private SqlConnection _connection = null!;

    private readonly List<string> _dbNamesBlackList = ["master", "tempdb", "model", "msdb"];

    private readonly List<string> _dbUsersBlackList =
    [
        "Sa",
        "##MS_PolicyEventProcessingLogin##",
        "##MS_PolicyTsqlExecutionLogin##",
        "sa"
    ];

    public async Task<(bool, string)> CheckAccess()
    {
        if (string.IsNullOrWhiteSpace(_sqlServerConnectionString))
            return (false, "Отсутствует строка подключения к SqlServer");

        try
        {
            _connection = new SqlConnection(_sqlServerConnectionString);
            await _connection.OpenAsync();

            await using var command = _connection.CreateCommand();
            command.CommandText = "SELECT @@VERSION";
            await command.ExecuteScalarAsync();
        }
        catch (Exception ex)
        {
            return (false, "Ошибка подключения к SqlServer: " + ex.Message);
        }

        return (true, "");
    }

    public async Task<IReadOnlyList<string>> GetUsers(string? prefix = null)
    {
        var result = new List<string>();

        await using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT name
                              FROM sys.sql_logins
                              WHERE (@prefix IS NULL OR name LIKE @prefix + '%')
                              ORDER BY name
                              """;

        command.Parameters.AddWithValue("@prefix", (object?)prefix ?? DBNull.Value);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(reader.GetString(0));
        }

        return result.Except(_dbUsersBlackList).ToList();
    }

    public async Task<bool> CreateUser(string username, string password)
    {
        var escapedName = EscapeIdentifier(username);
        var escapedPassword = password.Replace("'", "''");

        try
        {
            await using var command = _connection.CreateCommand();
            command.CommandText = $"""
                                   IF NOT EXISTS (
                                       SELECT 1
                                       FROM sys.sql_logins
                                       WHERE name = N'{escapedName}'
                                   )
                                   BEGIN
                                       CREATE LOGIN [{escapedName}]
                                       WITH PASSWORD = N'{escapedPassword}',
                                            DEFAULT_DATABASE = [{escapedName}],
                                            CHECK_EXPIRATION = OFF,
                                            CHECK_POLICY = OFF;

                                       USE [{escapedName}];

                                       CREATE USER [{escapedName}]
                                       FOR LOGIN [{escapedName}];

                                       ALTER ROLE [db_owner]
                                       ADD MEMBER [{escapedName}];
                                   END
                                   """;

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteUser(string username)
    {
        try
        {
            await using var command = _connection.CreateCommand();
            command.CommandText = $"""
                                   IF EXISTS (
                                       SELECT 1
                                       FROM sys.sql_logins
                                       WHERE name = @username
                                   )
                                   BEGIN
                                       DROP LOGIN [{EscapeIdentifier(username)}];
                                   END
                                   """;

            command.Parameters.AddWithValue("@username", username);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<string>> GetDatabases(string? prefix = null)
    {
        var result = new List<string>();

        await using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT name
                              FROM sys.databases
                              WHERE (@prefix IS NULL OR name LIKE @prefix + '%')
                              ORDER BY name
                              """;

        command.Parameters.AddWithValue(
            "@prefix",
            (object?)prefix ?? DBNull.Value);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(reader.GetString(0));
        }

        return result.Except(_dbNamesBlackList).ToList();
    }

    public async Task<bool> CreateDatabase(string name)
    {
        try
        {
            await using var command = _connection.CreateCommand();
            command.CommandText = $"""
                                   IF NOT EXISTS (
                                       SELECT 1
                                       FROM sys.databases
                                       WHERE name = @databaseName
                                   )
                                   BEGIN
                                       CREATE DATABASE [{EscapeIdentifier(name)}];
                                   END
                                   """;

            command.Parameters.AddWithValue("@databaseName", name);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteDatabase(string username)
    {
        try
        {
            await using var command = _connection.CreateCommand();
            command.CommandText = $"""
                                   IF EXISTS (
                                       SELECT 1
                                       FROM sys.databases
                                       WHERE name = @databaseName
                                   )
                                   BEGIN
                                       ALTER DATABASE [{EscapeIdentifier(username)}]
                                       SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

                                       DROP DATABASE [{EscapeIdentifier(username)}];
                                   END
                                   """;

            command.Parameters.AddWithValue("@databaseName", username);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string EscapeIdentifier(string value)
    {
        return value.Replace("]", "]]");
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}