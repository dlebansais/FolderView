namespace FolderView.Test;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public static class TestTools
{
    public static string GetExecutingProjectRootPath()
    {
        string? CurrentDirectory = Environment.CurrentDirectory;
        bool Continue = true;

        while (Continue)
        {
            string? ParentFolder = System.IO.Path.GetDirectoryName(CurrentDirectory);
            string? FileName = System.IO.Path.GetFileName(CurrentDirectory);

            List<string?> KnownFolderNames = new()
            {
                "net481",
                "net7.0",
                "net7.0-windows7.0",
                "net8.0",
                "net8.0-windows7.0",
                "Debug",
                "Release",
                "x64",
                "bin",
            };

            if (KnownFolderNames.Contains(FileName))
                CurrentDirectory = ParentFolder;
            else
                Continue = false;
        }

        Debug.Assert(CurrentDirectory is not null);

        return CurrentDirectory!;
    }

    public static IList<string> AsNameList(this IFolderCollection folders)
    {
        return ((IList<IFolder>)folders).Select(item => item.Name).ToList();
    }

    public static IList<string> AsNameList(this IFileCollection files)
    {
        return ((IList<IFile>)files).Select(item => item.Name).ToList();
    }

    public static async Task<IFolder> LoadLocalRootAsync()
    {
        ILocation Location = RootFolderStructure.GetRootAsLocalLocation();

        var RootFolder = await Path.RootFolderFromAsync(Location).ConfigureAwait(false);

        return RootFolder;
    }
}
