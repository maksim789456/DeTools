namespace CreateUsers.Models;

public interface IAccountManager
{
    public string Name { get; }
    public Task<(bool, string)> CheckAccess();
    public Task<IReadOnlyList<string>> GetUsers(string? prefix);
    public Task<bool> CreateUser(User user);
    public Task<bool> DeleteUser(string username);
}