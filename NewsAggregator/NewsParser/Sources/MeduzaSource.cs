using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Domain;
using NewsParser.RSS;

namespace NewsParser.Sources
{
    public class MeduzaSource : INewsSource
    {
        private readonly RssClient _rss;

        public string SourceName => "Meduza";

        public MeduzaSource(RssClient rss)
        {
            _rss = rss;
        }

        public async Task<IEnumerable<RawNewsItem>> GetItemsAsync(CancellationToken ct)
        {
            var items = await _rss.LoadAsync("https://meduza.io/rss/all");

            return items.Select(x =>
            {
                x.Source = SourceName;
                return x;
            });
        }
    }
}
