namespace FolderView.Test;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestPath
{
    [Test]
    public void TestCombineFolder()
    {
        IPath CombineResult;

        var RootFolder = TestTools.LoadLocalRoot();

        IFolder? NullFolder = null;
        CombineResult = Path.Combine(NullFolder, RootFolderStructure.RootFolders[0]);

        Assert.That(CombineResult.Ancestors, Has.Count.EqualTo(0));
        Assert.That(CombineResult.Name, Is.EqualTo(RootFolderStructure.RootFolders[0]));

        CombineResult = Path.Combine(RootFolder, RootFolderStructure.RootFolders[0]);

        Assert.That(CombineResult.Ancestors, Has.Count.EqualTo(0));
        Assert.That(CombineResult.Name, Is.EqualTo(RootFolderStructure.RootFolders[0]));

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);
        IFolder FirstLevelFolder = Path.GetRelativeFolder(RootFolder, FirstLevelFolderPath);

        CombineResult = Path.Combine(FirstLevelFolder, RootFolderStructure.Folder_0_0_Folders[0]);

        Assert.That(CombineResult.Ancestors, Has.Count.EqualTo(1));
        Assert.That(CombineResult.Ancestors[0], Is.EqualTo(RootFolderStructure.RootFolders[0]));
        Assert.That(CombineResult.Name, Is.EqualTo(RootFolderStructure.Folder_0_0_Folders[0]));
    }

    [Test]
    public void TestCombinePath()
    {
        IFolder? NullFolder = null;
        IPath FirstLevelPath = Path.Combine(NullFolder, RootFolderStructure.RootFolders[0]);

        Assert.That(FirstLevelPath.Ancestors, Has.Count.EqualTo(0));
        Assert.That(FirstLevelPath.Name, Is.EqualTo(RootFolderStructure.RootFolders[0]));

        IPath SecondLevelPath = Path.Combine(FirstLevelPath, RootFolderStructure.Folder_0_0_Folders[0]);

        Assert.That(SecondLevelPath.Ancestors, Has.Count.EqualTo(1));
        Assert.That(SecondLevelPath.Ancestors[0], Is.EqualTo(RootFolderStructure.RootFolders[0]));
        Assert.That(SecondLevelPath.Name, Is.EqualTo(RootFolderStructure.Folder_0_0_Folders[0]));
    }

    [Test]
    public void TestFirstLevelFolder()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);

        IFolder GetResult = Path.GetRelativeFolder(RootFolder, FirstLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public void TestFirstLevelFile()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        Path FirstLevelFilePath = new Path(new List<string>(), RootFolderStructure.RootFiles[0]);

        IFile GetResult = Path.GetRelativeFile(RootFolder, FirstLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }

    [Test]
    public void TestSecondLevelFolder()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);

        IFolder GetResult = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Folders[0]));
    }

    [Test]
    public void TestSecondLevelFile()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        Path SecondLevelFilePath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Files[0]);

        IFile GetResult = Path.GetRelativeFile(RootFolder, SecondLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Files[0]));
    }

    [Test]
    public void TestAncestorLevelFolder()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        IFolder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFolderPath = new Path(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFolders[0]);
        IFolder GetResult = Path.GetRelativeFolder(SecondLevelFolder, AncestorLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public void TestAncestorLevelFile()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        IFolder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFilePath = new Path(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFiles[0]);
        IFile GetResult = Path.GetRelativeFile(SecondLevelFolder, AncestorLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }

    [DebugOnly]
    [Test]
    public void TestNull()
    {
        Exception Exception;

        var RootFolder = TestTools.LoadLocalRoot();

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path FirstLevelFilePath = new Path(new List<string>(), RootFolderStructure.RootFiles[0]);

        Exception = Assert.ThrowsAsync<NullReferenceException>(async () => await Path.RootFolderFromAsync(null!));
        Assert.That(Exception.Message, Is.EqualTo("location"));

        IFolder? NullFolder = null;
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(NullFolder, null!));
        Assert.That(Exception.Message, Is.EqualTo("name"));

        IPath? NullPath = null!;
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
    public void TestInvalid()
    {
        Exception Exception;
        string PropertyName;

        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        var RootFolder = TestTools.LoadLocalRoot();

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path FirstLevelFilePath = new Path(new List<string>(), RootFolderStructure.RootFiles[0]);

        PropertyName = "Name";
        var FolderInvalidNameTask = Path.RootFolderFromAsync(Location);
        FolderInvalidNameTask.Wait();
        var FolderInvalidName = FolderInvalidNameTask.Result;
        FolderInvalidName.GetType().GetProperty(PropertyName)!.SetValue(FolderInvalidName, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(FolderInvalidName, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(FolderInvalidName, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(FolderInvalidName, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "Folders";
        var FolderInvalidFoldersTask = Path.RootFolderFromAsync(Location);
        FolderInvalidFoldersTask.Wait();
        var FolderInvalidFolders = FolderInvalidFoldersTask.Result;
        FolderInvalidFolders.GetType().GetProperty(PropertyName)!.SetValue(FolderInvalidFolders, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(FolderInvalidFolders, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(FolderInvalidFolders, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(FolderInvalidFolders, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        PropertyName = "Files";
        var FolderInvalidFilesTask = Path.RootFolderFromAsync(Location);
        FolderInvalidFilesTask.Wait();
        var FolderInvalidFiles = FolderInvalidFilesTask.Result;
        FolderInvalidFiles.GetType().GetProperty(PropertyName)!.SetValue(FolderInvalidFiles, null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.Combine(FolderInvalidFiles, string.Empty));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(FolderInvalidFiles, FirstLevelFolderPath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(FolderInvalidFiles, FirstLevelFilePath));
        Assert.That(Exception.Message, Is.EqualTo(PropertyName));

        Path FolderPathInvalidAncestor = new Path(null!, RootFolderStructure.RootFolders[0]);
        Path FolderPathInvalidName = new Path(new List<string>(), null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(RootFolder, FolderPathInvalidAncestor));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Ancestors)));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFolder(RootFolder, FolderPathInvalidName));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Name)));

        Path FilePathInvalidAncestor = new Path(null!, RootFolderStructure.RootFiles[0]);
        Path FilePathInvalidName = new Path(new List<string>(), null!);
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(RootFolder, FilePathInvalidAncestor));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Ancestors)));
        Exception = Assert.Throws<NullReferenceException>(() => Path.GetRelativeFile(RootFolder, FilePathInvalidName));
        Assert.That(Exception.Message, Is.EqualTo(nameof(Path.Name)));
    }

    [DebugOnly]
    [Test]
    public void TestFake()
    {
        Exception Exception;

        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();
        var RootFolder = TestTools.LoadLocalRoot();

        var FakeFolder = new FakeFolder(null, string.Empty, RootFolder.Folders, RootFolder.Files);
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FakeFolder, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolder)));

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);
        Path FirstLevelFilePath = new Path(new List<string>(), RootFolderStructure.RootFiles[0]);

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

        var FolderInvalidFoldersTask = Path.RootFolderFromAsync(Location);
        FolderInvalidFoldersTask.Wait();
        var FolderInvalidFolders = FolderInvalidFoldersTask.Result;
        FolderInvalidFolders.GetType().GetProperty("Folders")!.SetValue(FolderInvalidFolders, new FakeFolderCollection());
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FolderInvalidFolders, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolderCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFolder(FolderInvalidFolders, FirstLevelFolderPath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolderCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFile(FolderInvalidFolders, FirstLevelFilePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFolderCollection)));

        var FolderInvalidFilesTask = Path.RootFolderFromAsync(Location);
        FolderInvalidFilesTask.Wait();
        var FolderInvalidFiles = FolderInvalidFilesTask.Result;
        FolderInvalidFiles.GetType().GetProperty("Files")!.SetValue(FolderInvalidFiles, new FakeFileCollection());
        Exception = Assert.Throws<ArgumentException>(() => Path.Combine(FolderInvalidFiles, string.Empty));
        Assert.That(Exception.Message, Does.Contain(nameof(IFileCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFolder(FolderInvalidFiles, FirstLevelFolderPath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFileCollection)));
        Exception = Assert.Throws<ArgumentException>(() => Path.GetRelativeFile(FolderInvalidFiles, FirstLevelFilePath));
        Assert.That(Exception.Message, Does.Contain(nameof(IFileCollection)));
    }
}
