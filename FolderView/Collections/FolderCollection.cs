namespace FolderView;

using System.Collections.Generic;

/// <summary>
/// Represents a collection of folders.
/// </summary>
internal class FolderCollection : List<IFolder>, IFolderCollection
{
    /// <summary>
    /// Sets the parent for all folders in the collection and return a new collection.
    /// </summary>
    /// <param name="parent">The parent folder.</param>
    public FolderCollection WithParent(Folder parent)
    {
        FolderCollection Result = new();

        foreach (Folder Item in this)
        {
            Folder ModifiedItem = Item with { Parent = parent };
            ModifiedItem = ModifiedItem with { Folders = ((FolderCollection)ModifiedItem.Folders).WithParent(ModifiedItem), Files = ((FileCollection)ModifiedItem.Files).WithParent(ModifiedItem) };

            Result.Add(ModifiedItem);
        }

        return Result;
    }
}
