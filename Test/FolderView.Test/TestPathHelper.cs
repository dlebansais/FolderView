namespace FolderView.Test;

using System;
using NUnit.Framework;

[TestFixture]
public class TestPathHelper
{
    [Test]
    public void TestCombineFolder()
    {
        string Path1, Path2;
        string CombinedPath;

        Path1 = "test";
        Path2 = "test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo( $"{Path1}\\{Path2}"));

        Path1 = "test\\test";
        Path2 = "test\\test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        Path1 = "test/test";
        Path2 = "test/test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}/{Path2}"));

        Path1 = "test\\test";
        Path2 = "test/test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        Path1 = "test/test";
        Path2 = "test\\test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        Path1 = "test";
        Path2 = "test";

        CombinedPath = PathHelper.Combine(Path1 + "/", Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}/{Path2}"));

        CombinedPath = PathHelper.Combine(Path1 + "\\", Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        CombinedPath = PathHelper.Combine(Path1, "/" + Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}/{Path2}"));

        CombinedPath = PathHelper.Combine(Path1, "\\" + Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        CombinedPath = PathHelper.Combine(Path1 + "/", "/" + Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}/{Path2}"));

        CombinedPath = PathHelper.Combine(Path1 + "\\", "\\" + Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        CombinedPath = PathHelper.Combine(Path1 + "/", "\\" + Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        CombinedPath = PathHelper.Combine(Path1 + "\\", "/" + Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        Path1 = "test";
        Path2 = "test/";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}/{Path2}"));

        Path1 = "/test";
        Path2 = "test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}/{Path2}"));

        Path1 = "test";
        Path2 = "test\\";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));

        Path1 = "\\test";
        Path2 = "test";

        CombinedPath = PathHelper.Combine(Path1, Path2);

        Assert.That(CombinedPath, Is.EqualTo($"{Path1}\\{Path2}"));
    }
    [Test]
    public void TestGetFullPath()
    {
        string FullPath;

        FullPath = PathHelper.GetFullPath("test");

        Assert.That(FullPath, Is.EqualTo($"test"));

        FullPath = PathHelper.GetFullPath("test/test");

        Assert.That(FullPath, Is.EqualTo($"test/test"));

        FullPath = PathHelper.GetFullPath("test/../test");

        Assert.That(FullPath, Is.EqualTo($"test"));

        FullPath = PathHelper.GetFullPath("test\\..\\test");

        Assert.That(FullPath, Is.EqualTo($"test"));

        FullPath = PathHelper.GetFullPath("test\\../test");

        Assert.That(FullPath, Is.EqualTo($"test"));

        FullPath = PathHelper.GetFullPath("test1\\../test2\\test3/test4");

        Assert.That(FullPath, Is.EqualTo($"test2\\test3\\test4"));

        FullPath = PathHelper.GetFullPath("test1\\../test2\\test3/../test4/..\\test5");

        Assert.That(FullPath, Is.EqualTo($"test2/test5"));

        FullPath = PathHelper.GetFullPath("test1\\../test2\\test3/../test4/..\\..\\test5\\test6");

        Assert.That(FullPath, Is.EqualTo($"test5\\test6"));

        Assert.Throws<ArgumentException>(() => PathHelper.GetFullPath(".."));
        Assert.Throws<ArgumentException>(() => PathHelper.GetFullPath("test\\..\\.."));
        Assert.Throws<ArgumentException>(() => PathHelper.GetFullPath("\\.."));
        Assert.Throws<ArgumentException>(() => PathHelper.GetFullPath("\\..\\test"));
    }
}
