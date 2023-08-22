namespace FolderView;

using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Abstraction of a file.
/// </summary>
public interface IFile
{
    /// <summary>
    /// Gets the file's parent folder, <see langword="null"/> in the root folder.
    /// </summary>
    IFolder? Parent { get; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the path to the file.
    /// </summary>
    IPath Path { get; }

    /// <summary>
    /// Gets the file content, null if not loaded.
    /// </summary>
    Stream? Content { get; }

    /// <summary>
    /// Loads the file content.
    /// Since the file could have been deleted between enumeration and this call, <see cref="Content"/> could still be <see langword="null"/> upon return.
    /// </summary>
    Task LoadAsync();
}
