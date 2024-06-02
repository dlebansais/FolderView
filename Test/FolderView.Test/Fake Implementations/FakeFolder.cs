namespace FolderView.Test;

using System;

internal record FakeFolder(IFolder? Parent, string Name, IFolderCollection Folders, IFileCollection Files) : IFolder, ICloneable
{
    public bool IsRoot { get; } = Parent is null;
    public IPath Path { get; } = FolderView.Path.Combine(Parent, Name);

    object ICloneable.Clone()
    {
        return this with { };
    }

    public void Dispose()
    {
    }
}
