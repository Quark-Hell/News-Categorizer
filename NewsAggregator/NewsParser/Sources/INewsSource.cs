using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsParser.Models;

namespace NewsParser.Sources
{
    public interface INewsSource
    {
        string SourceName { get; }
        Task<IEnumerable<RawNewsItem>> GetItemsAsync(CancellationToken ct);
    }
}
