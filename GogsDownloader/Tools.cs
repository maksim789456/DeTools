using LibGit2Sharp;

namespace GogsDownloader;

public static class Tools
{
    public static void CloneRepository(string baseRepoUrl, string username, string password, string repoName, string pathToSave, bool withBranches = false)
    {
        var credentials = new UsernamePasswordCredentials
            { Username = username, Password = password };
        var cloneOptions = new CloneOptions
        {
            CredentialsProvider = (_, _, _) => credentials
        };
        var url = $"{baseRepoUrl}/{username}/{repoName}.git";
        Console.WriteLine($"Save {url} to {pathToSave}");
        Repository.Clone(url, pathToSave, cloneOptions);

        if (withBranches)
            CloneRemoteRepositoryBranches(pathToSave);
    }

    public static void CloneRemoteRepositoryBranches(string repoPath)
    {
        using var repo = new Repository(repoPath);

        // if only one remote branch -> skip branches grabbing
        if (repo.Branches.Count(x => x.IsRemote) == 1)
            return;

        // starting remote branches grabbing
        foreach (var remoteBranch in repo.Branches.Where(x => x.IsRemote))
        {
            // make local name from friendly name ('origin/test' to 'test')
            var localBranchName = remoteBranch.FriendlyName.Split('/').Last();
            // if branch with this name exists skip this remote
            if (repo.Branches[localBranchName] != null)
                continue;
            Console.WriteLine($"Grab branch '{remoteBranch.FriendlyName}' as '{localBranchName}'");
            // create local branch by local name
            Branch localBranch = repo.CreateBranch(localBranchName, remoteBranch.Tip);
            // link local branch to remote branch
            repo.Branches.Update(localBranch, b => b.UpstreamBranch = remoteBranch.UpstreamBranchCanonicalName);
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