using GogsDownloader;
using Newtonsoft.Json;

var config = new Config();
if (!File.Exists("config.json"))
    File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
else
    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) ?? new Config();

var baseRepoStr = config.BaseGogsUrl;

if (string.IsNullOrWhiteSpace(baseRepoStr))
{
    Console.WriteLine("Пустой адресс gogs!");
    Console.ReadLine();
    return;
}

if (baseRepoStr.EndsWith('/'))
    baseRepoStr = baseRepoStr.Remove(baseRepoStr.Length - 1);
Console.WriteLine(baseRepoStr);

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

Console.Write("Введите имя пользователя: ");
var username = Console.ReadLine();

if (string.IsNullOrWhiteSpace(username))
{
    Console.WriteLine("Пустое имя пользователя!");
    Console.ReadLine();
    return;
}

Console.Write("Введите пароль пользователя: ");
var password = Console.ReadLine();

if (string.IsNullOrWhiteSpace(password))
{
    Console.WriteLine("Пустое имя пользователя!");
    Console.ReadLine();
    return;
}

Console.Write("Введите название репозитория: ");
var repoName = Console.ReadLine();

if (string.IsNullOrWhiteSpace(repoName))
{
    Console.WriteLine("Пустое название репозитория!");
    Console.ReadLine();
    return;
}

var path = Tools.RecreateUserRepoDir("userData", username, repoName);

Tools.CloneRepository(baseRepoStr, username, password, repoName, path, true);