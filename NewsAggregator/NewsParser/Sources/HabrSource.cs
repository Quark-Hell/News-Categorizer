using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsParser.Models;
using NewsParser.RSS;

namespace NewsParser.Sources
{
    public class HabrSource : INewsSource
    {
        private readonly RssClient _rss;

        public string SourceName => "Habr";

        public HabrSource(RssClient rss)
        {
            _rss = rss;
        }

        public async Task<IEnumerable<RawNewsItem>> GetItemsAsync(CancellationToken ct)
        {
            var items = await _rss.LoadAsync("https://habr.com/ru/rss/articles/");

            return items.Select(x =>
            {
                x.Source = SourceName;
                return x;
            });
        }
    }
}
