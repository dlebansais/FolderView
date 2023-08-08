namespace FolderView.Test;

using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestPath
{
    [Test]
    public void TestFirstLevelFolder()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);

        IFolder GetResult = Path.GetRelativeFolder(RootFolder, FirstLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public void TestFirstLevelFile()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        Path FirstLevelFilePath = new Path(new List<string>(), RootFolderStructure.RootFiles[0]);

        IFile GetResult = Path.GetRelativeFile(RootFolder, FirstLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }

    [Test]
    public void TestSecondLevelFolder()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);

        IFolder GetResult = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Folders[0]));
    }

    [Test]
    public void TestSecondLevelFile()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFilePath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Files[0]);

        IFile GetResult = Path.GetRelativeFile(RootFolder, SecondLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Files[0]));
    }

    [Test]
    public void TestAncestorLevelFolder()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        IFolder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFolderPath = new Path(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFolders[0]);
        IFolder GetResult = Path.GetRelativeFolder(SecondLevelFolder, AncestorLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public void TestAncestorLevelFile()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = Path.RootFolderFrom(Location);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        IFolder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFilePath = new Path(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFiles[0]);
        IFile GetResult = Path.GetRelativeFile(SecondLevelFolder, AncestorLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }
}
