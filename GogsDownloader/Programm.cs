using GogsDownloader;
using GogsDownloader.Database;
using Newtonsoft.Json;

var config = new Config();
if (!File.Exists("config.json"))
    File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
else
    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) ?? new Config();

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

foreach (var user in config.Users)
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
        Tools.CloneRepository(baseRepoStr, user.Username, user.Password, repo.Name, path, true);
    }
}

Console.ReadLine();