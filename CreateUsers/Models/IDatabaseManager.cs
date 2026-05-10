namespace CreateUsers.Models;

public interface IDatabaseManager : IAccountManager
{
    public Task<IReadOnlyList<string>> GetDatabases(string? prefix);
    public Task<bool> CreateDatabase(string name);
    public Task<bool> DeleteDatabase(string name);
}