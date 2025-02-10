using System;
using System.IO;
using System.Linq;

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
        Console.WriteLine($"Code lines: {result.CodeLines}");
        Console.WriteLine($"Comment lines: {result.CommentLines}");
        Console.WriteLine($"Brace lines: {result.BraceLines}");
        Console.WriteLine($"Total lines: {result.TotalLines}");
    }

    static (int CodeLines, int CommentLines, int BraceLines, int TotalLines) CountLinesOfCode(string directoryPath, string[] fileExtensions)
    {
        int codeLines = 0;
        int commentLines = 0;
        int braceLines = 0;
        int totalLines = 0;

        foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
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

        return (codeLines, commentLines, braceLines, totalLines);
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
            totalLines++;

            if (trimmedLine.StartsWith("/*"))
            {
                inBlockComment = true;
            }

            if (inBlockComment || trimmedLine.StartsWith("//"))
            {
                commentLines++;
            }
            else if (trimmedLine == "{" || trimmedLine == "}")
            {
                braceLines++;
            }
            else if (!string.IsNullOrEmpty(trimmedLine))
            {
                codeLines++;
            }

            if (inBlockComment && trimmedLine.EndsWith("*/"))
            {
                inBlockComment = false;
            }
        }

        return (codeLines, commentLines, braceLines, totalLines);
    }
}
