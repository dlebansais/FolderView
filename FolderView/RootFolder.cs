namespace FolderView;

using System;
using System.IO;

/// <summary>
/// Provides a view of the root folder in a folder structure.
/// </summary>
internal record RootFolder : Folder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RootFolder"/> class.
    /// </summary>
    /// <param name="rootUri">The path or address to the root.</param>
    public RootFolder(Uri rootUri)
        : base(null, string.Empty, GetSubfolderList(rootUri), GetFileList(rootUri))
    {
        Folders = ((FolderCollection)Folders).WithParent(this);
        Files = ((FileCollection)Files).WithParent(this);
    }

    private static bool TryParseAsLocal(Uri rootUri, out string localRoot)
    {
        if (rootUri.IsFile)
        {
            localRoot = rootUri.LocalPath;
            return true;
        }

        localRoot = null!;
        return false;
    }

    private static bool TryParseAsRemote(Uri rootUri, out string remoteRoot)
    {
        throw new NotImplementedException();
    }

    private static FolderCollection GetSubfolderList(Uri rootUri)
    {
        FolderCollection Result = new();

        if (TryParseAsLocal(rootUri, out string LocalRoot))
            Result = GetSubfolderList(LocalRoot);
        else if (TryParseAsRemote(rootUri, out string RemoteRoot))
            Result = GetSubfolderList(RemoteRoot);

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

    private static FileCollection GetFileList(Uri rootUri)
    {
        FileCollection Result = new();

        if (TryParseAsLocal(rootUri, out string LocalRoot))
            Result = GetFileList(LocalRoot);
        else if (TryParseAsRemote(rootUri, out string RemoteRoot))
            Result = GetFileList(RemoteRoot);

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
}
