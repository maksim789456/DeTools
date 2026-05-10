using System.Text.RegularExpressions;
using DotNetEnv;
using MySql.Data.MySqlClient;

namespace CreateUsers.Managers;

public partial class MySqlManager : IDatabaseManager, IDisposable, IAsyncDisposable
{
    private readonly string _mySqlConnectionString = Env.GetString("MYSQL_CONNECTION_STRING");
    private MySqlConnection _connection = null!;

    private readonly List<string> _dbNamesBlackList = ["mysql", "information_schema", "performance_schema"];

    private readonly List<string> _dbUsersBlackList =
    [
        "mysql.infoschema",
        "mysql.session",
        "mysql.sys",
        "root"
    ];

    [GeneratedRegex("^[a-zA-Z0-9_]+$")]
    private static partial Regex MySqlSafeNameRegex();

    public async Task<(bool, string)> CheckAccess()
    {
        if (string.IsNullOrWhiteSpace(_mySqlConnectionString))
            return (false, "Отсутствует строка подключения к MySql серверу");

        try
        {
            _connection = new MySqlConnection(_mySqlConnectionString);
            await _connection.OpenAsync();

            await using var command = _connection.CreateCommand();
            command.CommandText = "SELECT VERSION()";
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            return (false, "Ошибка подключения к MySql серверу: " + e.Message);
        }

        return (true, "");
    }

    public async Task<IReadOnlyList<string>> GetUsers(string? prefix)
    {
        var users = new List<string>();

        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = """
                          SELECT User
                          FROM mysql.user
                          WHERE (@prefix IS NULL OR User LIKE CONCAT(@prefix, '%'))
                          ORDER BY User
                          """;
        cmd.Parameters.AddWithValue("@prefix", (object?)prefix ?? DBNull.Value);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(reader.GetString(0));
        }

        return users.Except(_dbUsersBlackList).ToList();
    }

    public async Task<bool> CreateUser(string username, string password)
    {
        if (!MySqlSafeNameRegex().IsMatch(username))
            return false;

        await using var transaction = await _connection.BeginTransactionAsync();
        try
        {
            await using var cmd1 = _connection.CreateCommand();
            cmd1.Transaction = transaction;
            cmd1.CommandText = $"CREATE USER IF NOT EXISTS '{username}'@'%' IDENTIFIED BY @password";
            cmd1.Parameters.AddWithValue("@password", password);
            await cmd1.ExecuteNonQueryAsync();

            await using var cmd2 = _connection.CreateCommand();
            cmd2.Transaction = transaction;
            cmd2.CommandText = $"GRANT ALL PRIVILEGES ON `{username}`.* TO '{username}'@'%'";
            await cmd2.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteUser(string username)
    {
        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = $"DROP USER IF EXISTS `{username}`@'%'";
        return await cmd.ExecuteNonQueryAsync() >= 0;
    }

    public async Task<IReadOnlyList<string>> GetDatabases(string? prefix)
    {
        var databases = new List<string>();

        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = """
                          SELECT SCHEMA_NAME
                          FROM INFORMATION_SCHEMA.SCHEMATA
                          WHERE (@prefix IS NULL OR SCHEMA_NAME LIKE CONCAT(@prefix, '%'))
                          ORDER BY SCHEMA_NAME
                          """;
        cmd.Parameters.AddWithValue("@prefix", (object?)prefix ?? DBNull.Value);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            databases.Add(reader.GetString(0));
        }

        return databases.Except(_dbNamesBlackList).ToList();
    }

    public async Task<bool> CreateDatabase(string name)
    {
        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{name}`";
        return await cmd.ExecuteNonQueryAsync() >= 0;
    }

    public async Task<bool> DeleteDatabase(string name)
    {
        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = $"DROP DATABASE IF EXISTS `{name}`";
        return await cmd.ExecuteNonQueryAsync() >= 0;
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