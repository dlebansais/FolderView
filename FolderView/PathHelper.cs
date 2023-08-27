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
    public static string Combine(string path1, string path2)
    {
        int SlashSeparatorCount = CountCharacter(path1, SlashSeparator) + CountCharacter(path2, SlashSeparator);
        int BackslashSeparatorCount = CountCharacter(path1, BackslashSeparator) + CountCharacter(path2, BackslashSeparator);
        char PreferredSeparator = SlashSeparatorCount > BackslashSeparatorCount ? SlashSeparator : BackslashSeparator;

        bool Path1EndsWithSeparator = false;
        Path1EndsWithSeparator |= path1.EndsWith(SlashSeparator);
        Path1EndsWithSeparator |= path1.EndsWith(BackslashSeparator);

        string TrimmedPath1 = Path1EndsWithSeparator ? path1.Substring(0, path1.Length - 1) : path1;

        bool Path2StartsWithSeparator = false;
        Path2StartsWithSeparator |= path2.StartsWith(SlashSeparator);
        Path2StartsWithSeparator |= path2.StartsWith(BackslashSeparator);

        string TrimmedPath2 = Path2StartsWithSeparator ? path2.Substring(1) : path2;

        return TrimmedPath1 + PreferredSeparator + TrimmedPath2;
    }

    /// <summary>
    /// Returns the absolute path for the specified path string.
    /// </summary>
    /// <param name="path">The path.</param>
    public static string GetFullPath(string path)
    {
        int SlashSeparatorCount = CountCharacter(path, SlashSeparator);
        int BackslashSeparatorCount = CountCharacter(path, BackslashSeparator);
        char PreferredSeparator = SlashSeparatorCount > BackslashSeparatorCount ? SlashSeparator : BackslashSeparator;

        string PathWithNormalizedSeparator = path.Replace(SlashSeparator, PreferredSeparator).Replace(BackslashSeparator, PreferredSeparator);
        bool PathStartsWithSeparator = PathWithNormalizedSeparator.StartsWith(PreferredSeparator);

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
                throw new ArgumentException(nameof(path));

        string Result = string.Join(PreferredSeparator, CombinedPaths);

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
