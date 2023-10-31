namespace FolderView;

using System;

/// <summary>
/// Abstraction of a location.
/// </summary>
public interface ILocation
{
    /// <summary>
    /// Gets the absolute path to the provided relative path starting from this location.
    /// </summary>
    /// <param name="path">The relative path.</param>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
    string GetAbsolutePath(IPath path);
}
