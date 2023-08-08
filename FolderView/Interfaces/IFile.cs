namespace FolderView;

/// <summary>
/// Abstraction of a file.
/// </summary>
public interface IFile
{
    /// <summary>
    /// Gets the file's parent folder, <see langword="null"/> in the root folder.
    /// </summary>
    IFolder? Parent { get; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the path to the file.
    /// </summary>
    IPath Path { get; }
}
