using NickBuhro.Translit;
using Spectre.Console;

namespace CreateUsers;

public static class Dialogues
{
    public static string AskGroupName()
    {
        var groupName = AnsiConsole.Prompt(new TextPrompt<string>("Название группы:"));
        return Transliteration.CyrillicToLatin(groupName, Language.Russian).Replace('-', '_').ToUpper();
    }

    public static IReadOnlyList<(string, string)> MakeUsers(string groupName)
    {
        var idType = AnsiConsole.Prompt(
            new SelectionPrompt<IdType>()
                .Title("Выберете как создавать аккаунты:")
                .UseConverter(x => Utils.EnumGetDescriptionConverter(x))
                .AddChoices(Enum.GetValues<IdType>().Cast<IdType>())
        );

        var users = new List<(string, string)>();

        switch (idType)
        {
            case IdType.WorkspaceNumber:
                var workspacesCount = AskWorkspacesCount();
                users.AddRange(Enumerable.Range(1, workspacesCount)
                    .Select(i => ($"{groupName}_{i}", Utils.GeneratePassword())));
                break;
            case IdType.GroupList:
                var groupListFilepath = AskGroupListFilepath();
                var lines = File.ReadLines(groupListFilepath);
                users.AddRange(lines.Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Split(','))
                    .Select(x => x[0])
                    .Select(x => Transliteration.CyrillicToLatin(x, Language.Russian))
                    .Select(x => ($"{groupName}_{x}", Utils.GeneratePassword()))
                );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return users;
    }

    public static int AskWorkspacesCount() => AnsiConsole.Prompt(new TextPrompt<int>("Количество рабочих мест:")
        .DefaultValue(20)
        .Validate(count => count > 0
            ? ValidationResult.Success()
            : ValidationResult.Error("[red]Количество рабочих мест должно быть больше 0[/]")));

    public static string AskGroupListFilepath() => AnsiConsole.Prompt(
        new TextPrompt<string>("Путь до списка студентов:")
            .Validate(path => File.Exists(path)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Такого файла не существует[/]"))
            .Validate(path => !string.IsNullOrWhiteSpace(File.ReadAllText(path))
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Файл пустой[/]")));
}