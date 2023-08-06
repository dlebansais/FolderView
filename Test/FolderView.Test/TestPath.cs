﻿namespace FolderView.Test;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestPath
{
    [Test]
    public void TestFirstLevelFolder()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        Path FirstLevelFolderPath = new Path(new List<string>(), RootFolderStructure.RootFolders[0]);

        Folder GetResult = Path.GetRelativeFolder(RootFolder, FirstLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public void TestFirstLevelFile()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        Path FirstLevelFilePath = new Path(new List<string>(), RootFolderStructure.RootFiles[0]);

        File GetResult = Path.GetRelativeFile(RootFolder, FirstLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }

    [Test]
    public void TestSecondLevelFolder()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);

        Folder GetResult = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Folders[0]));
    }

    [Test]
    public void TestSecondLevelFile()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFilePath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Files[0]);

        File GetResult = Path.GetRelativeFile(RootFolder, SecondLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0].Files[0]));
    }

    [Test]
    public void TestAncestorLevelFolder()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        Folder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFolderPath = new Path(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFolders[0]);
        Folder GetResult = Path.GetRelativeFolder(SecondLevelFolder, AncestorLevelFolderPath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Folders[0]));
    }

    [Test]
    public void TestAncestorLevelFile()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        Path SecondLevelFolderPath = new Path(new List<string>() { RootFolderStructure.RootFolders[0] }, RootFolderStructure.Folder_0_0_Folders[0]);
        Folder SecondLevelFolder = Path.GetRelativeFolder(RootFolder, SecondLevelFolderPath);
        Path AncestorLevelFilePath = new Path(new List<string>() { Path.Ancestor, Path.Ancestor }, RootFolderStructure.RootFiles[0]);
        File GetResult = Path.GetRelativeFile(SecondLevelFolder, AncestorLevelFilePath);

        Assert.That(GetResult, Is.EqualTo(RootFolder.Files[0]));
    }
}
