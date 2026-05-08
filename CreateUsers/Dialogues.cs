using NickBuhro.Translit;
using Spectre.Console;

namespace CreateUsers;

public static class Dialogues
{
    public static string AskGroupName()
    {
        var groupName = AnsiConsole.Prompt(new TextPrompt<string>("Название группы:"));
        return Transliteration.CyrillicToLatin(groupName, Language.Russian).ToUpper();
    }

    public static void AskUsersCreateSettings()
    {
        var idType = AnsiConsole.Prompt(
            new SelectionPrompt<IdType>()
                .Title("Выберете как создавать аккаунты:")
                .UseConverter(x => Utils.EnumGetDescriptionConverter(x))
                .AddChoices(Enum.GetValues(typeof(IdType)).Cast<IdType>())
        );

        switch (idType)
        {
            case IdType.WorkspaceNumber:
                AskUsersCreateWorkspaces();
                break;
            case IdType.GroupList:
                AskUsersCreateGroupList();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void AskUsersCreateWorkspaces()
    {
        var maxWorkspaceNumber = AnsiConsole.Ask("Количество рабочих мест:", 20);
        Console.WriteLine(maxWorkspaceNumber);
    }

    public static void AskUsersCreateGroupList()
    {
        var groupListFilepath = AnsiConsole.Prompt(
            new TextPrompt<string>("Путь до списка студентов:")
                .Validate(path => File.Exists(path)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Такого файла не существует[/]"))
                .Validate(path => !string.IsNullOrWhiteSpace(File.ReadAllText(path))
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Файл пустой[/]"))
        );
        Console.WriteLine(groupListFilepath);
    }
}