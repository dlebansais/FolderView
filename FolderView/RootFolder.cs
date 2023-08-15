namespace FolderView;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Octokit;

/// <summary>
/// Provides a view of the root folder in a folder structure.
/// </summary>
internal record RootFolder : Folder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RootFolder"/> class.
    /// </summary>
    /// <param name="folders">The subfolders.</param>
    /// <param name="files">The files in the root folder.</param>
    public RootFolder(IFolderCollection folders, IFileCollection files)
        : base(null, string.Empty, folders, files)
    {
        Folders = ((FolderCollection)Folders).WithParent(this);
        Files = ((FileCollection)Files).WithParent(this);
    }

    /// <summary>
    /// Enumerates folders and files at the provided location.
    /// </summary>
    /// <param name="location">The location.</param>
    internal static async Task<(IFolderCollection Folders, IFileCollection Files)> TryParseAsync(ILocation location)
    {
        FolderCollection ResultFolders = new();
        FileCollection ResultFiles = new();

        if (location is LocalLocation AsLocal)
        {
            string LocalPath = AsLocal.LocalRoot;

            var Directories = Directory.GetDirectories(LocalPath);

            foreach (var Directory in Directories)
            {
                string Name = System.IO.Path.GetFileName(Directory);
                FolderCollection Folders = GetSubfolderList(Directory);
                FileCollection Files = GetFileList(Directory);

                Folder NewFolder = new(null, Name, Folders, Files);
                ResultFolders.Add(NewFolder);
            }

            var FileNames = Directory.GetFiles(LocalPath);

            foreach (var FileName in FileNames)
            {
                string Name = System.IO.Path.GetFileName(FileName);
                File NewFile = new(null, Name);

                ResultFiles.Add(NewFile);
            }
        }

        if (location is GitHubLocation AsRemoteLocation)
        {
            string? AppName = typeof(RootFolder).Assembly.GetName().Name;
            Debug.Assert(AppName is not null);

            GitHubClient Client = new GitHubClient(new ProductHeaderValue(AppName));
            var Contents = await Client.Repository.Content.GetAllContents(AsRemoteLocation.UserName, AsRemoteLocation.RepositoryName, AsRemoteLocation.RemoteRoot);

            ResultFolders = await GetSubfolderListAsync(Client, AsRemoteLocation, Contents);
            ResultFiles = GetFileList(Client, AsRemoteLocation, Contents);
        }

        return (ResultFolders, ResultFiles);
    }

    private static FolderCollection GetSubfolderList(string localPath)
    {
        FolderCollection Result = new();

        var Directories = Directory.GetDirectories(localPath);

        foreach (var Directory in Directories)
        {
            string Name = System.IO.Path.GetFileName(Directory);
            FolderCollection Folders = GetSubfolderList(Directory);
            FileCollection Files = GetFileList(Directory);

            Folder NewFolder = new(null, Name, Folders, Files);
            Result.Add(NewFolder);
        }

        return Result;
    }

    private static async Task<FolderCollection> GetSubfolderListAsync(GitHubClient client, GitHubLocation remoteLocation, IReadOnlyList<RepositoryContent> remoteContent)
    {
        FolderCollection Result = new();

        foreach (var RepositoryContent in remoteContent)
        {
            bool ParseSuccess = RepositoryContent.Type.TryParse(out ContentType Type);
            Debug.Assert(ParseSuccess);

            if (Type == ContentType.Dir)
            {
                string Name = RepositoryContent.Name;

                var Contents = await client.Repository.Content.GetAllContents(remoteLocation.UserName, remoteLocation.RepositoryName, RepositoryContent.Path);
                IReadOnlyList<RepositoryContent> SubContent = Contents;

                FolderCollection Folders = await GetSubfolderListAsync(client, remoteLocation, SubContent);
                FileCollection Files = GetFileList(client, remoteLocation, SubContent);

                Folder NewFolder = new(null, Name, Folders, Files);
                Result.Add(NewFolder);
            }
        }

        return Result;
    }

    private static FileCollection GetFileList(string localPath)
    {
        FileCollection? Result = new();

        var FileNames = Directory.GetFiles(localPath);

        foreach (var FileName in FileNames)
        {
            string Name = System.IO.Path.GetFileName(FileName);
            File NewFile = new(null, Name);

            Result.Add(NewFile);
        }

        return Result;
    }

    private static FileCollection GetFileList(GitHubClient client, GitHubLocation remoteLocation, IReadOnlyList<RepositoryContent> remoteContent)
    {
        FileCollection Result = new();

        foreach (var RepositoryContent in remoteContent)
        {
            bool ParseSuccess = RepositoryContent.Type.TryParse(out ContentType Type);
            Debug.Assert(ParseSuccess);

            if (Type == ContentType.File)
            {
                string Name = RepositoryContent.Name;
                File NewFile = new(null, Name);

                Result.Add(NewFile);
            }
        }

        return Result;
    }
}
