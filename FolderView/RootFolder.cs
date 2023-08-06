namespace FolderView;

using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Provides a view of the root folder in a folder structure.
/// </summary>
public record RootFolder : Folder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RootFolder"/> class.
    /// </summary>
    /// <param name="rootUri">The path or address to the root.</param>
    public RootFolder(Uri rootUri)
        : base(null, string.Empty, GetSubfolderList(rootUri), GetFileList(rootUri))
    {
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

    private static List<Folder> GetSubfolderList(Uri rootUri)
    {
        List<Folder> Result = new();

        if (TryParseAsLocal(rootUri, out string LocalRoot))
            Result = GetSubfolderList(null, LocalRoot);
        else if (TryParseAsRemote(rootUri, out string RemoteRoot))
            Result = GetSubfolderList(null, RemoteRoot);

        return Result;
    }

    private static List<Folder> GetSubfolderList(Folder? parent, string localPath)
    {
        List<Folder> Result = new();

        var Directories = Directory.GetDirectories(localPath);

        foreach (var Directory in Directories)
        {
            string Name = System.IO.Path.GetFileName(Directory);
            List<Folder> Folders = GetSubfolderList(parent, Directory);
            List<File> Files = GetFileList(parent, Directory);

            Folder NewFolder = new(parent, Name, Folders, Files);
            Result.Add(NewFolder);
        }

        return Result;
    }

    private static List<File> GetFileList(Uri rootUri)
    {
        List<File> Result = new();

        if (TryParseAsLocal(rootUri, out string LocalRoot))
            Result = GetFileList(null, LocalRoot);
        else if (TryParseAsRemote(rootUri, out string RemoteRoot))
            Result = GetFileList(null, RemoteRoot);

        return Result;
    }

    private static List<File> GetFileList(Folder? parent, string localPath)
    {
        List<File> Result = new();

        var FileNames = Directory.GetFiles(localPath);

        foreach (var FileName in FileNames)
        {
            string Name = System.IO.Path.GetFileName(FileName);

            File NewFile = new(parent, Name);
            Result.Add(NewFile);
        }

        return Result;
    }
}
