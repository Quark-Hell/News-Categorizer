using NewsParser.Interfaces;
using NewsParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.Parses
{
    public class DefaultNewsParser : INewsParser
    {
        public Task<IEnumerable<NewsItem>> ParseAsync(IEnumerable<RawNewsItem> rawItems)
        {
            var result = rawItems.Select(x => new NewsItem
            {
                Title = x.Title,
                Url = x.Link,
                PublishedAt = x.PublishedAt,
                Content = x.Description
            });

            return Task.FromResult(result);
        }
    }
}
