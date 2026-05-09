namespace CreateUsers.Managers;

public interface IAccountManager
{
    public Task<(bool, string)> CheckAccess();
    public Task<IReadOnlyList<string>> GetUsers(string? prefix);
    public Task<bool> CreateUser(string username, string password);
    public Task<bool> DeleteUser(string username);
}