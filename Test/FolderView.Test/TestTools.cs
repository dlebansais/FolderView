namespace FolderView.Test;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

            switch (FileName)
            {
                case "net7.0":
                case "net7.0-windows7.0":
                case "Debug":
                case "Release":
                case "x64":
                case "bin":
                    CurrentDirectory = ParentFolder;
                    continue;
                default:
                    Continue = false;
                    break;
            }
        }

        Debug.Assert(CurrentDirectory is not null);

        return CurrentDirectory!;
    }

    public static List<string> AsNameList(this IList<Folder> folders)
    {
        return folders.Select(item => item.Name).ToList();
    }

    public static List<string> AsNameList(this IList<File> files)
    {
        return files.Select(item => item.Name).ToList();
    }
}
