namespace FolderView;

/// <summary>
/// Represents a file.
/// </summary>
/// <param name="Parent">The parent folder.</param>
/// <param name="Name">The file name.</param>
public record File(Folder? Parent, string Name)
{
    /// <summary>
    /// Gets the path to the file.
    /// </summary>
    public Path Path { get; } = Path.Combine(Parent, Name);
}
