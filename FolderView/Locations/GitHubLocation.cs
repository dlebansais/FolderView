namespace FolderView;

using System;
using System.Diagnostics;

/// <summary>
/// Represents a location on github.com.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="RepositoryName">The repositiry name.</param>
/// <param name="RemoteRoot">The remote root location as folder names separated by the slash character, or the empty string for the repository root.</param>
[DebuggerDisplay("{UserName,nq}/{RepositoryName,nq}, /{CanonicalRoot,nq}")]
public record GitHubLocation(string UserName, string RepositoryName, string RemoteRoot) : ILocation
{
    /// <summary>
    /// Gets the application name.
    /// </summary>
    public static string? AppName => typeof(GitHubLocation).Assembly.GetName().Name;

    /// <inheritdoc/>
    public string GetAbsolutePath(IPath path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        string Result;

        Result = PathHelper.Combine(CanonicalRoot, ((Path)path).Combined);
        Result = PathHelper.GetFullPath(Result);

        return Result;
    }

    /// <summary>
    /// Gets the canonical root.
    /// </summary>
    public string CanonicalRoot
    {
        get
        {
            if (CanonicalRootInternal is null)
                CanonicalRootInternal = PathHelper.GetFullPath(RemoteRoot);

            return CanonicalRootInternal;
        }
    }

    private string? CanonicalRootInternal;
}
