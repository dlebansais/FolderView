namespace FolderView.Test;

using System;
using NUnit.Framework;

[TestFixture]
public class TestRootFolder
{
    private const string LocalFolderStructureName = "TestRootFolder";
    private const string GitHubProjectUri = "https://github.com/dlebansais/FolderView/";

    [Test]
    public void CreateWithLocalUri()
    {
        string TestProjectRoot = TestTools.GetExecutingProjectRootPath();
        string TestRoot = System.IO.Path.GetDirectoryName(TestProjectRoot)!;
        string LocalUriString = System.IO.Path.Combine(TestRoot, LocalFolderStructureName);
        Uri LocalUri = new(LocalUriString);

        var TestObject = new RootFolder(LocalUri);
        AssertRootFolderStructure(TestObject);
    }

    //[Test]
    public void CreateWithRemoteUri()
    {
        string RemoteUriString = $"{GitHubProjectUri}/Test/{LocalFolderStructureName}";
        Uri RemoteUri = new(RemoteUriString);

        var TestObject = new RootFolder(RemoteUri);
        AssertRootFolderStructure(TestObject);
    }

    private void AssertRootFolderStructure(RootFolder rootFolder)
    {
        Assert.That(rootFolder, Is.Not.Null);

        var TestObjectFolders = rootFolder.Folders;
        var TestObjectFolderNames = TestObjectFolders.AsNameList();

        Assert.That(TestObjectFolderNames, Has.Count.EqualTo(RootFolderStructure.RootFolders.Count));
        for (int Index = 0; Index < TestObjectFolderNames.Count; Index++)
            Assert.That(TestObjectFolderNames[Index], Is.EqualTo(RootFolderStructure.RootFolders[Index]));

        var TestObjectFiles = rootFolder.Files;
        var TestObjectFileNames = TestObjectFiles.AsNameList();

        Assert.That(TestObjectFileNames, Has.Count.EqualTo(RootFolderStructure.RootFiles.Count));
        for (int Index = 0; Index < TestObjectFileNames.Count; Index++)
            Assert.That(TestObjectFileNames[Index], Is.EqualTo(RootFolderStructure.RootFiles[Index]));
    }
}
