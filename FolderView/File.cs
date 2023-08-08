namespace FolderView;

/// <summary>
/// Represents a file.
/// </summary>
/// <inheritdoc/>
internal record File(IFolder? Parent, string Name) : IFile
{
    /// <inheritdoc/>
    public IPath Path { get; } = FolderView.Path.Combine(Parent, Name);
}
