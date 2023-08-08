namespace FolderView;

/// <summary>
/// Represents a location available locally.
/// </summary>
/// <param name="LocalRoot">The local root location.</param>
public record LocalLocation(string LocalRoot) : ILocation
{
}
