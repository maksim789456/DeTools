using System.Text.RegularExpressions;

namespace GogsDownloader;

public static class UsersFileParser
{
    private static readonly Regex _loginRegex = new (@"^Логин:\s*(.*)\s$");
    private static readonly Regex _passwordRegex = new (@"^Пароль:\s*(.*)\s$");

    public static IEnumerable<User> ParseFile(string pathToFile)
    {
        if (!File.Exists(pathToFile))
        {
            Console.WriteLine("Users file dont exists!");
            return Array.Empty<User>();
        }

        var users = new List<User>();

        var lines = File.ReadAllLines(pathToFile);
        var lastParcedLogin = "";
        var nextLineIsPass = false;
        foreach (var line in lines)
        {
            if (!nextLineIsPass)
            {
                var match = _loginRegex.Match(line);
                if (match.Success)
                {
                    lastParcedLogin = match.Groups[1].Value;
                    nextLineIsPass = true;
                }
            }
            else
            {
                nextLineIsPass = false;
                var match = _passwordRegex.Match(line);
                if (match.Success)
                {
                    var password = match.Groups[1].Value;
                    users.Add(new User(username: lastParcedLogin, password: password));
                }
            }
        }

        return users;
    }
}