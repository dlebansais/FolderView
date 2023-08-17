namespace FolderView;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using NotFoundException = System.IO.FileNotFoundException;

/// <summary>
/// Represents the path from the root to a folder or file.
/// </summary>
/// <inheritdoc/>
[DebuggerDisplay("{Combined,nq}")]
public record Path(IList<string> Ancestors, string Name) : IPath, IEquatable<Path>
{
    /// <summary>
    /// Defines the ancestor string in path.
    /// </summary>
    public const string Ancestor = "..";

    /// <summary>
    /// Gets a combined string from <see cref="Ancestors"/>.
    /// </summary>
    internal string Combined
    {
        get
        {
            if (Ancestors is null)
                return string.Empty;

            string Result = string.Empty;

            foreach (string Ancestor in Ancestors)
                Result += $"/{Ancestor}";

            Result += $"/{Name}";

            return Result;
        }
    }

    /// <inheritdoc/>
    public virtual bool Equals(Path? other)
    {
        return other is not null &&
               Ancestors.SequenceEqual(other.Ancestors) &&
               Name == other.Name;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int Result = Name.GetHashCode();

        foreach (string Ancestor in Ancestors)
            Result ^= Ancestor.GetHashCode();

        return Result;
    }

    /// <inheritdoc/>
    public IPath To(string name) => Combine(this, name);

    /// <inheritdoc/>
    public IPath Up()
    {
        if (Ancestors.Count == 0)
            throw new InvalidOperationException();

        List<string> ParentNameList = new(Ancestors);
        int LastAncestorIndex = ParentNameList.Count - 1;
        string LastAncestor = ParentNameList[LastAncestorIndex];

        ParentNameList.RemoveAt(LastAncestorIndex);
        IPath Result = new Path(ParentNameList, LastAncestor);

        return Result;
    }

    /// <summary>
    /// Gets a root folder from a local path or remote address.
    /// </summary>
    /// <param name="location">The location of the root.</param>
    public static async Task<IFolder> RootFolderFromAsync(ILocation location)
    {
        location.MustBeNotNull();
        location.MustBeValid();

        (IFolderCollection Folders, IFileCollection Files) = await RootFolder.TryParseAsync(location);
        RootFolder Result = new RootFolder(location, Folders, Files);

        Result.EnsureNotNull();
        return Result;
    }

    /// <summary>
    /// Combines a parent folder and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name in the parent's path.</param>
    public static IPath Combine(IFolder? parent, string name)
    {
        if (parent is not null)
            parent.MustBeValid();
        name.MustBeNotNull();

        List<string> ParentNameList = parent is null
            ? new List<string>()
            : parent.Name == string.Empty
                ? new(parent.Path.Ancestors)
                : new(parent.Path.Ancestors) { parent.Name };

        IPath Result = new Path(ParentNameList, name);

        Result.EnsureNotNull();
        return Result;
    }

    /// <summary>
    /// Combines a parent path and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent path.</param>
    /// <param name="name">The name in the parent path.</param>
    public static IPath Combine(IPath parent, string name)
    {
        parent.MustBeNotNull();
        parent.MustBeValid();
        name.MustBeNotNull();

        List<string> ParentNameList = new(parent.Ancestors) { parent.Name };
        IPath Result = new Path(ParentNameList, name);

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
