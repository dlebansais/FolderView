namespace FolderView;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Octokit;

/// <summary>
/// Provides a view of the root folder in a folder structure.
/// </summary>
[DebuggerDisplay("(root)")]
internal record RootFolder : Folder, IFolder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RootFolder"/> class.
    /// </summary>
    /// <param name="location">The root folder location.</param>
    /// <param name="folders">The subfolders.</param>
    /// <param name="files">The files in the root folder.</param>
    public RootFolder(ILocation location, IFolderCollection folders, IFileCollection files)
        : base(null, string.Empty, folders, files)
    {
        Location = location;
        Folders = ((FolderCollection)Folders).WithParent(this);
        Files = ((FileCollection)Files).WithParent(this);
    }

    /// <summary>
    /// Gets the root folder location.
    /// </summary>
    public ILocation Location { get; }

    /// <summary>
    /// Enumerates folders and files at the provided location.
    /// </summary>
    /// <param name="location">The location.</param>
    internal static async Task<(IFolderCollection Folders, IFileCollection Files)> TryParseAsync(ILocation location)
    {
        FolderCollection ResultFolders = new();
        FileCollection ResultFiles = new();

        if (location is LocalLocation AsLocal)
            FillLocalFoldersAndFiles(AsLocal.LocalRoot, ResultFolders, ResultFiles);
        else if (location is GitHubLocation AsRemoteLocation)
            (ResultFolders, ResultFiles) = await ParseRemoteAsync(AsRemoteLocation).ConfigureAwait(false);

        return (ResultFolders, ResultFiles);
    }

    private static void FillLocalFoldersAndFiles(string localPath, FolderCollection folders, FileCollection files)
    {
        var Directories = Directory.GetDirectories(localPath);

        foreach (var Directory in Directories)
        {
            string Name = System.IO.Path.GetFileName(Directory);
            FolderCollection Folders = GetSubfolderList(Directory);
            FileCollection Files = GetFileList(Directory);

            Folder NewFolder = new(null, Name, Folders, Files);

            folders.Add(NewFolder);
        }

        var FileNames = Directory.GetFiles(localPath);

        foreach (var FileName in FileNames)
        {
            string Name = System.IO.Path.GetFileName(FileName);
            File NewFile = new(null, Name);

            files.Add(NewFile);
        }
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

    private static async Task<(FolderCollection Folders, FileCollection Files)> ParseRemoteAsync(GitHubLocation remoteLocation)
    {
        GitHubClient Client = new(new ProductHeaderValue(GitHubLocation.AppName));
        var Contents = await Client.Repository.Content.GetAllContents(remoteLocation.UserName, remoteLocation.RepositoryName, remoteLocation.RemoteRoot).ConfigureAwait(false);

        FolderCollection ResultFolders = await GetSubfolderListAsync(Client, remoteLocation, Contents).ConfigureAwait(false);
        FileCollection ResultFiles = GetFileList(Contents);

        return (ResultFolders, ResultFiles);
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

                var Contents = await client.Repository.Content.GetAllContents(remoteLocation.UserName, remoteLocation.RepositoryName, RepositoryContent.Path).ConfigureAwait(false);
                IReadOnlyList<RepositoryContent> SubContent = Contents;

                FolderCollection Folders = await GetSubfolderListAsync(client, remoteLocation, SubContent).ConfigureAwait(false);
                FileCollection Files = GetFileList(SubContent);

                Folder NewFolder = new(null, Name, Folders, Files);
                Result.Add(NewFolder);
            }
        }

        return Result;
    }

    private static FileCollection GetFileList(IReadOnlyList<RepositoryContent> remoteContent)
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

    /// <inheritdoc/>
    object ICloneable.Clone()
    {
        return this with { };
    }
}
