namespace FolderView;

/// <summary>
/// Represents a location on github.com.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="RepositoryName">The repositiry name.</param>
/// <param name="RemoteRoot">The remote root location as folder names separated by the slash character, or the empty string for the repository root.</param>
public record GitHubLocation(string UserName, string RepositoryName, string RemoteRoot) : ILocation
{
}
