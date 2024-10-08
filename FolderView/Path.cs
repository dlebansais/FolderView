﻿namespace FolderView;

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
public partial record Path(IList<string> Ancestors, string Name) : IPath, IEquatable<Path>
{
    /// <summary>
    /// Gets the empty path.
    /// </summary>
    public static Path Empty { get; } = new(new List<string>(), string.Empty);

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
#if NET6_0_OR_GREATER
        int Result = Name.GetHashCode(StringComparison.InvariantCulture);
#else
        int Result = Name.GetHashCode();
#endif

        foreach (string Ancestor in Ancestors)
        {
#if NET6_0_OR_GREATER
            Result ^= Ancestor.GetHashCode(StringComparison.InvariantCulture);
#else
            Result ^= Ancestor.GetHashCode();
#endif
        }

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
    [RequireNotNull(nameof(location))]
    [Require("location.IsValid()")]
    [Ensure("Result.IsNotNull()")]
    private static async Task<IFolder> RootFolderFromAsyncVerified(ILocation location)
    {
        (IFolderCollection Folders, IFileCollection Files) = await RootFolder.TryParseAsync(location).ConfigureAwait(false);
        RootFolder Result = new(location, Folders, Files);

        return Result;
    }

    /// <summary>
    /// Combines a parent folder and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name in the parent's path.</param>
    [Require("parent is null || parent.IsValid()")]
    [RequireNotNull(nameof(name))]
    [Ensure("Result.IsNotNull()")]
    private static IPath CombineVerified(IFolder? parent, string name)
    {
        List<string> ParentNameList = parent is null
            ? new List<string>()
            : parent.Name == string.Empty
                ? new(parent.Path.Ancestors)
                : new(parent.Path.Ancestors) { parent.Name };

        IPath Result = new Path(ParentNameList, name);

        return Result;
    }

    /// <summary>
    /// Combines a parent path and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent path.</param>
    /// <param name="name">The name in the parent path.</param>
    [RequireNotNull(nameof(parent))]
    [Require("parent.IsValid()")]
    [RequireNotNull(nameof(name))]
    [Ensure("Result.IsNotNull()")]
    private static IPath CombineVerified(IPath parent, string name)
    {
        List<string> ParentNameList = new(parent.Ancestors) { parent.Name };
        IPath Result = new Path(ParentNameList, name);

        return Result;
    }

    /// <summary>
    /// Gets the folder starting from a parent and following a path.
    /// </summary>
    /// <param name="parent">The parent folder, <see langword="null"/> for the root folder.</param>
    /// <param name="path">The path.</param>
    [RequireNotNull(nameof(parent))]
    [Require("parent.IsValid()")]
    [RequireNotNull(nameof(path))]
    [Require("path.IsValid()")]
    [Ensure("Result.IsNotNull()")]
    private static IFolder GetRelativeFolderVerified(IFolder parent, IPath path)
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
    [RequireNotNull(nameof(parent))]
    [Require("parent.IsValid()")]
    [RequireNotNull(nameof(path))]
    [Require("path.IsValid()")]
    [Ensure("Result.IsNotNull()")]
    private static IFile GetRelativeFileVerified(IFolder parent, IPath path)
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

    /// <inheritdoc/>
    public string ToPathString(bool fromRoot = false) => ToPathString(System.IO.Path.DirectorySeparatorChar, fromRoot);

    /// <inheritdoc/>
    public string ToPathString(char separator, bool fromRoot = false) => ToPathString(separator.ToString(), fromRoot);

    /// <inheritdoc/>
    public string ToPathString(string separator, bool fromRoot = false) => (fromRoot ? separator : string.Empty) + string.Join(separator, new List<string>(Ancestors) { Name });
}
