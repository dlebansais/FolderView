namespace FolderView.Test;

using System;
using NUnit.Framework;

[TestFixture]
public class TestLocation
{
    [DebugOnly]
    [Test]
    public void TestFake()
    {
        ILocation Location = new FakeLocation();
        Exception Exception = Assert.Throws<ArgumentException>(() => Path.RootFolderFrom(Location));
        Assert.That(Exception.Message, Does.Contain(nameof(ILocation)));
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
        Exception = Assert.Throws<NullReferenceException>(() => Path.RootFolderFrom(LocationInvalidLocalRoot));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "UserName";
        var LocationInvalidUserName = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidUserName.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidUserName, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.RootFolderFrom(LocationInvalidUserName));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "RepositoryName";
        var LocationInvalidRepositoryName = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidRepositoryName.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidRepositoryName, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.RootFolderFrom(LocationInvalidRepositoryName));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "RemoteRoot";
        var LocationInvalidRemoteRoot = RootFolderStructure.GetRootAsRemoteLocation();
        LocationInvalidRemoteRoot.GetType().GetProperty(PropertyName)!.SetValue(LocationInvalidRemoteRoot, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.RootFolderFrom(LocationInvalidRemoteRoot));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
    }
}
