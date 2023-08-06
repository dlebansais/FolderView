namespace FolderView.Test;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestFile
{
    [Test]
    public void TestWithLocalUri()
    {
        Uri LocalUri = RootFolderStructure.GetRootAsLocalUri();

        var RootFolder = new RootFolder(LocalUri);
        Assert.That(RootFolder, Is.Not.Null);

        var RootFiles = RootFolder.Files;
        var RootFileNames = RootFiles.AsNameList();
        CollectionAssert.AreEqual(RootFileNames, RootFolderStructure.RootFiles);

        Assert.That(RootFiles, Has.Count.EqualTo(RootFolderStructure.RootFiles.Count));

        TestRootFile(RootFiles, 0);
        TestRootFile(RootFiles, 1);
        TestRootNoMoreFile(RootFiles, 2);
    }

    private void TestRootFile(IList<File> rootFiles, int index)
    {
        File TestObject = rootFiles[index];
        Assert.That(TestObject, Is.Not.Null);

        Assert.That(TestObject.Name, Is.EqualTo(RootFolderStructure.RootFiles[index]));
    }

    private void TestRootNoMoreFile(IList<File> rootFiles, int index)
    {
        Assert.That(rootFiles, Has.Count.EqualTo(index));
    }
}
