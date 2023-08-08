namespace FolderView;

/// <summary>
/// Abstraction of a folder.
/// </summary>
public interface IFolder
{
    /// <summary>
    /// Gets the folder's parent folder, <see langword="null"/> in the root folder.
    /// </summary>
    IFolder? Parent { get; }

    /// <summary>
    /// Gets the folder name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the folder subfolders.
    /// </summary>
    IFolderCollection Folders { get; }

    /// <summary>
    /// Gets the folder files.
    /// </summary>
    IFileCollection Files { get; }

    /// <summary>
    /// Gets a value indicating whether this folder is the root folder.
    /// </summary>
    bool IsRoot { get; }

    /// <summary>
    /// Gets the path to the folder.
    /// </summary>
    IPath Path { get; }
}
