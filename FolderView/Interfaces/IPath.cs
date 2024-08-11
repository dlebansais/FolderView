namespace FolderView;

using System.Collections.Generic;

/// <summary>
/// Abstraction of a path to a file or folder.
/// </summary>
public interface IPath
{
    /// <summary>
    /// Gets the name of ancestors folders.
    /// </summary>
    IList<string> Ancestors { get; }

    /// <summary>
    /// Gets the name of the file or folder.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns a new composed path with the provided name.
    /// </summary>
    /// <param name="name">The name.</param>
    IPath To(string name);

    /// <summary>
    /// Returns a new composed path to the parent.
    /// </summary>
    IPath Up();

    /// <summary>
    /// Gets the path as a string using the default OS separator.
    /// </summary>
    /// <param name="fromRoot"><see langword="true"/> if the result to represent a path from a root folder.</param>
    string ToPathString(bool fromRoot = false);

    /// <summary>
    /// Gets the path as a string.
    /// </summary>
    /// <param name="separator">The separator.</param>
    /// <param name="fromRoot"><see langword="true"/> if the result to represent a path from a root folder.</param>
    string ToPathString(char separator, bool fromRoot = false);

    /// <summary>
    /// Gets the path as a string.
    /// </summary>
    /// <param name="separator">The separator.</param>
    /// <param name="fromRoot"><see langword="true"/> if the result to represent a path from a root folder.</param>
    string ToPathString(string separator, bool fromRoot = false);
}
