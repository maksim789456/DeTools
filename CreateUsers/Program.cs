using CreateUsers;
using CreateUsers.Managers;
using CreateUsers.Models;
using DotNetEnv;
using Spectre.Console;

Env.Load();

var gogs = new GogsManager();
var gitea = new GiteaManager();
var sqlServer = new SqlServerManager();
var mysql = new MySqlManager();

AnsiConsole.Clear();
var checkRes = await AnsiConsole.Status().StartAsync("Проверяем сервера и доступы", async _ =>
{
    var gogsRes = await gogs.CheckAccess();
    AnsiConsole.MarkupLine(gogsRes.Item1 ? "[green]✓ Gogs доступен[/]" : $"[red]{gogsRes.Item2}[/]");

    var giteaRes = await gitea.CheckAccess();
    AnsiConsole.MarkupLine(giteaRes.Item1 ? "[green]✓ Gitea доступен[/]" : $"[red]{giteaRes.Item2}[/]");

    var sqlServerRes = await sqlServer.CheckAccess();
    AnsiConsole.MarkupLine(sqlServerRes.Item1 ? "[green]✓ SqlServer доступен[/]" : $"[red]{sqlServerRes.Item2}[/]");

    var mysqlRes = await mysql.CheckAccess();
    AnsiConsole.MarkupLine(mysqlRes.Item1 ? "[green]✓ MySql доступен[/]" : $"[red]{mysqlRes.Item2}[/]");

    var anyGit = gogsRes.Item1 || giteaRes.Item1;
    if (anyGit && sqlServerRes.Item1 && mysqlRes.Item1) return true;
    AnsiConsole.MarkupLine("[red]Один из серверов недоступен или не хватает прав[/]");
    return false;
});
if (!checkRes)
    return 1;

var groupName = Dialogues.AskGroupName();
Console.WriteLine(groupName);

var users = Dialogues.MakeUsers(groupName);
Dialogues.PrintUsers(users);

var accountTypes = Dialogues.AskAccountTypes();

var tasks = users
    .SelectMany(user => accountTypes.Select(accountType =>
    {
        IAccountManager manager = accountType switch
        {
            AccountType.Gogs => gogs,
            AccountType.Gitea => gitea,
            AccountType.SqlServer => sqlServer,
            AccountType.MySql => mysql,
            _ => throw new ArgumentOutOfRangeException()
        };

        return (manager, user);
    }))
    .ToList();

foreach (var task in tasks)
{
    if (task.manager is IDatabaseManager dbManager)
    {
        var dbRes = await dbManager.CreateDatabase(task.user.Username);
        if (!dbRes)
            AnsiConsole.MarkupLine(
                $"[red]Ошибка при создании базы данных {task.user.Username} в {task.manager.Name}[/]");
    }

    var userRes = await task.manager.CreateUser(task.user);
    if (!userRes)
        AnsiConsole.MarkupLine($"[red]Ошибка при создании пользователя {task.user.Username} в {task.manager.Name}[/]");
}

AnsiConsole.MarkupLine("[green]✓ Все пользователи/БД созданы[/]");
var exportFilename = $"{groupName}_users.csv";
File.WriteAllLines(exportFilename, users.Select(u => $"{groupName},{u.Username},{u.Password}"));
AnsiConsole.MarkupLine($"[green] Логины и пароли пользователей выгружены в {exportFilename}[/]");

sqlServer.Dispose();
mysql.Dispose();
return 0;