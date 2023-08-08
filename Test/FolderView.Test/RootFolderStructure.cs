namespace FolderView.Test;

using System.Collections.Generic;

public static class RootFolderStructure
{
    public static List<string> RootFolders { get; } = new() { "Folder_0_0", "Folder_0_1", "Folder_0_2" };
    public static List<string> RootFiles { get; } = new() { "File_0.txt", "File_1.txt" };

    public static List<string> Folder_0_0_Folders { get; } = new() { "Folder_1_0" };
    public static List<string> Folder_0_0_Files { get; } = new() { "File_0.txt", "File_1.txt" };

    public static List<string> Folder_0_0_Folder_1_0_Folders { get; } = new();
    public static List<string> Folder_0_0_Folder_1_0_Files { get; } = new() { "File_0.txt" };

    public static List<string> Folder_0_1_Folders { get; } = new() { "Folder_1_0" };
    public static List<string> Folder_0_1_Files { get; } = new();

    public static List<string> Folder_0_1_Folder_1_0_Folders { get; } = new();
    public static List<string> Folder_0_1_Folder_1_0_Files { get; } = new() { "File_0.txt" };

    public static List<string> Folder_0_2_Folders { get; } = new();
    public static List<string> Folder_0_2_Files { get; } = new() { "File_0.txt" };

    private const string LocalFolderStructureName = "TestRootFolder";
    private const string GitHubProjectUserName = "dlebansais";
    private const string GitHubProjectRepositoryName = "FolderView";

    public static ILocation GetRootAsLocalLocation()
    {
        string TestProjectRoot = TestTools.GetExecutingProjectRootPath();
        string TestRoot = System.IO.Path.GetDirectoryName(TestProjectRoot)!;
        string LocalUriString = System.IO.Path.Combine(TestRoot, LocalFolderStructureName);
        LocalLocation LocalLocation = new(LocalUriString);

        return LocalLocation;
    }

    public static ILocation GetRootAsRemoteLocation()
    {
        string RemoteRoot = $"Test/{LocalFolderStructureName}";
        GitHubLocation RemoteLocation = new(GitHubProjectUserName, GitHubProjectRepositoryName, RemoteRoot);

        return RemoteLocation;
    }
}
