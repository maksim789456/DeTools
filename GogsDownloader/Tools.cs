using LibGit2Sharp;

namespace GogsDownloader;

public static class Tools
{
    public static void CloneRepository(string baseRepoUrl, string username, string password, string repoName,
        string pathToSave, bool branchesAsSeparateFolders)
    {
        var credentials = new UsernamePasswordCredentials
            { Username = username, Password = password };
        var cloneOptions = new CloneOptions
        {
            CredentialsProvider = (_, _, _) => credentials
        };
        var url = $"{baseRepoUrl}/{username}/{repoName}.git";
        Console.WriteLine($"Save {url} to {pathToSave}");
        if (!branchesAsSeparateFolders)
        {
            Console.WriteLine("Grabbing remote branches as local");
            LibGit2Sharp.Repository.Clone(url, pathToSave, cloneOptions);
        }
        else
        {
            Console.WriteLine("Grabbing branches as folders");
            var tempFolder = Path.Combine(pathToSave, ".temp");
            RecreateDirectory(tempFolder);
            LibGit2Sharp.Repository.Clone(url, tempFolder, cloneOptions);
            var branches = CloneRemoteRepositoryBranches(tempFolder).ToArray();
            foreach (var branch in branches)
            {
                Console.WriteLine($"{branch} as work");
                var branchPath = Path.Combine(pathToSave, branch);
                RecreateDirectory(branchPath);
                CopyFilesRecursively(tempFolder, branchPath);
                using var repo = new LibGit2Sharp.Repository(branchPath);
                Commands.Checkout(repo, branch);
            }

            RecursiveDeleteDirectory(tempFolder);
        }
    }

    /// <summary>
    /// Clone remote branches as local branches
    /// </summary>
    /// <param name="repoPath">local repository path</param>
    public static IEnumerable<string> CloneRemoteRepositoryBranches(string repoPath)
    {
        using var repo = new LibGit2Sharp.Repository(repoPath);

        // starting remote branches grabbing
        foreach (var remoteBranch in repo.Branches.Where(x => x.IsRemote).ToArray())
        {
            // make local name from friendly name ('origin/test' to 'test')
            var localBranchName = remoteBranch.FriendlyName.Split('/').Last();
            // if branch with this name exists skip this remote
            if (repo.Branches[localBranchName] != null)
                continue;
            // create local branch by local name
            Branch localBranch = repo.CreateBranch(localBranchName, remoteBranch.Tip);
            // link local branch to remote branch
            repo.Branches.Update(localBranch, b => b.UpstreamBranch = remoteBranch.UpstreamBranchCanonicalName);
        }

        return repo.Branches.Where(x => !x.IsRemote).Select(x => x.FriendlyName);
    }

    /// <summary>
    /// Create directory for user and repository
    /// </summary>
    /// <param name="basePath">Base path</param>
    /// <param name="username">User username</param>
    /// <param name="repoName">User repository name</param>
    /// <returns></returns>
    public static string RecreateUserRepoDir(string basePath, string username, string repoName)
    {
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        if (!Directory.Exists(Path.Combine(basePath, username)))
            Directory.CreateDirectory(Path.Combine(basePath, username));

        var path = Path.Combine(basePath, username, repoName);

        RecreateDirectory(path);

        return path;
    }

    /// <summary>
    /// Create directory by path
    /// If directory exists at first delete her
    /// </summary>
    /// <param name="path"></param>
    private static void RecreateDirectory(string path)
    {
        if (Directory.Exists(path))
            RecursiveDeleteDirectory(path);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Set basic access level to directory and recursively delete her
    /// </summary>
    /// <param name="path">Path to directory</param>
    private static void RecursiveDeleteDirectory(string path)
    {
        var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };
        foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
        {
            info.Attributes = FileAttributes.Normal;
        }

        Directory.Delete(path, true);
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }
}