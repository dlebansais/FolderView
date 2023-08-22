namespace FolderView.Test;

internal record FakeFolder(IFolder? Parent, string Name, IFolderCollection Folders, IFileCollection Files) : IFolder
{
    public bool IsRoot { get; } = Parent is null;
    public IPath Path { get; } = FolderView.Path.Combine(Parent, Name);

    public void Dispose()
    {
    }
}
