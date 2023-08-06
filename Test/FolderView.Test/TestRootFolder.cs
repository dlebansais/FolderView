namespace FolderView.Test;

using System;
using NUnit.Framework;

[TestFixture]
public class TestRootFolder
{
    [Test]
    public void CreateWithLocalUri()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var TestObject = new RootFolder(LocalUri);
        AssertRootFolderStructure(TestObject);
    }

    //[Test]
    public void CreateWithRemoteUri()
    {
        Uri RemoteUri = RootFolderStructure.GetRootAsRemoteUri();

        var TestObject = new RootFolder(RemoteUri);
        AssertRootFolderStructure(TestObject);
    }

    private void AssertRootFolderStructure(RootFolder rootFolder)
    {
        Assert.That(rootFolder, Is.Not.Null);

        var TestObjectFolders = rootFolder.Folders;
        var TestObjectFolderNames = TestObjectFolders.AsNameList();

        Assert.That(TestObjectFolderNames, Has.Count.EqualTo(RootFolderStructure.RootFolders.Count));
        CollectionAssert.AreEqual(TestObjectFolderNames, RootFolderStructure.RootFolders);

        var TestObjectFiles = rootFolder.Files;
        var TestObjectFileNames = TestObjectFiles.AsNameList();

        Assert.That(TestObjectFileNames, Has.Count.EqualTo(RootFolderStructure.RootFiles.Count));
        CollectionAssert.AreEqual(TestObjectFileNames, RootFolderStructure.RootFiles);
    }
}
