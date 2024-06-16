namespace FolderView;

using System;
using System.Diagnostics;

/// <summary>
/// Represents the empty location.
/// </summary>
[DebuggerDisplay("Empty")]
public record EmptyLocation : ILocation
{
    /// <inheritdoc/>
    public string GetAbsolutePath(IPath path)
    {
        throw new NotSupportedException("Paths are not supported for the Empty location.");
    }
}
