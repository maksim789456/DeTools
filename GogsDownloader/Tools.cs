using LibGit2Sharp;

namespace GogsDownloader;

public static class Tools
{
    public static void CloneRepository(string baseRepoUrl, string username, string password, string repoName, string pathToSave, bool withBranches = false)
    {
        var credentials = new UsernamePasswordCredentials
            { Username = username, Password = password };
        var co = new CloneOptions
        {
            CredentialsProvider = (url, user, cred) => credentials
        };
        var url = $"{baseRepoUrl}/{username}/{repoName}.git";
        Console.WriteLine($"Save url to {pathToSave}");
        Repository.Clone(url, pathToSave, co);

        if (withBranches)
        {
            using (var repo = new Repository(pathToSave))
            {
                repo.Network.Remotes.Update("origin", x => x.Url = url);
                var options = new PushOptions
                {
                    CredentialsProvider = (url, user, cred) => credentials
                };
                repo.Network.Push(repo.Network.Remotes["origin"],repo.Refs.Select(x=>x.CanonicalName),options);
            }
        }
    }

    public static string RecreateUserRepoDir(string basePath, string username, string repoName)
    {
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        if (!Directory.Exists(Path.Combine(basePath, username)))
            Directory.CreateDirectory(Path.Combine(basePath, username));

        var path = Path.Combine(basePath, username, repoName);

        if (Directory.Exists(path))
        {
            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };
            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }
            Directory.Delete(path, true);
        }

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }
}