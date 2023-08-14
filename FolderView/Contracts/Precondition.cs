namespace Contracts;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FolderView;

/// <summary>
/// Provide tools for pre-conditions.
/// </summary>
internal static class Precondition
{
    /// <summary>
    /// Checks whether a reference is null.
    /// </summary>
    /// <typeparam name="T">The reference type. Do not specify it, this should be automatically infered.</typeparam>
    /// <param name="reference">The reference to check.</param>
    /// <param name="callerMethodArgumentName">The name of the caller argument.</param>
    /// <exception cref="System.NullReferenceException">If <paramref name="reference"/> is null.</exception>
    [Conditional("DEBUG")]
    public static void MustBeNotNull<T>(this T? reference, [CallerArgumentExpression("reference")] string callerMethodArgumentName = "")
        where T : class
    {
        if (reference is null) throw new NullReferenceException(callerMethodArgumentName);
    }

    /// <summary>
    /// Checks whether a location is a valid object.
    /// </summary>
    /// <param name="location">The location to check.</param>
    [Conditional("DEBUG")]
    public static void MustBeValid(this ILocation location)
    {
        if (location is LocalLocation AsLocal)
        {
            if (AsLocal.LocalRoot is null) throw new NullReferenceException(nameof(LocalLocation.LocalRoot));
        }
        else if (location is GitHubLocation AsRemoteLocation)
        {
            if (AsRemoteLocation.UserName is null) throw new NullReferenceException(nameof(GitHubLocation.UserName));
            if (AsRemoteLocation.RepositoryName is null) throw new NullReferenceException(nameof(GitHubLocation.RepositoryName));
            if (AsRemoteLocation.RemoteRoot is null) throw new NullReferenceException(nameof(GitHubLocation.RemoteRoot));
        }
        else
        {
            throw new ArgumentException($"Only {typeof(LocalLocation).FullName} and {typeof(GitHubLocation).FullName} are allowed to implement {nameof(ILocation)}");
        }
    }

    /// <summary>
    /// Checks whether a folder is a valid object.
    /// </summary>
    /// <param name="folder">The folder to check.</param>
    [Conditional("DEBUG")]
    public static void MustBeValid(this IFolder folder)
    {
        if (folder is Folder AsFolder)
        {
            if (AsFolder.Name is null) throw new NullReferenceException(nameof(Folder.Name));
            if (AsFolder.Folders is null) throw new NullReferenceException(nameof(Folder.Folders));
            if (AsFolder.Folders is not FolderCollection) throw new ArgumentException($"Only {typeof(FolderCollection).FullName} is allowed to implement {nameof(IFolderCollection)}");
            if (AsFolder.Files is null) throw new NullReferenceException(nameof(Folder.Files));
            if (AsFolder.Files is not FileCollection) throw new ArgumentException($"Only {typeof(FileCollection).FullName} is allowed to implement {nameof(IFileCollection)}");
        }
        else
        {
            throw new ArgumentException($"Only {typeof(Folder).FullName} is allowed to implement {nameof(IFolder)}");
        }
    }

    /// <summary>
    /// Checks whether a path is a valid object.
    /// </summary>
    /// <param name="path">The path to check.</param>
    [Conditional("DEBUG")]
    public static void MustBeValid(this IPath path)
    {
        if (path is Path AsPath)
        {
            if (AsPath.Name is null) throw new NullReferenceException(nameof(Path.Name));
            if (AsPath.Ancestors is null) throw new NullReferenceException(nameof(Path.Ancestors));
        }
        else
        {
            throw new ArgumentException($"Only {typeof(Path).FullName} is allowed to implement {nameof(IPath)}");
        }
    }
}
