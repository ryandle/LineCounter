using System;
using System.Collections.Generic;
using System.Linq;

class ConsoleResultExporter : IResultExporter
{
    public void Export(IOrderedEnumerable<KeyValuePair<string, LineCounts>> sortedResult, List<FileLineCounts> top25Files)
    {
        foreach (KeyValuePair<string, LineCounts> folder in sortedResult)
        {
            Console.WriteLine($"Folder: {folder.Key}");
            Console.WriteLine($"\tCode lines: {folder.Value.CodeLines}");
            Console.WriteLine($"\tComment lines: {folder.Value.CommentLines}");
            Console.WriteLine($"\tBrace lines: {folder.Value.BraceLines}");
            Console.WriteLine($"\tUsing lines: {folder.Value.UsingLines}");
            Console.WriteLine($"\tTotal lines: {folder.Value.TotalLines}");
        }

        Console.WriteLine("\nTop 25 Files by Code Lines:");
        foreach (var file in top25Files)
        {
            Console.WriteLine($"File: {file.FilePath}");
            Console.WriteLine($"\tCode lines: {file.CodeLines}");
            Console.WriteLine($"\tComment lines: {file.CommentLines}");
            Console.WriteLine($"\tBrace lines: {file.BraceLines}");
            Console.WriteLine($"\tUsing lines: {file.UsingLines}");
            Console.WriteLine($"\tTotal lines: {file.TotalLines}");
        }
    }
}
