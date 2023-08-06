namespace FolderView;

using System.Collections.Generic;

/// <summary>
/// Represents the path from the root to a folder or file.
/// </summary>
/// <param name="Parents">The list of parent folders.</param>
/// <param name="Name">The name of the folder or file.</param>
public record Path(IList<string> Parents, string Name)
{
    /// <summary>
    /// Combines a parent path and a name to return the path to that name.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name in the parent's path.</param>
    public static Path Combine(Folder? parent, string name)
    {
        if (parent is null)
        {
            return new Path(new List<string>(), name);
        }
        else
        {
            List<string> ParentNameList = new(parent.Path.Parents) { parent.Name };

            return new Path(ParentNameList, name);
        }
    }
}
