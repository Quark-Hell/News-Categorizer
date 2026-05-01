using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewsParser.Models;

namespace NewsParser.Interfaces
{
    public interface INewsParser
    {
        Task<IEnumerable<NewsItem>> ParseAsync(IEnumerable<RawNewsItem> rawItems);
    }
}
