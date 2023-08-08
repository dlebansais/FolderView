namespace FolderView;

using System.Collections.Generic;

/// <summary>
/// Represents a collection of files.
/// </summary>
internal class FileCollection : List<IFile>, IFileCollection
{
    /// <summary>
    /// Sets the parent for all files in the collection and return a new collection.
    /// </summary>
    /// <param name="parent">The parent folder.</param>
    public FileCollection WithParent(Folder parent)
    {
        FileCollection Result = new();

        foreach (File Item in this)
            Result.Add(Item with { Parent = parent });

        return Result;
    }
}
