using CreateUsers;
using CreateUsers.Managers;
using DotNetEnv;
using Spectre.Console;

Env.Load();

var gogs = new GogsManager();
var sqlServer = new SqlServerManager();
var mysql = new MySqlManager();

AnsiConsole.Clear();
var checkRes = await AnsiConsole.Status().StartAsync("Проверяем сервера и доступы", async _ =>
{
    var gogsRes = await gogs.CheckAccess();
    AnsiConsole.MarkupLine(gogsRes.Item1 ? "[green]✓ Gogs доступен[/]" : $"[red]{gogsRes.Item2}[/]");

    var sqlServerRes = await sqlServer.CheckAccess();
    AnsiConsole.MarkupLine(sqlServerRes.Item1 ? "[green]✓ SqlServer доступен[/]" : $"[red]{sqlServerRes.Item2}[/]");

    var mysqlRes = await mysql.CheckAccess();
    AnsiConsole.MarkupLine(mysqlRes.Item1 ? "[green]✓ MySql доступен[/]" : $"[red]{mysqlRes.Item2}[/]");

    if (gogsRes.Item1 && sqlServerRes.Item1 && mysqlRes.Item1) return true;
    AnsiConsole.MarkupLine("[red]Один из серверов недоступен или не хватает прав[/]");
    return false;

});
if (!checkRes)
    return 1;

var groupName = Dialogues.AskGroupName();
Console.WriteLine(groupName);

Dialogues.AskUsersCreateSettings();

var accountTypes = AnsiConsole.Prompt(
    new MultiSelectionPrompt<AccountType>()
        .Title("Выберете где создавать аккаунты:")
        .AddChoices(Enum.GetValues<AccountType>().Cast<AccountType>())
        .Select(AccountType.Gogs)
        .Select(AccountType.MySql)
        .Select(AccountType.SqlServer)
);

foreach (var accountType in accountTypes)
{
    Console.WriteLine($"{accountType}");
}

sqlServer.Dispose();
return 0;