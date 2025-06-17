using System.Text;
using AsciiTableFormatter;
using DbChecker;
using GogsDownloader;

const string DB_IP = "localhost";
string[] IgnoredTables = ["sysdiagrams", "__EFMigrationsHistory"];

var users = UsersFileParser.ParseFile("users.txt");
var sb = new StringBuilder();
foreach (var user in users)
{
    Console.WriteLine(user.Username);
    sb.AppendLine(user.Username);
    var tables = DbTableGettingService
        .GetTableStat($"Server={DB_IP};Database={user.Username};User Id={user.Username};Password={user.Password};")
        .Where(x => !IgnoredTables.Contains(x.Item1))
        .Select(x => new
        {
            Table = x.Item1,
            EntitiesInTable = x.Item2
        }).ToArray();
    if (tables.Length == 0)
    {
        Console.WriteLine("No tables");
        sb.AppendLine("No tables");
        Console.WriteLine();
        sb.AppendLine();
        continue;
    }

    var output = Formatter.Format(tables);
    Console.WriteLine(output);
    sb.AppendLine(output);
}

if (File.Exists("out.txt"))
    File.Delete("out.txt");
File.WriteAllText("out.txt", sb.ToString());