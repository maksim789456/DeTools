using GogsDownloader;
using GogsDownloader.Database;

var config = Config.GetInstance();

var baseRepoStr = config.BaseGogsUrl;

if (string.IsNullOrWhiteSpace(baseRepoStr))
{
    Console.WriteLine("Empty Gogs url!");
    Console.ReadLine();
    return;
}

if (baseRepoStr.EndsWith('/'))
    baseRepoStr = baseRepoStr.Remove(baseRepoStr.Length - 1);

try
{
    Uri baseRepo = new Uri(baseRepoStr);
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadLine();
    return;
}

GogsDbContext dbContext = new GogsDbContext(config.ConnectionString, config.DatabaseType);
var users = config.UseExternalUsersFile
    ? UsersFileParser.ParseFile(config.PathToUsersFile).ToArray()
    : config.Users;

if (users.Length == 0)
{
    Console.WriteLine("No users data in config/users file!");
    Console.ReadLine();
    return;
}

foreach (var user in users)
{
    Console.WriteLine($"Check user '{user.Username}'");
    var gogsUser = dbContext.Users.FirstOrDefault(x => x.Name == user.Username);
    if (gogsUser is null)
    {
        Console.WriteLine($"User {user.Username} dont find in Gogs");
        continue;
    }

    var gogsUserRepos = dbContext.Repositories.Where(x => x.OwnerId == gogsUser.Id).ToArray();
    if (gogsUserRepos.Length == 0)
    {
        Console.WriteLine($"User {user.Username} dont contains any repos in Gogs");
        continue;
    }

    foreach (var repo in gogsUserRepos)
    {
        var path = Tools.RecreateUserRepoDir("userData", user.Username, repo.Name);
        Tools.CloneRepository(baseRepoStr, user.Username, user.Password, repo.Name, path, config.BranchesAsSeparateFolders);
    }
}

Console.ReadLine();