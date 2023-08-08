namespace FolderView;

using System;
using System.Collections.Generic;
using NotFoundException = System.IO.FileNotFoundException;

/// <summary>
/// Represents the path from the root to a folder or file.
/// </summary>
/// <param name="Ancestors">The list of ancestor folders.</param>
/// <param name="Name">The name of the folder or file.</param>
public record Path(IList<string> Ancestors, string Name) : IPath
{
    /// <summary>
    /// Defines the ancestor string in path.
    /// </summary>
    public const string Ancestor = "..";

    /// <summary>
    /// Gets a root folder from a local path or remote address.
    /// </summary>
    /// <param name="uri">The path or address to the root.</param>
    public static IFolder RootFolderFrom(Uri uri)
    {
        return new RootFolder(uri);
    }

    /// <summary>
    /// Combines a parent path and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name in the parent's path.</param>
    public static IPath Combine(IFolder? parent, string name)
    {
        if (parent is null)
        {
            return new Path(new List<string>(), name);
        }
        else
        {
            List<string> ParentNameList = new(parent.Path.Ancestors) { parent.Name };

            return new Path(ParentNameList, name);
        }
    }

    /// <summary>
    /// Gets the folder starting from a parent and following a path.
    /// </summary>
    /// <param name="parent">The parent folder, <see langword="null"/> for the root folder.</param>
    /// <param name="path">The path.</param>
    public static IFolder GetRelativeFolder(IFolder parent, Path path)
    {
        IFolder AncestorFolder = NavigateAncestors(parent, path);
        IFolder Result = AncestorFolder.Folders.Find(item => item.Name == path.Name) ?? throw new NotFoundException();

        return Result;
    }

    /// <summary>
    /// Gets the folder starting from a parent and following a path.
    /// </summary>
    /// <param name="parent">The parent folder, <see langword="null"/> for the root folder.</param>
    /// <param name="path">The path.</param>
    public static IFile GetRelativeFile(IFolder parent, IPath path)
    {
        IFolder AncestorFolder = NavigateAncestors(parent, path);
        IFile Result = AncestorFolder.Files.Find(item => item.Name == path.Name) ?? throw new NotFoundException();

        return Result;
    }

    private static IFolder NavigateAncestors(IFolder parent, IPath path)
    {
        IFolder CurrentFolder = parent;

        foreach (string Name in path.Ancestors)
            CurrentFolder = Name == Ancestor
                ? CurrentFolder.Parent ?? throw new NotFoundException()
                : CurrentFolder.Folders.Find(item => item.Name == Name) ?? throw new NotFoundException();

        return CurrentFolder;
    }
}
