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
        bool includeCurlyBraceLines = false;

        foreach (string arg in args)
        {
            if (arg.StartsWith("-FileTypes"))
            {
                fileExtensions = arg.Substring(11).Split(',');
            }
            else if (arg == "-IncludeCurlyBraceLines")
            {
                includeCurlyBraceLines = true;
            }
        }

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("The provided directory does not exist.");
            return;
        }

        int totalLinesOfCode = CountLinesOfCode(directoryPath, fileExtensions, includeCurlyBraceLines);
        Console.WriteLine($"Total lines of code (excluding comments and empty lines): {totalLinesOfCode}");
    }

    static int CountLinesOfCode(string directoryPath, string[] fileExtensions, bool includeCurlyBraceLines)
    {
        int totalLinesOfCode = 0;

        foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
        {
            if (fileExtensions.Contains(Path.GetExtension(file)))
            {
                totalLinesOfCode += CountLinesInFile(file, includeCurlyBraceLines);
            }
        }

        return totalLinesOfCode;
    }

    static int CountLinesInFile(string filePath, bool includeCurlyBraceLines)
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
                if (includeCurlyBraceLines || (!includeCurlyBraceLines && trimmedLine != "{" && trimmedLine != "}"))
                {
                    linesOfCode++;
                }
            }

            if (inBlockComment && trimmedLine.EndsWith("*/"))
            {
                inBlockComment = false;
            }
        }

        return linesOfCode;
    }
}
