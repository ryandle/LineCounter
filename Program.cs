using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a directory path.");
            return;
        }

        string directoryPath = args[0];
        string[] fileExtensions = { ".cs", ".js", ".html" };

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "-FileTypes" && i + 1 < args.Length)
            {
                fileExtensions = args[i + 1].Split(',').Select(ext => ext.StartsWith(".") ? ext : "." + ext).ToArray();
                i++;
            }
        }

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("The provided directory does not exist.");
            return;
        }

        var result = CountLinesOfCode(directoryPath, fileExtensions);
        foreach (var folder in result)
        {
            Console.WriteLine($"Folder: {folder.Key}");
            Console.WriteLine($"\tCode lines: {folder.Value.CodeLines}");
            Console.WriteLine($"\tComment lines: {folder.Value.CommentLines}");
            Console.WriteLine($"\tBrace lines: {folder.Value.BraceLines}");
            Console.WriteLine($"\tTotal lines: {folder.Value.TotalLines}");
        }
    }

    static Dictionary<string, (int CodeLines, int CommentLines, int BraceLines, int TotalLines)> CountLinesOfCode(string directoryPath, string[] fileExtensions)
    {
        var folderStats = new Dictionary<string, (int CodeLines, int CommentLines, int BraceLines, int TotalLines)>();

        // Count lines in the top-level directory
        int topLevelCodeLines = 0;
        int topLevelCommentLines = 0;
        int topLevelBraceLines = 0;
        int topLevelTotalLines = 0;

        foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly))
        {
            if (fileExtensions.Contains(Path.GetExtension(file)))
            {
                var result = CountLinesInFile(file);
                topLevelCodeLines += result.CodeLines;
                topLevelCommentLines += result.CommentLines;
                topLevelBraceLines += result.BraceLines;
                topLevelTotalLines += result.TotalLines;
            }
        }

        folderStats[directoryPath] = (topLevelCodeLines, topLevelCommentLines, topLevelBraceLines, topLevelTotalLines);

        // Count lines in the first-level subdirectories
        foreach (string subDirectory in Directory.GetDirectories(directoryPath))
        {
            int codeLines = 0;
            int commentLines = 0;
            int braceLines = 0;
            int totalLines = 0;

            foreach (string file in Directory.GetFiles(subDirectory, "*.*", SearchOption.AllDirectories))
            {
                if (fileExtensions.Contains(Path.GetExtension(file)))
                {
                    var result = CountLinesInFile(file);
                    codeLines += result.CodeLines;
                    commentLines += result.CommentLines;
                    braceLines += result.BraceLines;
                    totalLines += result.TotalLines;
                }
            }

            folderStats[subDirectory] = (codeLines, commentLines, braceLines, totalLines);
        }

        return folderStats;
    }

    static (int CodeLines, int CommentLines, int BraceLines, int TotalLines) CountLinesInFile(string filePath)
    {
        int codeLines = 0;
        int commentLines = 0;
        int braceLines = 0;
        int totalLines = 0;
        bool inBlockComment = false;

        foreach (string line in File.ReadLines(filePath))
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("/*"))
            {
                inBlockComment = true;
            }

            if (inBlockComment || trimmedLine.StartsWith("//"))
            {
                commentLines++;
                totalLines++;
            }
            else if (trimmedLine == "{" || trimmedLine == "}")
            {
                braceLines++;
                totalLines++;
            }
            else if (!string.IsNullOrEmpty(trimmedLine))
            {
                codeLines++;
                totalLines++;
            }

            if (inBlockComment && trimmedLine.EndsWith("*/"))
            {
                inBlockComment = false;
            }
        }

        return (codeLines, commentLines, braceLines, totalLines);
    }
}
