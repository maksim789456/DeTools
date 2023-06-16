namespace GogsDownloader;

public class AccessUser
{
    public AccessUser(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; }
    public string Password { get; }
}