namespace FolderView;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Octokit;

/// <summary>
/// Provides a view of the root folder in a folder structure.
/// </summary>
internal record RootFolder : Folder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RootFolder"/> class.
    /// </summary>
    /// <param name="location">The path or address to the root.</param>
    public RootFolder(ILocation location)
        : base(null, ValidateLocationOrThrow(location), GetSubfolderList(location), GetFileList(location))
    {
        Folders = ((FolderCollection)Folders).WithParent(this);
        Files = ((FileCollection)Files).WithParent(this);
    }

    private static bool TryParseAsLocal(ILocation location, out string localRoot)
    {
        if (location is LocalLocation AsLocal)
        {
            localRoot = AsLocal.LocalRoot;
            return true;
        }

        localRoot = null!;
        return false;
    }

    private static bool TryParseAsRemote(ILocation location, out GitHubClient client, out GitHubLocation remoteLocation, out IReadOnlyList<RepositoryContent> remoteRoot)
    {
        if (location is GitHubLocation AsRemoteLocation)
        {
            string? AppName = typeof(RootFolder).Assembly.GetName().Name;
            Debug.Assert(AppName is not null);

            client = new GitHubClient(new ProductHeaderValue(AppName));
            var Contents = client.Repository.Content.GetAllContents(AsRemoteLocation.UserName, AsRemoteLocation.RepositoryName, AsRemoteLocation.RemoteRoot);
            Contents.Wait();

            remoteLocation = AsRemoteLocation;
            remoteRoot = Contents.Result;
            return true;
        }

        client = null!;
        remoteLocation = null!;
        remoteRoot = null!;
        return false;
    }

    private static string ValidateLocationOrThrow(ILocation location)
    {
        if (!TryParseAsLocal(location, out _) &&
            !TryParseAsRemote(location, out _, out _, out _))
            throw new ArgumentException(nameof(location));

        return string.Empty;
    }

    private static FolderCollection GetSubfolderList(ILocation location)
    {
        FolderCollection? Result = new();

        if (TryParseAsLocal(location, out string LocalRoot))
            Result = GetSubfolderList(LocalRoot);

        if (TryParseAsRemote(location, out GitHubClient Client, out GitHubLocation RemoteLocation, out IReadOnlyList<RepositoryContent> RemoteRoot))
            Result = GetSubfolderList(Client, RemoteLocation, RemoteRoot);

        Debug.Assert(Result is not null);

        return Result;
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

    private static FolderCollection GetSubfolderList(GitHubClient client, GitHubLocation remoteLocation, IReadOnlyList<RepositoryContent> remoteContent)
    {
        FolderCollection Result = new();

        foreach (var RepositoryContent in remoteContent)
            if (RepositoryContent.Type.TryParse(out ContentType Type) && Type == ContentType.Dir)
            {
                string Name = RepositoryContent.Name;

                var Contents = client.Repository.Content.GetAllContents(remoteLocation.UserName, remoteLocation.RepositoryName, RepositoryContent.Path);
                Contents.Wait();
                IReadOnlyList<RepositoryContent> SubContent = Contents.Result;

                FolderCollection Folders = GetSubfolderList(client, remoteLocation, SubContent);
                FileCollection Files = GetFileList(client, remoteLocation, SubContent);

                Folder NewFolder = new(null, Name, Folders, Files);
                Result.Add(NewFolder);
            }

        return Result;
    }

    private static FileCollection GetFileList(ILocation location)
    {
        FileCollection Result = new();

        if (TryParseAsLocal(location, out string LocalRoot))
            Result = GetFileList(LocalRoot);

        if (TryParseAsRemote(location, out GitHubClient Client, out GitHubLocation RemoteLocation, out IReadOnlyList<RepositoryContent> RemoteRoot))
            Result = GetFileList(Client, RemoteLocation, RemoteRoot);

        Debug.Assert(Result is not null);

        return Result;
    }

    private static FileCollection GetFileList(string localPath)
    {
        FileCollection Result = new();

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
            if (RepositoryContent.Type.TryParse(out ContentType Type) && Type == ContentType.File)
            {
                string Name = RepositoryContent.Name;
                File NewFile = new(null, Name);

                Result.Add(NewFile);
            }

        return Result;
    }
}
