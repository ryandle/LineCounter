using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

// Define a record to replace the tuple
record LineCounts(int CodeLines, int CommentLines, int BraceLines, int TotalLines);

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

        Dictionary<string, LineCounts> result = CountLinesOfCode(directoryPath, fileExtensions);

        // Sort the result by TotalLines in descending order
        IOrderedEnumerable<KeyValuePair<string, LineCounts>> sortedResult = result.OrderByDescending(folder => folder.Value.TotalLines);

        foreach (KeyValuePair<string, LineCounts> folder in sortedResult)
        {
            Console.WriteLine($"Folder: {folder.Key}");
            Console.WriteLine($"\tCode lines: {folder.Value.CodeLines}");
            Console.WriteLine($"\tComment lines: {folder.Value.CommentLines}");
            Console.WriteLine($"\tBrace lines: {folder.Value.BraceLines}");
            Console.WriteLine($"\tTotal lines: {folder.Value.TotalLines}");
        }
    }

    static Dictionary<string, LineCounts> CountLinesOfCode(string directoryPath, string[] fileExtensions)
    {
        var folderStats = new Dictionary<string, LineCounts>();

        // Count lines in the top-level directory
        int topLevelCodeLines = 0;
        int topLevelCommentLines = 0;
        int topLevelBraceLines = 0;
        int topLevelTotalLines = 0;

        string[] topLevelFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly);
        int totalFiles = topLevelFiles.Length + Directory.GetDirectories(directoryPath).Sum(subDir => Directory.GetFiles(subDir, "*.*", SearchOption.AllDirectories).Length);
        int processedFiles = 0;

        Console.WriteLine($"Counting lines of code in {directoryPath}...{totalFiles} files to process.");
        foreach (string file in topLevelFiles)
        {
            if (fileExtensions.Contains(Path.GetExtension(file)))
            {
                LineCounts result = CountLinesInFile(file);
                topLevelCodeLines += result.CodeLines;
                topLevelCommentLines += result.CommentLines;
                topLevelBraceLines += result.BraceLines;
                topLevelTotalLines += result.TotalLines;
            }
            processedFiles++;
            PrintProgress(processedFiles, totalFiles);
        }

        folderStats[directoryPath] = new LineCounts(topLevelCodeLines, topLevelCommentLines, topLevelBraceLines, topLevelTotalLines);

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
                    LineCounts result = CountLinesInFile(file);
                    codeLines += result.CodeLines;
                    commentLines += result.CommentLines;
                    braceLines += result.BraceLines;
                    totalLines += result.TotalLines;
                }
                processedFiles++;
                PrintProgress(processedFiles, totalFiles);
            }

            folderStats[subDirectory] = new LineCounts(codeLines, commentLines, braceLines, totalLines);
        }

        return folderStats;
    }

    static LineCounts CountLinesInFile(string filePath)
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

        return new LineCounts(codeLines, commentLines, braceLines, totalLines);
    }

    static void PrintProgress(int processedFiles, int totalFiles)
    {
        Console.Write($"\rProcessing Files... {processedFiles}/{totalFiles}");
        if (processedFiles == totalFiles)
        {
            Console.WriteLine();
        }
    }
}
