using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Domain;

namespace NewsParser.Parser
{
    public interface INewsParser
    {
        Task<IEnumerable<NewsItem>> ParseAsync(IEnumerable<RawNewsItem> rawItems);
    }
}
