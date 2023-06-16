using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GogsDownloader;

public class Config
{
    private static Config? instance;

    private Config()
    {
    }

    public static Config GetInstance()
    {
        if (instance != null) return instance;
        instance = new Config();
        if (!File.Exists("config.json"))
            File.WriteAllText("config.json", JsonConvert.SerializeObject(instance));
        else
            instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) ?? new Config();

        return instance;
    }

    public string BaseGogsUrl { get; set; } = "http://localhost:3000";
    public string ConnectionString { get; set; } = "";

    [JsonConverter(typeof(StringEnumConverter))]
    public DatabaseType DatabaseType { get; set; } = DatabaseType.MySql;

    public AccessUser[] Users { get; set; } = Array.Empty<AccessUser>();
    public bool UseExternalUsersFile { get; set; } = false;
    public string PathToUsersFile { get; set; } = "users.txt";
    public bool BranchesAsSeparateFolders { get; set; } = false;
}

public enum DatabaseType
{
    MsSql,
    Postgre,
    MySql,
    Sqlite
}