namespace FolderView;

using System;
using System.Collections.Generic;

/// <summary>
/// Provides tools to manipulate paths.
/// </summary>
public static class PathHelper
{
    private const char SlashSeparator = '/';
    private const char BackslashSeparator = '\\';

    /// <summary>
    /// Combines strings into a path.
    /// </summary>
    /// <param name="path1">The base path.</param>
    /// <param name="path2">The relative path.</param>
    /// <exception cref="ArgumentNullException"><paramref name="path1"/> or <paramref name="path2"/> is null.</exception>
    public static string Combine(string path1, string path2)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(path1);
        ArgumentNullException.ThrowIfNull(path2);
#else
        if (path1 is null)
            throw new ArgumentNullException(nameof(path1));
        if (path2 is null)
            throw new ArgumentNullException(nameof(path2));
#endif

        int SlashSeparatorCount = CountCharacter(path1, SlashSeparator) + CountCharacter(path2, SlashSeparator);
        int BackslashSeparatorCount = CountCharacter(path1, BackslashSeparator) + CountCharacter(path2, BackslashSeparator);
        char PreferredSeparator = SlashSeparatorCount > BackslashSeparatorCount ? SlashSeparator : BackslashSeparator;

        bool Path1EndsWithSeparator = false;
#if NET6_0_OR_GREATER
        Path1EndsWithSeparator |= path1.EndsWith(SlashSeparator);
        Path1EndsWithSeparator |= path1.EndsWith(BackslashSeparator);
#else
        Path1EndsWithSeparator |= path1.EndsWith($"{SlashSeparator}", StringComparison.Ordinal);
        Path1EndsWithSeparator |= path1.EndsWith($"{BackslashSeparator}", StringComparison.Ordinal);
#endif

        string TrimmedPath1 = Path1EndsWithSeparator ? path1.Substring(0, path1.Length - 1) : path1;

        bool Path2StartsWithSeparator = false;
#if NET6_0_OR_GREATER
        Path2StartsWithSeparator |= path2.StartsWith(SlashSeparator);
        Path2StartsWithSeparator |= path2.StartsWith(BackslashSeparator);
#else
        Path2StartsWithSeparator |= path2.StartsWith($"{SlashSeparator}", StringComparison.Ordinal);
        Path2StartsWithSeparator |= path2.StartsWith($"{BackslashSeparator}", StringComparison.Ordinal);
#endif

        string TrimmedPath2 = Path2StartsWithSeparator ? path2.Substring(1) : path2;

        return TrimmedPath1 + PreferredSeparator + TrimmedPath2;
    }

    /// <summary>
    /// Returns the absolute path for the specified path string.
    /// </summary>
    /// <param name="path">The path.</param>
    public static string GetFullPath(string path)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(path);
#else
        if (path is null)
            throw new ArgumentNullException(nameof(path));
#endif

        int SlashSeparatorCount = CountCharacter(path, SlashSeparator);
        int BackslashSeparatorCount = CountCharacter(path, BackslashSeparator);
        char PreferredSeparator = SlashSeparatorCount > BackslashSeparatorCount ? SlashSeparator : BackslashSeparator;

        string PathWithNormalizedSeparator = path.Replace(SlashSeparator, PreferredSeparator).Replace(BackslashSeparator, PreferredSeparator);

#if NET6_0_OR_GREATER
        bool PathStartsWithSeparator = PathWithNormalizedSeparator.StartsWith(PreferredSeparator);
#else
        bool PathStartsWithSeparator = PathWithNormalizedSeparator.StartsWith($"{PreferredSeparator}", StringComparison.Ordinal);
#endif

        if (PathStartsWithSeparator)
            PathWithNormalizedSeparator = PathWithNormalizedSeparator.Substring(1);

        string[] SplittedPath = PathWithNormalizedSeparator.Split(PreferredSeparator);

        List<string> CombinedPaths = new();

        for (int Index = 0; Index < SplittedPath.Length; Index++)
            if (SplittedPath[Index] != Path.Ancestor)
                CombinedPaths.Add(SplittedPath[Index]);
            else if (CombinedPaths.Count > 0)
                CombinedPaths.RemoveAt(CombinedPaths.Count - 1);
            else
                throw new ArgumentException(null, nameof(path));

#if NET6_0_OR_GREATER
        string Result = string.Join(PreferredSeparator, CombinedPaths);
#else
        string Result = string.Join($"{PreferredSeparator}", CombinedPaths);
#endif

        if (PathStartsWithSeparator)
            Result = PreferredSeparator + Result;

        return Result;
    }

    private static int CountCharacter(string s, char c)
    {
        int Count = 0;

        foreach (char Character in s)
            if (Character == c)
                Count++;

        return Count;
    }
}
