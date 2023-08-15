namespace FolderView;

using System.Diagnostics;

/// <summary>
/// Represents a location on github.com.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="RepositoryName">The repositiry name.</param>
/// <param name="RemoteRoot">The remote root location as folder names separated by the slash character, or the empty string for the repository root.</param>
[DebuggerDisplay("{UserName,nq}/{RepositoryName,nq}, /{RemoteRoot,nq}")]
public record GitHubLocation(string UserName, string RepositoryName, string RemoteRoot) : ILocation
{
    /// <summary>
    /// Gets the application name.
    /// </summary>
    public static string? AppName => typeof(GitHubLocation).Assembly.GetName().Name;

    /// <summary>
    /// Gets the absolute path to the provided relative path starting from this location.
    /// </summary>
    /// <param name="path">The relative path.</param>
    public string GetAbsolutePath(IPath path)
    {
        string Result = RemoteRoot + ((Path)path).Combined;

        return Result;
    }
}
