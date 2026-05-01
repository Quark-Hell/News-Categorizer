using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewsParser.Interfaces;
using NewsParser.Models;
using NewsParser.RSS;

namespace NewsParser.Sources
{
    public class RBKSource : INewsSource
    {
        private readonly RssClient _rss;

        public string SourceName => "RBK";

        public RBKSource(RssClient rss)
        {
            _rss = rss;
        }

        public Task<IEnumerable<RawNewsItem>> GetItemsAsync(CancellationToken ct)
        {
            return _rss.LoadAsync("https://rssexport.rbc.ru/rbcnews/news/30/full.rss");
        }
    }
}
