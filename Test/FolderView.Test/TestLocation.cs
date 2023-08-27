namespace FolderView.Test;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestLocation
{
    [Test]
    public void TestCanonicalPath()
    {
        LocalLocation Location = (LocalLocation)RootFolderStructure.GetRootAsLocalLocation();
        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);

        string AbsolutePath = Location.GetAbsolutePath(FirstLevelFolderPath);
        string? AbsolutePathFolder = System.IO.Path.GetDirectoryName(AbsolutePath);

        Assert.That(AbsolutePathFolder, Is.EqualTo(Location.CanonicalRoot));
    }

    [DebugOnly]
    [Test]
    public void TestFake()
    {
        ILocation Location = new FakeLocation();
        Exception Exception = Assert.ThrowsAsync<ArgumentException>(async () => await Path.RootFolderFromAsync(Location));

        Assert.That(Exception.Message, Does.Contain(nameof(ILocation)));

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);

        Assert.Throws<NotImplementedException>(() => Location.GetAbsolutePath(FirstLevelFolderPath));
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
        Exception = Assert.ThrowsAsync<NullReferenceException>(async () => await Path.RootFolderFromAsync(LocationInvalidLocalRoot));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

#if ENABLE_REMOTE
        PropertyName = "UserName";
        var LocationInvalidUserName = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidUserName.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidUserName, null!);
        Exception = Assert.ThrowsAsync<NullReferenceException>(async () => await Path.RootFolderFromAsync(LocationInvalidUserName));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "RepositoryName";
        var LocationInvalidRepositoryName = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidRepositoryName.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidRepositoryName, null!);
        Exception = Assert.ThrowsAsync<NullReferenceException>(async () => await Path.RootFolderFromAsync(LocationInvalidRepositoryName));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "RemoteRoot";
        var LocationInvalidRemoteRoot = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidRemoteRoot.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidRemoteRoot, null!);
        Exception = Assert.ThrowsAsync<NullReferenceException>(async () => await Path.RootFolderFromAsync(LocationInvalidRemoteRoot));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
#endif
    }
}
