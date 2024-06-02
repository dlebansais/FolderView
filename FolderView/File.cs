namespace FolderView;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Contracts;
using Octokit;

/// <summary>
/// Represents a file.
/// </summary>
/// <inheritdoc/>
[DebuggerDisplay("{Name,nq} (path: {((Path)Path).Combined,nq})")]
internal record File(IFolder? Parent, string Name) : IFile
{
    /// <inheritdoc/>
    public IPath Path => FolderView.Path.Combine(Parent, Name);

    /// <inheritdoc/>
    public Stream? Content { get; private set; }

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        IFolder Folder = Contract.AssertNotNull(Parent);
        while (Folder is not RootFolder)
            Folder = Contract.AssertNotNull(Folder.Parent);

        ILocation Location = Contract.AssertNotNull(Folder as RootFolder).Location;

        if (Location is LocalLocation AsLocal)
            LoadLocal(AsLocal);

        if (Location is GitHubLocation AsRemote)
            await LoadRemoteAsync(AsRemote).ConfigureAwait(false);

        Debug.Assert(Content is not null);
    }

    private void LoadLocal(LocalLocation localLocation)
    {
        string AbsolutePath = localLocation.GetAbsolutePath(Path);
        Content = new FileStream(AbsolutePath, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    private async Task LoadRemoteAsync(GitHubLocation remoteLocation)
    {
        string AbsolutePath = remoteLocation.GetAbsolutePath(Path);

        ProductHeaderValue ProductInformation = new(GitHubLocation.AppName);
        GitHubClient Client = new(ProductInformation);
        var Contents = await Client.Repository.Content.GetAllContents(remoteLocation.UserName, remoteLocation.RepositoryName, AbsolutePath).ConfigureAwait(false);

        Debug.Assert(Contents.Count <= 1);

        // If the file has been removed since it was enumerated, the method will leave Content to null.
        foreach (var Item in Contents)
        {
            byte[] DecodedContent = Convert.FromBase64String(Item.EncodedContent);
            Content = new MemoryStream(DecodedContent);
        }
    }

    /// <summary>
    /// Disposes of resources.
    /// </summary>
    public void Dispose()
    {
        if (Content is not null)
        {
            Content.Dispose();
            Content = null;
        }
    }
}
