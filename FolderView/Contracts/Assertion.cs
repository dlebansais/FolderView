namespace Contracts;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FolderView;

/// <summary>
/// Provide tools for pre-conditions.
/// </summary>
internal static class Assertion
{
    /// <summary>
    /// Checks whether a location is a valid object.
    /// </summary>
    /// <param name="location">The location to check.</param>
    public static bool IsValid(this ILocation location)
    {
        return IsLocationValid(location, out _, out _);
    }

    /// <summary>
    /// Checks whether a location is a valid object.
    /// </summary>
    /// <param name="location">The location to check.</param>
    /// <param name="message">The message if the location is not valid and <paramref name="propertyName"/> is null upon return.</param>
    /// <param name="propertyName">The property name if the location is not valid and <paramref name="message"/> is null upon return.</param>
    /// <returns><see langword="true"/> if the location is valid; Otherwise, <see langword="false"/>.</returns>
    private static bool IsLocationValid(ILocation location, out string? message, out string? propertyName)
    {
        message = null;
        propertyName = null;

        if (location is LocalLocation AsLocal)
        {
            if (AsLocal.LocalRoot is null)
            {
                propertyName = nameof(LocalLocation.LocalRoot);
                return false;
            }
        }
        else if (location is GitHubLocation AsRemoteLocation)
        {
            if (AsRemoteLocation.UserName is null)
            {
                propertyName = nameof(GitHubLocation.UserName);
                return false;
            }

            if (AsRemoteLocation.RepositoryName is null)
            {
                propertyName = nameof(GitHubLocation.RepositoryName);
                return false;
            }

            if (AsRemoteLocation.RemoteRoot is null)
            {
                propertyName = nameof(GitHubLocation.RemoteRoot);
                return false;
            }
        }
        else
        {
            message = $"Only {typeof(LocalLocation).FullName} and {typeof(GitHubLocation).FullName} are allowed to implement {nameof(ILocation)}";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks whether a folder is a valid object.
    /// </summary>
    /// <param name="folder">The folder to check.</param>
    public static bool IsValid(this IFolder folder)
    {
        return IsFolderValid(folder, out _, out _);
    }

    /// <summary>
    /// Checks whether a folder is a valid object.
    /// </summary>
    /// <param name="folder">The folder to check.</param>
    /// <param name="message">The message if the folder is not valid and <paramref name="propertyName"/> is null upon return.</param>
    /// <param name="propertyName">The property name if the folder is not valid and <paramref name="message"/> is null upon return.</param>
    /// <returns><see langword="true"/> if the folder is valid; Otherwise, <see langword="false"/>.</returns>
    private static bool IsFolderValid(IFolder folder, out string? message, out string? propertyName)
    {
        message = null;
        propertyName = null;

        if (folder is Folder AsFolder)
        {
            if (AsFolder.Name is null)
            {
                propertyName = nameof(Folder.Name);
                return false;
            }

            if (AsFolder.Folders is null)
            {
                propertyName = nameof(Folder.Folders);
                return false;
            }

            if (AsFolder.Folders is not FolderCollection)
            {
                message = $"Only {typeof(FolderCollection).FullName} is allowed to implement {nameof(IFolderCollection)}";
                return false;
            }

            if (AsFolder.Files is null)
            {
                propertyName = nameof(Folder.Files);
                return false;
            }

            if (AsFolder.Files is not FileCollection)
            {
                message = $"Only {typeof(FileCollection).FullName} is allowed to implement {nameof(IFileCollection)}";
                return false;
            }
        }
        else
        {
            message = $"Only {typeof(Folder).FullName} is allowed to implement {nameof(IFolder)}";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks whether a path is a valid object.
    /// </summary>
    /// <param name="path">The path to check.</param>
    public static bool IsValid(this IPath path)
    {
        return IsPathValid(path, out _, out _);
    }

    /// <summary>
    /// Checks whether a path is a valid object.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="message">The message if the path is not valid and <paramref name="propertyName"/> is null upon return.</param>
    /// <param name="propertyName">The property name if the path is not valid and <paramref name="message"/> is null upon return.</param>
    /// <returns><see langword="true"/> if the path is valid; Otherwise, <see langword="false"/>.</returns>
    private static bool IsPathValid(IPath path, out string? message, out string? propertyName)
    {
        message = null;
        propertyName = null;

        if (path is Path AsPath)
        {
            if (AsPath.Name is null)
            {
                propertyName = nameof(Path.Name);
                return false;
            }

            if (AsPath.Ancestors is null)
            {
                propertyName = nameof(Path.Ancestors);
                return false;
            }
        }
        else
        {
            message = $"Only {typeof(Path).FullName} is allowed to implement {nameof(IPath)}";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks whether a reference is not null.
    /// </summary>
    /// <typeparam name="T">The reference type. Do not specify it, this should be automatically infered.</typeparam>
    /// <param name="reference">The reference.</param>
    public static bool IsNotNull<T>(this T? reference)
        where T : class
    {
        return reference is not null;
    }
}
