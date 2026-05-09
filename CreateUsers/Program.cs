using CreateUsers;
using Spectre.Console;

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