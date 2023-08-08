namespace FolderView;

using System.Collections.Generic;

/// <summary>
/// Abstraction of a collection of folders.
/// </summary>
public interface IFolderCollection : IReadOnlyList<IFolder>
{
    /// <summary>
    /// Searches for a folder that matches the provided predicate.
    /// </summary>
    /// <param name="match">The predicate.</param>
    IFolder? Find(System.Predicate<IFolder> match);
}
