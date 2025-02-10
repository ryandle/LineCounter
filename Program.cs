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

        if (args.Length > 1 && args[1].StartsWith("-FileTypes"))
        {
            fileExtensions = args[1].Substring(11).Split(',');
        }

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("The provided directory does not exist.");
            return;
        }

        int totalLinesOfCode = CountLinesOfCode(directoryPath, fileExtensions);
        Console.WriteLine($"Total lines of code (excluding comments and empty lines): {totalLinesOfCode}");
    }

    static int CountLinesOfCode(string directoryPath, string[] fileExtensions)
    {
        int totalLinesOfCode = 0;

        foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
        {
            if (fileExtensions.Contains(Path.GetExtension(file)))
            {
                totalLinesOfCode += CountLinesInFile(file);
            }
        }

        return totalLinesOfCode;
    }

    static int CountLinesInFile(string filePath)
    {
        int linesOfCode = 0;
        bool inBlockComment = false;

        foreach (string line in File.ReadLines(filePath))
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("/*"))
            {
                inBlockComment = true;
            }

            if (!inBlockComment && !trimmedLine.StartsWith("//") && !string.IsNullOrEmpty(trimmedLine))
            {
                linesOfCode++;
            }

            if (inBlockComment && trimmedLine.EndsWith("*/"))
            {
                inBlockComment = false;
            }
        }

        return linesOfCode;
    }
}
