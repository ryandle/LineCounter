using System.Collections.Generic;
using System.Linq;

interface IResultExporter
{
    void Export(IOrderedEnumerable<KeyValuePair<string, LineCounts>> sortedResult);
}
