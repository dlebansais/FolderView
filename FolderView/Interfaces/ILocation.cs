namespace FolderView;

/// <summary>
/// Abstraction of a location.
/// </summary>
public interface ILocation
{
    /// <summary>
    /// Gets the absolute path to the provided relative path starting from this location.
    /// </summary>
    /// <param name="path">The relative path.</param>
    string GetAbsolutePath(IPath path);
}
