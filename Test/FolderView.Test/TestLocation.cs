namespace FolderView.Test;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestLocation
{
    [Test]
    public void TestLocalLocationNullPath()
    {
        LocalLocation Location = (LocalLocation)RootFolderStructure.GetRootAsLocalLocation();

        _ = Assert.Throws<ArgumentNullException>(() => Location.GetAbsolutePath(null!));
    }

    [Test]
    public void TestRemoteLocationNullPath()
    {
        GitHubLocation Location = (GitHubLocation)RootFolderStructure.GetRootAsRemoteLocation();

        _ = Assert.Throws<ArgumentNullException>(() => Location.GetAbsolutePath(null!));
    }

    [Test]
    public void TestLocalAbsolutePath()
    {
        LocalLocation Location = (LocalLocation)RootFolderStructure.GetRootAsLocalLocation();
        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);

        string AbsolutePath = Location.GetAbsolutePath(FirstLevelFolderPath);
        string? AbsolutePathFolder = System.IO.Path.GetDirectoryName(AbsolutePath);

        Assert.That(AbsolutePathFolder, Is.EqualTo(Location.CanonicalRoot));
    }

    [Test]
    public void TestRemoteAbsolutePath()
    {
        GitHubLocation Location = (GitHubLocation)RootFolderStructure.GetRootAsRemoteLocation();
        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);

        string AbsolutePath = Location.GetAbsolutePath(FirstLevelFolderPath);
        string? AbsolutePathFolder = System.IO.Path.GetDirectoryName(AbsolutePath);

#if NET6_0_OR_GREATER
        AbsolutePathFolder = AbsolutePathFolder?.Replace("\\", "/", StringComparison.OrdinalIgnoreCase);
#else
        AbsolutePathFolder = AbsolutePathFolder?.Replace("\\", "/");
#endif

        Assert.That(AbsolutePathFolder, Is.EqualTo(Location.CanonicalRoot));
    }

    [DebugOnly]
    [Test]
    public void TestFake()
    {
        FakeLocation Location = new();
        Exception Exception = Assert.ThrowsAsync<ArgumentException>(async () => await Path.RootFolderFromAsync(Location).ConfigureAwait(false));

        Assert.That(Exception.Message, Does.Contain(nameof(ILocation)));

        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);

        _ = Assert.Throws<NotImplementedException>(() => Location.GetAbsolutePath(FirstLevelFolderPath));
    }

    [DebugOnly]
    [Test]
    public void TestNull()
    {
        Exception Exception;
        string PropertyName;

        PropertyName = "LocalRoot";
        var LocationInvalidLocalRoot = RootFolderStructure.GetRootAsLocalLocation();
        LocationInvalidLocalRoot.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidLocalRoot, null!);
        Exception = Assert.ThrowsAsync<ArgumentException>(async () => await Path.RootFolderFromAsync(LocationInvalidLocalRoot).ConfigureAwait(false));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

#if ENABLE_REMOTE
        PropertyName = "UserName";
        var LocationInvalidUserName = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidUserName.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidUserName, null!);
        Exception = Assert.ThrowsAsync<ArgumentException>(async () => await Path.RootFolderFromAsync(LocationInvalidUserName).ConfigureAwait(false));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "RepositoryName";
        var LocationInvalidRepositoryName = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidRepositoryName.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidRepositoryName, null!);
        Exception = Assert.ThrowsAsync<ArgumentException>(async () => await Path.RootFolderFromAsync(LocationInvalidRepositoryName).ConfigureAwait(false));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "RemoteRoot";
        var LocationInvalidRemoteRoot = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidRemoteRoot.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidRemoteRoot, null!);
        Exception = Assert.ThrowsAsync<ArgumentException>(async () => await Path.RootFolderFromAsync(LocationInvalidRemoteRoot).ConfigureAwait(false));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
#endif
    }
}
