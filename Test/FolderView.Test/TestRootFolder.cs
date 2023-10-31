namespace FolderView.Test;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class TestRootFolder
{
    [Test]
    public async Task CreateWithLocalUriAsync()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        using var TestObject = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        AssertRootFolderStructure(TestObject);
    }

    [Test]
    public async Task CreateWithRemoteUriAsync()
    {
#if ENABLE_REMOTE
        ILocation Location = RootFolderStructure.GetRootAsRemoteLocation();

        using var TestObject = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        AssertRootFolderStructure(TestObject);
#endif
    }

    private static void AssertRootFolderStructure(IFolder rootFolder)
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
