namespace FolderView.Test;

using System.Text;
using NUnit.Framework;

[TestFixture]
public class TestFile
{
    [Test]
    public void TestWithLocal()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolderTask = Path.RootFolderFromAsync(Location);
        RootFolderTask.Wait();
        var RootFolder = RootFolderTask.Result;
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

    [Test]
    public void TestLoadLocal()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        TestLoadRootFile(Location);
    }

    [Test]
    public void TestLoadRemote()
    {
#if ENABLE_REMOTE
        ILocation Location = RootFolderStructure.GetRootAsRemoteLocation();
        TestLoadRootFile(Location);
#endif
    }

    private void TestLoadRootFile(ILocation location)
    {
        var RootFolderTask = Path.RootFolderFromAsync(location);
        RootFolderTask.Wait();
        var RootFolder = RootFolderTask.Result;
        Assert.That(RootFolder, Is.Not.Null);

        Assert.That(RootFolder.Files, Has.Count.GreaterThan(0));
        var FirstFile = RootFolder.Files[0];

        var LoadTask = FirstFile.LoadAsync();
        LoadTask.Wait();

        var Content = FirstFile.Content;
        Assert.That(Content, Is.Not.Null);

        string ContentAsString = Encoding.UTF8.GetString(Content);
        Assert.That(ContentAsString, Is.EqualTo(FirstFile.Path.Name));
    }

    [Test]
    public void TestLoadLocalSubfolder()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        TestLoadSubfolderFile(Location);
    }

    [Test]
    public void TestLoadRemoteSubfolder()
    {
#if ENABLE_REMOTE
        ILocation Location = RootFolderStructure.GetRootAsRemoteLocation();
        TestLoadSubfolderFile(Location);
#endif
    }

    private void TestLoadSubfolderFile(ILocation location)
    {
        var RootFolderTask = Path.RootFolderFromAsync(location);
        RootFolderTask.Wait();
        var RootFolder = RootFolderTask.Result;
        Assert.That(RootFolder, Is.Not.Null);

        Assert.That(RootFolder.Folders, Has.Count.GreaterThan(0));
        var FirstFolder = RootFolder.Folders[0];
        Assert.That(FirstFolder.Files, Has.Count.GreaterThan(0));
        var FirstFile = FirstFolder.Files[0];

        var LoadTask = FirstFile.LoadAsync();
        LoadTask.Wait();

        var Content = FirstFile.Content;
        Assert.That(Content, Is.Not.Null);

        string ContentAsString = Encoding.UTF8.GetString(Content);
        Assert.That(ContentAsString, Is.EqualTo(FirstFile.Path.Name));
    }
}
