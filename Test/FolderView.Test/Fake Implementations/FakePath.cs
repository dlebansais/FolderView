namespace FolderView;

using System.Collections.Generic;

public record FakePath(IList<string> Ancestors, string Name) : IPath
{
}
