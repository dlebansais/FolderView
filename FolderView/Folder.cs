namespace FolderView;

using System.Diagnostics;

/// <summary>
/// Represents a folder.
/// </summary>
/// <inheritdoc/>
[DebuggerDisplay("{Name,nq} (path: {((Path)Path).Combined,nq})")]
internal record Folder(IFolder? Parent, string Name, IFolderCollection Folders, IFileCollection Files) : IFolder
{
    /// <inheritdoc/>
    public bool IsRoot => Parent is null;

    /// <inheritdoc/>
    public IPath Path => FolderView.Path.Combine(Parent, Name);
}
