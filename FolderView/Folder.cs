namespace FolderView;

/// <summary>
/// Represents a folder.
/// </summary>
/// <inheritdoc/>
internal record Folder(IFolder? Parent, string Name, IFolderCollection Folders, IFileCollection Files) : IFolder
{
    /// <inheritdoc/>
    public bool IsRoot { get; } = Parent is null;

    /// <inheritdoc/>
    public IPath Path { get; } = FolderView.Path.Combine(Parent, Name);
}
