namespace Contracts;

using System.Diagnostics;

/// <summary>
/// Provide tools for post-conditions.
/// </summary>
internal static class Postcondition
{
    /// <summary>
    /// Ensures a reference is not null.
    /// </summary>
    /// <typeparam name="T">The reference type. Do not specify it, this should be automatically infered.</typeparam>
    /// <param name="reference">The reference.</param>
    [Conditional("DEBUG")]
    public static void EnsureNotNull<T>(this T reference)
        where T : class
    {
        // Nothing to do, the compileur catches null parameters at compile time.
    }
}
