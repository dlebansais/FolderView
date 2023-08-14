namespace FolderView;

using System.Collections.Generic;
using Contracts;
using NotFoundException = System.IO.FileNotFoundException;

/// <summary>
/// Represents the path from the root to a folder or file.
/// </summary>
/// <inheritdoc/>
public record Path(IList<string> Ancestors, string Name) : IPath
{
    /// <summary>
    /// Defines the ancestor string in path.
    /// </summary>
    public const string Ancestor = "..";

    /// <summary>
    /// Gets a root folder from a local path or remote address.
    /// </summary>
    /// <param name="location">The location of the root.</param>
    public static IFolder RootFolderFrom(ILocation location)
    {
        location.MustBeNotNull();
        location.MustBeValid();

        IFolder Result = new RootFolder(location);

        Result.EnsureNotNull();
        return Result;
    }

    /// <summary>
    /// Combines a parent path and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name in the parent's path.</param>
    public static IPath Combine(IFolder? parent, string name)
    {
        if (parent is not null)
            parent.MustBeValid();
        name.MustBeNotNull();

        IPath Result;

        if (parent is null)
        {
            Result = new Path(new List<string>(), name);
        }
        else
        {
            List<string> ParentNameList = new(parent.Path.Ancestors) { parent.Name };

            Result = new Path(ParentNameList, name);
        }

        Result.EnsureNotNull();
        return Result;
    }

    /// <summary>
    /// Gets the folder starting from a parent and following a path.
    /// </summary>
    /// <param name="parent">The parent folder, <see langword="null"/> for the root folder.</param>
    /// <param name="path">The path.</param>
    public static IFolder GetRelativeFolder(IFolder parent, IPath path)
    {
        parent.MustBeNotNull();
        parent.MustBeValid();
        path.MustBeNotNull();
        path.MustBeValid();

        IFolder AncestorFolder = NavigateAncestors(parent, path);
        IFolder Result = AncestorFolder.Folders.Find(item => item.Name == path.Name) ?? throw new NotFoundException();

        Result.EnsureNotNull();
        return Result;
    }

    /// <summary>
    /// Gets the folder starting from a parent and following a path.
    /// </summary>
    /// <param name="parent">The parent folder, <see langword="null"/> for the root folder.</param>
    /// <param name="path">The path.</param>
    public static IFile GetRelativeFile(IFolder parent, IPath path)
    {
        parent.MustBeNotNull();
        parent.MustBeValid();
        path.MustBeNotNull();
        path.MustBeValid();

        IFolder AncestorFolder = NavigateAncestors(parent, path);
        IFile Result = AncestorFolder.Files.Find(item => item.Name == path.Name) ?? throw new NotFoundException();

        Result.EnsureNotNull();
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
