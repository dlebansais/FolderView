namespace FolderView;

/// <summary>
/// Represents a file.
/// </summary>
/// <param name="Parent">The parent folder.</param>
/// <param name="Name">The file name.</param>
internal record File(IFolder? Parent, string Name) : IFile
{
    /// <summary>
    /// Gets the path to the file.
    /// </summary>
    public IPath Path { get; } = FolderView.Path.Combine(Parent, Name);
}
