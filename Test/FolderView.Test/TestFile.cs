namespace FolderView.Test;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class TestFile
{
    [Test]
    public async Task TestWithLocalAsync()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        using var RootFolder = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        Assert.That(RootFolder, Is.Not.Null);

        var RootFiles = RootFolder.Files;
        var RootFileNames = RootFiles.AsNameList();
        CollectionAssert.AreEquivalent(RootFileNames, RootFolderStructure.RootFiles);

        Assert.That(RootFiles, Has.Count.EqualTo(RootFolderStructure.RootFiles.Count));

        TestRootFile(RootFiles, 0);
        TestRootFile(RootFiles, 1);
        TestRootNoMoreFile(RootFiles, 2);
    }

    private static void TestRootFile(IFileCollection rootFiles, int index)
    {
        IFile TestObject = rootFiles[index];
        Assert.That(TestObject, Is.Not.Null);

        Assert.That(TestObject.Name, Is.EqualTo(RootFolderStructure.RootFiles[index]));
    }

    private static void TestRootNoMoreFile(IFileCollection rootFiles, int index)
    {
        Assert.That(rootFiles, Has.Count.EqualTo(index));
    }

    [Test]
    public async Task TestLoadLocalAsync()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        await TestLoadRootFileAsync(Location).ConfigureAwait(false);
    }

    [Test]
    public async Task TestLoadRemoteAsync()
    {
#if ENABLE_REMOTE
        ILocation Location = RootFolderStructure.GetRootAsRemoteLocation();
        await TestLoadRootFileAsync(Location).ConfigureAwait(false);
#endif
    }

    private static async Task TestLoadRootFileAsync(ILocation location)
    {
        using var RootFolder = await Path.RootFolderFromAsync(location).ConfigureAwait(false);
        Assert.That(RootFolder, Is.Not.Null);

        Assert.That(RootFolder.Files, Has.Count.GreaterThan(0));
        var FirstFile = RootFolder.Files[0];

        await FirstFile.LoadAsync().ConfigureAwait(false);

        using var Content = FirstFile.Content;
        Assert.That(Content, Is.Not.Null);

        using System.IO.StreamReader Reader = new(Content);
        string ContentAsString = await Reader.ReadToEndAsync().ConfigureAwait(false);

        Assert.That(ContentAsString, Is.EqualTo(FirstFile.Path.Name));
    }

    [Test]
    public async Task TestLoadLocalSubfolderAsync()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        await TestLoadSubfolderFileAsync(Location).ConfigureAwait(false);
    }

    [Test]
    public async Task TestLoadRemoteSubfolderAsync()
    {
#if ENABLE_REMOTE
        ILocation Location = RootFolderStructure.GetRootAsRemoteLocation();
        await TestLoadSubfolderFileAsync(Location).ConfigureAwait(false);
#endif
    }

    private static async Task TestLoadSubfolderFileAsync(ILocation location)
    {
        using var RootFolder = await Path.RootFolderFromAsync(location).ConfigureAwait(false);
        Assert.That(RootFolder, Is.Not.Null);

        Assert.That(RootFolder.Folders, Has.Count.GreaterThan(0));
        var FirstFolder = RootFolder.Folders[0];
        Assert.That(FirstFolder.Files, Has.Count.GreaterThan(0));
        var FirstFile = FirstFolder.Files[0];

        await FirstFile.LoadAsync().ConfigureAwait(false);

        using var Content = FirstFile.Content;
        Assert.That(Content, Is.Not.Null);

        using System.IO.StreamReader Reader = new(Content);
        string ContentAsString = await Reader.ReadToEndAsync().ConfigureAwait(false);

        Assert.That(ContentAsString, Is.EqualTo(FirstFile.Path.Name));
    }
}
