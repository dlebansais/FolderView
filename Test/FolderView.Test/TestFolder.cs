namespace FolderView.Test;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestFolder
{
    [Test]
    public void TestWithLocalUri()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        var RootSubfolders = RootFolder.Folders;
        var RootFolderNames = RootSubfolders.AsNameList();
        CollectionAssert.AreEqual(RootFolderNames, RootFolderStructure.RootFolders);

        Assert.That(RootSubfolders, Has.Count.EqualTo(RootFolderStructure.RootFolders.Count));

        TestRootSubfolder(RootSubfolders, 0, RootFolderStructure.Folder_0_0_Folders, RootFolderStructure.Folder_0_0_Files);
        TestRootSubfolder(RootSubfolders, 1, RootFolderStructure.Folder_0_1_Folders, RootFolderStructure.Folder_0_1_Files);
        TestRootSubfolder(RootSubfolders, 2, RootFolderStructure.Folder_0_2_Folders, RootFolderStructure.Folder_0_2_Files);
        TestRootNoMoreFolder(RootSubfolders, 3);
    }

    private void TestRootSubfolder(IList<Folder> rootSubfolders, int index, List<string> folderNames, List<string> fileNames)
    {
        Folder TestObject = rootSubfolders[index];
        Assert.That(TestObject, Is.Not.Null);

        Assert.That(TestObject.Name, Is.EqualTo(RootFolderStructure.RootFolders[index]));

        var TestObjectFolderNames = TestObject.Folders.AsNameList();
        CollectionAssert.AreEqual(TestObjectFolderNames, folderNames);

        var TestObjectFileNames = TestObject.Files.AsNameList();
        CollectionAssert.AreEqual(TestObjectFileNames, fileNames);
    }

    private void TestRootNoMoreFolder(IList<Folder> rootSubfolders, int index)
    {
        Assert.That(rootSubfolders, Has.Count.EqualTo(index));
    }
}
