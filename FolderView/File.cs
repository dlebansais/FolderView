namespace FolderView;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Octokit;

/// <summary>
/// Represents a file.
/// </summary>
/// <inheritdoc/>
internal record File(IFolder? Parent, string Name) : IFile
{
    /// <inheritdoc/>
    public IPath Path { get; } = FolderView.Path.Combine(Parent, Name);

    /// <inheritdoc/>
    public byte[]? Content { get; private set; }

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        IFolder? RootParent = Parent;
        while (RootParent is not null && RootParent is not FolderView.RootFolder)
            RootParent = RootParent.Parent;

        Debug.Assert(RootParent is RootFolder);
        ILocation Location = ((RootFolder)RootParent).Location;

        if (Location is LocalLocation AsLocal)
            LoadLocal(AsLocal);

        if (Location is GitHubLocation AsRemote)
            await LoadRemoteAsync(AsRemote);

        Debug.Assert(Content is not null);
    }

    private void LoadLocal(LocalLocation localLocation)
    {
        string AbsolutePath = localLocation.GetAbsolutePath(Path);
        using FileStream Stream = new(AbsolutePath, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
        using BinaryReader Reader = new(Stream);

        Content = Reader.ReadBytes((int)Stream.Length);
    }

    private async Task LoadRemoteAsync(GitHubLocation remoteLocation)
    {
        string AbsolutePath = remoteLocation.GetAbsolutePath(Path);

        GitHubClient Client = new GitHubClient(new ProductHeaderValue(GitHubLocation.AppName));
        var Contents = await Client.Repository.Content.GetAllContents(remoteLocation.UserName, remoteLocation.RepositoryName, AbsolutePath);

        Debug.Assert(Contents.Count <= 1);

        // If the file has been removed since it was enumerated, the method will leave Content to null.
        foreach (var Item in Contents)
            Content = Convert.FromBase64String(Item.EncodedContent);
    }
}
