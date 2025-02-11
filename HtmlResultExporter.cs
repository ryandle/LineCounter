using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class HtmlResultExporter : IResultExporter
{
    public void Export(IOrderedEnumerable<KeyValuePair<string, LineCounts>> sortedResult, List<FileLineCounts> top25Files)
    {
        StringBuilder html = new StringBuilder();
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<title>Line Counts Report</title>");
        html.AppendLine("<style>");
        html.AppendLine("th { cursor: pointer; }");
        html.AppendLine("</style>");
        html.AppendLine("<script>");
        html.AppendLine("function sortTable(n) {");
        html.AppendLine("  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
        html.AppendLine("  table = document.getElementById('lineCountsTable');");
        html.AppendLine("  switching = true;");
        html.AppendLine("  dir = 'asc';");
        html.AppendLine("  while (switching) {");
        html.AppendLine("    switching = false;");
        html.AppendLine("    rows = table.rows;");
        html.AppendLine("    for (i = 1; i < (rows.length - 1); i++) {");
        html.AppendLine("      shouldSwitch = false;");
        html.AppendLine("      x = rows[i].getElementsByTagName('TD')[n];");
        html.AppendLine("      y = rows[i + 1].getElementsByTagName('TD')[n];");
        html.AppendLine("      if (dir == 'asc') {");
        html.AppendLine("        if (parseInt(x.innerHTML) > parseInt(y.innerHTML)) {");
        html.AppendLine("          shouldSwitch = true;");
        html.AppendLine("          break;");
        html.AppendLine("        }");
        html.AppendLine("      } else if (dir == 'desc') {");
        html.AppendLine("        if (parseInt(x.innerHTML) < parseInt(y.innerHTML)) {");
        html.AppendLine("          shouldSwitch = true;");
        html.AppendLine("          break;");
        html.AppendLine("        }");
        html.AppendLine("      }");
        html.AppendLine("    }");
        html.AppendLine("    if (shouldSwitch) {");
        html.AppendLine("      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
        html.AppendLine("      switching = true;");
        html.AppendLine("      switchcount ++;");
        html.AppendLine("    } else {");
        html.AppendLine("      if (switchcount == 0 && dir == 'asc') {");
        html.AppendLine("        dir = 'desc';");
        html.AppendLine("        switching = true;");
        html.AppendLine("      }");
        html.AppendLine("    }");
        html.AppendLine("  }");
        html.AppendLine("}");
        html.AppendLine("</script>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("<h1>Line Counts Report</h1>");
        html.AppendLine("<table id='lineCountsTable' border='1'>");
        html.AppendLine("<tr>");
        html.AppendLine("<th onclick='sortTable(0)'>Folder</th>");
        html.AppendLine("<th onclick='sortTable(1)'>Code Lines</th>");
        html.AppendLine("<th onclick='sortTable(2)'>Comment Lines</th>");
        html.AppendLine("<th onclick='sortTable(3)'>Brace Lines</th>");
        html.AppendLine("<th onclick='sortTable(4)'>Using Lines</th>");
        html.AppendLine("<th onclick='sortTable(5)'>Total Lines</th>");
        html.AppendLine("</tr>");

        int totalCodeLines = 0;
        int totalCommentLines = 0;
        int totalBraceLines = 0;
        int totalUsingLines = 0;
        int totalLines = 0;

        foreach (var folder in sortedResult)
        {
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{folder.Key}</td>");
            html.AppendLine($"<td>{folder.Value.CodeLines}</td>");
            html.AppendLine($"<td>{folder.Value.CommentLines}</td>");
            html.AppendLine($"<td>{folder.Value.BraceLines}</td>");
            html.AppendLine($"<td>{folder.Value.UsingLines}</td>");
            html.AppendLine($"<td><b>{folder.Value.TotalLines}</b></td>");
            html.AppendLine("</tr>");

            totalCodeLines += folder.Value.CodeLines;
            totalCommentLines += folder.Value.CommentLines;
            totalBraceLines += folder.Value.BraceLines;
            totalUsingLines += folder.Value.UsingLines;
            totalLines += folder.Value.TotalLines;
        }

        html.AppendLine("<tr>");
        html.AppendLine("<td><b>Total</b></td>");
        html.AppendLine($"<td><b>{totalCodeLines}</b></td>");
        html.AppendLine($"<td><b>{totalCommentLines}</b></td>");
        html.AppendLine($"<td><b>{totalBraceLines}</b></td>");
        html.AppendLine($"<td><b>{totalUsingLines}</b></td>");
        html.AppendLine($"<td><b>{totalLines}</b></td>");
        html.AppendLine("</tr>");

        html.AppendLine("</table>");

        html.AppendLine("<h2>Top 25 Files by Code Lines</h2>");
        html.AppendLine("<table id='top25FilesTable' border='1'>");
        html.AppendLine("<tr>");
        html.AppendLine("<th>File Path</th>");
        html.AppendLine("<th>Code Lines</th>");
        html.AppendLine("<th>Comment Lines</th>");
        html.AppendLine("<th>Brace Lines</th>");
        html.AppendLine("<th>Using Lines</th>");
        html.AppendLine("<th>Total Lines</th>");
        html.AppendLine("</tr>");

        foreach (var file in top25Files)
        {
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{file.FilePath}</td>");
            html.AppendLine($"<td>{file.CodeLines}</td>");
            html.AppendLine($"<td>{file.CommentLines}</td>");
            html.AppendLine($"<td>{file.BraceLines}</td>");
            html.AppendLine($"<td>{file.UsingLines}</td>");
            html.AppendLine($"<td>{file.TotalLines}</td>");
            html.AppendLine("</tr>");
        }

        html.AppendLine("</table>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "LineCountsReport.html"), html.ToString());
    }
}
