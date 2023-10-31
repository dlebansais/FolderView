namespace FolderView.Test;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class TestPath
{
    [Test]
    public async Task TestCombineFolderAsync()
    {
        IPath CombineResult;

        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        const IFolder? NullFolder = null;
        CombineResult = Path.Combine(NullFolder, RootFolderStructure.RootFolders[0]);

        Assert.That(CombineResult.Ancestors, Has.Count.EqualTo(0));
        Assert.That(CombineResult.Name, Is.EqualTo(RootFolderStructure.RootFolders[0]));

        CombineResult = Path.Combine(RootFolder, RootFolderStructure.RootFolders[0]);

        Assert.That(CombineResult.Ancestors, Has.Count.EqualTo(0));
        Assert.That(CombineResult.Name, Is.EqualTo(RootFolderStructure.RootFolders[0]));

        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);
        IFolder FirstLevelFolder = Path.GetRelativeFolder(RootFolder, FirstLevelFolderPath);

        CombineResult = Path.Combine(FirstLevelFolder, RootFolderStructure.Folder_0_0_Folders[0]);

        Assert.That(CombineResult.Ancestors, Has.Count.EqualTo(1));
        Assert.That(CombineResult.Ancestors[0], Is.EqualTo(RootFolderStructure.RootFolders[0]));
        Assert.That(CombineResult.Name, Is.EqualTo(RootFolderStructure.Folder_0_0_Folders[0]));
    }

    [Test]
    public void TestCombinePath()
    {
        const IFolder? NullFolder = null;
        IPath FirstLevelPath = Path.Combine(NullFolder, RootFolderStructure.RootFolders[0]);

        Assert.That(FirstLevelPath.Ancestors, Has.Count.EqualTo(0));
        Assert.That(FirstLevelPath.Name, Is.EqualTo(RootFolderStructure.RootFolders[0]));

        IPath SecondLevelPath = Path.Combine(FirstLevelPath, RootFolderStructure.Folder_0_0_Folders[0]);

        Assert.That(SecondLevelPath.Ancestors, Has.Count.EqualTo(1));
        Assert.That(SecondLevelPath.Ancestors[0], Is.EqualTo(RootFolderStructure.RootFolders[0]));
        Assert.That(SecondLevelPath.Name, Is.EqualTo(RootFolderStructure.Folder_0_0_Folders[0]));
    }

    [Test]
    public async Task TestFirstLevelFolderAsync()
    {
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);

        IFolder GetResult = Path.GetRelativeFolder(RootFolder, FirstLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public async Task TestFirstLevelFileAsync()
    {
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path FirstLevelFilePath = new(new List<string>(), RootFolderStructure.RootFiles[0]);

        IFile GetResult = Path.GetRelativeFile(RootFolder, FirstLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }

    [Test]
    public async Task TestSecondLevelFolderAsync()
    {
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path SecondLevelFolderPath = new(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);

        IFolder GetResult = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Folders[0]));
    }

    [Test]
    public async Task TestSecondLevelFileAsync()
    {
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path SecondLevelFilePath = new(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Files[0]);

        IFile GetResult = Path.GetRelativeFile(RootFolder, SecondLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Files[0]));
    }

    [Test]
    public async Task TestAncestorLevelFolderAsync()
    {
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path SecondLevelFolderPath = new(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        IFolder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFolderPath = new(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFolders[0]);
        IFolder GetResult = Path.GetRelativeFolder(SecondLevelFolder, AncestorLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public async Task TestAncestorLevelFileAsync()
    {
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path SecondLevelFolderPath = new(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        IFolder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFilePath = new(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFiles[0]);
        IFile GetResult = Path.GetRelativeFile(SecondLevelFolder, AncestorLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }

    [Test]
    public void TestTo()
    {
        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path SecondLevelFolderPath = new(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);

        IPath FirstToSecondPath = FirstLevelFolderPath.To(RootFolderStructure.Folder_0_0_Folders[0]);
        Assert.That(FirstToSecondPath, Is.EqualTo(SecondLevelFolderPath));
        Assert.That(FirstToSecondPath.GetHashCode(), Is.EqualTo(SecondLevelFolderPath.GetHashCode()));

        IPath SamePath = FirstToSecondPath.Up();
        Assert.That(FirstLevelFolderPath, Is.EqualTo(SamePath));
    }

    [Test]
    public void TestUp()
    {
        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path SecondLevelFolderPath = new(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);

        IPath SecondToFirstPath = SecondLevelFolderPath.Up();
        Assert.That(SecondToFirstPath, Is.EqualTo(FirstLevelFolderPath));
        Assert.That(SecondToFirstPath.GetHashCode(), Is.EqualTo(FirstLevelFolderPath.GetHashCode()));

        IPath SamePath = SecondToFirstPath.To(RootFolderStructure.Folder_0_0_Folders[0]);
        Assert.That(SecondLevelFolderPath, Is.EqualTo(SamePath));
    }

    [DebugOnly]
    [Test]
    public async Task TestNullAsync()
    {
        Exception Exception;

        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path FirstLevelFilePath = new(new List<string>(), RootFolderStructure.RootFiles[0]);

        Exception = Assert.ThrowsAsync<NullReferenceException>(async () => await Path.RootFolderFromAsync(null!).ConfigureAwait(false));
        Assert.That(Exception.Message, Is.EqualTo("location"));

        const IFolder? NullFolder = null;
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(NullFolder, null!));
        Assert.That(Exception.Message, Is.EqualTo("name"));

        const IPath? NullPath = null!;
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(NullPath, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo("parent"));

        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(null!, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo("parent"));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(RootFolder, null!));
        Assert.That(Exception.Message, Is.EqualTo("path"));

        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(null!, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo("parent"));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(RootFolder, null!));
        Assert.That(Exception.Message, Is.EqualTo("path"));
    }

    [DebugOnly]
    [Test]
    public async Task TestInvalidAsync()
    {
        Exception Exception;
        string PropertyName;

        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path FirstLevelFilePath = new(new List<string>(), RootFolderStructure.RootFiles[0]);

        PropertyName = "Name";
        var FolderInvalidName = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        FolderInvalidName.GetType().GetProperty(PropertyName)!.SetValue(FolderInvalidName, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(FolderInvalidName, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(FolderInvalidName, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(FolderInvalidName, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "Folders";
        var FolderInvalidFolders = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        FolderInvalidFolders.GetType().GetProperty(PropertyName)!.SetValue(FolderInvalidFolders, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(FolderInvalidFolders, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(FolderInvalidFolders, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(FolderInvalidFolders, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "Files";
        var FolderInvalidFiles = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        FolderInvalidFiles.GetType().GetProperty(PropertyName)!.SetValue(FolderInvalidFiles, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(FolderInvalidFiles, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(FolderInvalidFiles, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(FolderInvalidFiles, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        Path FolderPathInvalidAncestor = new(null!, RootFolderStructure.RootFolders[0]);
        Path FolderPathInvalidName = new(new List<string>(), null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(RootFolder, FolderPathInvalidAncestor));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Ancestors)));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(RootFolder, FolderPathInvalidName));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Name)));

        Path FilePathInvalidAncestor = new(null!, RootFolderStructure.RootFiles[0]);
        Path FilePathInvalidName = new(new List<string>(), null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(RootFolder, FilePathInvalidAncestor));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Ancestors)));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(RootFolder, FilePathInvalidName));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Name)));
    }

    [DebugOnly]
    [Test]
    public async Task TestFakeAsync()
    {
        Exception Exception;

        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        using var RootFolder = await TestTools.LoadLocalRootAsync().ConfigureAwait(false);

        using var FakeFolder = new FakeFolder(null, string.Empty, RootFolder.Folders, RootFolder.Files);
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FakeFolder, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolder)));

        Path FirstLevelFolderPath = new(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path FirstLevelFilePath = new(new List<string>(), RootFolderStructure.RootFiles[0]);

        FakePath TestFakePath = new(new List<string>(), string.Empty);
        _ = Assert.Throws<NotImplementedException>(() => TestFakePath.To(string.Empty));
        _ = Assert.Throws<NotImplementedException>(() => TestFakePath.Up());

        var FolderFakePath = new FakePath(new List<string>(), RootFolderStructure.RootFolders[0]);
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFolder(FakeFolder, FirstLevelFolderPath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolder)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFolder(RootFolder, FolderFakePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IPath)));

        var FileFakePath = new FakePath(new List<string>(), RootFolderStructure.RootFiles[0]);
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FileFakePath, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IPath)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFile(FakeFolder, FirstLevelFilePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolder)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFile(RootFolder, FileFakePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IPath)));

        var FolderInvalidFolders = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        FolderInvalidFolders.GetType().GetProperty("Folders")!.SetValue(FolderInvalidFolders, new FakeFolderCollection());
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FolderInvalidFolders, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolderCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFolder(FolderInvalidFolders, FirstLevelFolderPath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolderCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFile(FolderInvalidFolders, FirstLevelFilePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolderCollection)));

        var FolderInvalidFiles = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);
        FolderInvalidFiles.GetType().GetProperty("Files")!.SetValue(FolderInvalidFiles, new FakeFileCollection());
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FolderInvalidFiles, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IFileCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFolder(FolderInvalidFiles, FirstLevelFolderPath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFileCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFile(FolderInvalidFiles, FirstLevelFilePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFileCollection)));
    }
}
