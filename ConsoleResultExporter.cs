using System;
using System.Collections.Generic;
using System.Linq;

class ConsoleResultExporter : IResultExporter
{
    public void Export(IOrderedEnumerable<KeyValuePair<string, LineCounts>> sortedResult)
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
    }
}
