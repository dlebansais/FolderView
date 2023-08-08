namespace FolderView;

using System.Collections.Generic;

/// <summary>
/// Abstraction of a collection of files.
/// </summary>
public interface IFileCollection : IReadOnlyList<IFile>
{
    /// <summary>
    /// Searches for a file that matches the provided predicate.
    /// </summary>
    /// <param name="match">The predicate.</param>
    IFile? Find(System.Predicate<IFile> match);
}
