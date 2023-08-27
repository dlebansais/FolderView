namespace FolderView;

using System.Diagnostics;

/// <summary>
/// Represents a location available locally.
/// </summary>
/// <param name="LocalRoot">The local root location.</param>
[DebuggerDisplay("{CanonicalRoot,nq}")]
public record LocalLocation(string LocalRoot) : ILocation
{
    /// <inheritdoc/>
    public string GetAbsolutePath(IPath path)
    {
        string Result = CanonicalRoot;

        foreach (string Name in path.Ancestors)
            Result = PathHelper.Combine(Result, Name);

        Result = PathHelper.Combine(Result, path.Name);
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
                CanonicalRootInternal = PathHelper.GetFullPath(LocalRoot);

            return CanonicalRootInternal;
        }
    }

    private string? CanonicalRootInternal;
}
