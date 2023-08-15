namespace FolderView.Test;

using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestFolder
{
    [Test]
    public void TestWithLocalUri()
    {
        var RootFolder = TestTools.LoadLocalRoot();

        var RootSubfolders = RootFolder.Folders;
        var RootFolderNames = RootSubfolders.AsNameList();
        CollectionAssert.AreEquivalent(RootFolderNames, RootFolderStructure.RootFolders);

        Assert.That(RootSubfolders, Has.Count.EqualTo(RootFolderStructure.RootFolders.Count));

        TestRootSubfolder(RootSubfolders, 0, RootFolderStructure.Folder_0_0_Folders, RootFolderStructure.Folder_0_0_Files);
        TestRootSubfolder(RootSubfolders, 1, RootFolderStructure.Folder_0_1_Folders, RootFolderStructure.Folder_0_1_Files);
        TestRootSubfolder(RootSubfolders, 2, RootFolderStructure.Folder_0_2_Folders, RootFolderStructure.Folder_0_2_Files);
        TestRootNoMoreFolder(RootSubfolders, 3);
    }

    private void TestRootSubfolder(IFolderCollection rootSubfolders, int index, List<string> folderNames, List<string> fileNames)
    {
        IFolder TestObject = rootSubfolders[index];
        Assert.That(TestObject, Is.Not.Null);

        Assert.That(TestObject.Name, Is.EqualTo(RootFolderStructure.RootFolders[index]));

        var TestObjectFolderNames = TestObject.Folders.AsNameList();
        CollectionAssert.AreEquivalent(TestObjectFolderNames, folderNames);

        var TestObjectFileNames = TestObject.Files.AsNameList();
        CollectionAssert.AreEquivalent(TestObjectFileNames, fileNames);
    }

    private void TestRootNoMoreFolder(IFolderCollection rootSubfolders, int index)
    {
        Assert.That(rootSubfolders, Has.Count.EqualTo(index));
    }
}
