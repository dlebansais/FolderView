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
}
