namespace FolderView.Test;

using NUnit.Framework;

[TestFixture]
public class TestRootFolder
{
    [Test]
    public void CreateWithLocalUri()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var TestObject = Path.RootFolderFromAsync(Location);
        TestObject.Wait();
        AssertRootFolderStructure(TestObject.Result);
    }

    [Test]
    public void CreateWithRemoteUri()
    {
#if ENABLE_REMOTE
        ILocation Location = RootFolderStructure.GetRootAsRemoteLocation();

        var TestObject = Path.RootFolderFromAsync(Location);
        TestObject.Wait();
        AssertRootFolderStructure(TestObject.Result);
#endif
    }

    private void AssertRootFolderStructure(IFolder rootFolder)
    {
        Assert.That(rootFolder, Is.Not.Null);

        var TestObjectFolders = rootFolder.Folders;
        var TestObjectFolderNames = TestObjectFolders.AsNameList();

        Assert.That(TestObjectFolderNames, Has.Count.EqualTo(RootFolderStructure.RootFolders.Count));
        CollectionAssert.AreEquivalent(TestObjectFolderNames, RootFolderStructure.RootFolders);

        var TestObjectFiles = rootFolder.Files;
        var TestObjectFileNames = TestObjectFiles.AsNameList();

        Assert.That(TestObjectFileNames, Has.Count.EqualTo(RootFolderStructure.RootFiles.Count));
        CollectionAssert.AreEquivalent(TestObjectFileNames, RootFolderStructure.RootFiles);
    }
}
