namespace FolderView;

using System.Diagnostics;

/// <summary>
/// Represents a location available locally.
/// </summary>
/// <param name="LocalRoot">The local root location.</param>
[DebuggerDisplay("{LocalRoot,nq}")]
public record LocalLocation(string LocalRoot) : ILocation
{
    /// <summary>
    /// Gets the absolute path to the provided relative path starting from this location.
    /// </summary>
    /// <param name="path">The relative path.</param>
    public string GetAbsolutePath(IPath path)
    {
        string Result = LocalRoot;

        foreach (string Name in path.Ancestors)
            Result = System.IO.Path.Combine(Result, Name);

        Result = System.IO.Path.Combine(Result, path.Name);

        return Result;
    }
}
