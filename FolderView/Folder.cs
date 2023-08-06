namespace FolderView;

/// <summary>
/// Represents a folder.
/// </summary>
/// <param name="Parent">The parent folder, or <see langword="null"/> for a root folder.</param>
/// <param name="Name">The folder name.</param>
/// <param name="Folders">The list of folders in this folder.</param>
/// <param name="Files">The list of files in this folder.</param>
public record Folder(Folder? Parent, string Name, FolderCollection Folders, FileCollection Files)
{
    /// <summary>
    /// Gets a value indicating whether this folder is the root folder.
    /// </summary>
    public bool IsRoot { get; } = Parent is null;

    /// <summary>
    /// Gets the path to the folder.
    /// </summary>
    public Path Path { get; } = Path.Combine(Parent, Name);
}
