namespace FolderView;

using System;
using System.Diagnostics;

/// <summary>
/// Represents the empty location.
/// </summary>
[DebuggerDisplay("Empty")]
public record EmptyLocation : ILocation
{
    /// <summary>
    /// Gets the EmptyLocation instance.
    /// </summary>
    public static EmptyLocation Instance { get; } = new EmptyLocation();

    /// <summary>
    /// Gets the empty folder.
    /// </summary>
    public static IFolder Root { get; } = new Folder(null, string.Empty, new FolderCollection(), new FileCollection());

    private EmptyLocation()
    {
    }

    /// <inheritdoc/>
    public string GetAbsolutePath(IPath path)
    {
        throw new NotSupportedException("Paths are not supported for the Empty location.");
    }
}
