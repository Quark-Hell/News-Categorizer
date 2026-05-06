using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Domain;
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

        public async Task<IEnumerable<NewsItem>> GetItemsAsync(CancellationToken ct)
        {
            var items = await _rss.LoadAsync("https://rssexport.rbc.ru/rbcnews/news/30/full.rss");

            return items.Select(x =>
            {
                x.Source = SourceName;
                return x;
            });
        }
    }
}
