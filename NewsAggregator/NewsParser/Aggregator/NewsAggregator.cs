using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NewsParser.Interfaces;
using NewsParser.Models;

namespace NewsParser.Aggregator
{
    public class NewsAggregator
    {
        private readonly IEnumerable<INewsSource> _sources;
        private readonly INewsParser _parser;

        public NewsAggregator(IEnumerable<INewsSource> sources, INewsParser parser)
        {
            _sources = sources;
            _parser = parser;
        }

        public async Task<List<NewsItem>> AggregateAsync(CancellationToken ct)
        {
            var result = new List<NewsItem>();

            foreach (var source in _sources)
            {
                var raw = await source.GetItemsAsync(ct);
                var parsed = await _parser.ParseAsync(raw);

                foreach (var item in parsed)
                {
                    item.Source = source.SourceName;
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
