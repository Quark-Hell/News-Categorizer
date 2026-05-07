using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Domain;
using NewsParser.RSS;

namespace NewsParser.Sources
{
    public class VCruSource : INewsSource
    {
        private readonly RssClient _rss;

        public string SourceName => "VC.ru";

        public VCruSource(RssClient rss)
        {
            _rss = rss;
        }

        public async Task<IEnumerable<NewsItem>> GetItemsAsync(CancellationToken ct)
        {
            var items = await _rss.LoadAsync("https://vc.ru/rss/all");

            return items.Select(x =>
            {
                x.Source = SourceName;
                return x;
            });
        }
    }
}
