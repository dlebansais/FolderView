namespace FolderView.Test;

using NUnit.Framework;

[TestFixture]
public class TestFile
{
    [Test]
    public void TestWithLocalUri()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        var RootFiles = RootFolder.Files;
        var RootFileNames = RootFiles.AsNameList();
        CollectionAssert.AreEquivalent(RootFileNames, RootFolderStructure.RootFiles);

        Assert.That(RootFiles, Has.Count.EqualTo(RootFolderStructure.RootFiles.Count));

        TestRootFile(RootFiles, 0);
        TestRootFile(RootFiles, 1);
        TestRootNoMoreFile(RootFiles, 2);
    }

    private void TestRootFile(IFileCollection rootFiles, int index)
    {
        IFile TestObject = rootFiles[index];
        Assert.That(TestObject, Is.Not.Null);

        Assert.That(TestObject.Name, Is.EqualTo(RootFolderStructure.RootFiles[index]));
    }

    private void TestRootNoMoreFile(IFileCollection rootFiles, int index)
    {
        Assert.That(rootFiles, Has.Count.EqualTo(index));
    }
}
