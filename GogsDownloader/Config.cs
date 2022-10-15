namespace GogsDownloader;

public class Config
{
    public string BaseGogsUrl { get; set; } = "http://localhost:3000";
    public string ConnectionString { get; set; } = "";
    public User[] Users { get; set; } = Array.Empty<User>();
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}