using NewsParser.ContentExtractor;
using NewsParser.ContentLoader;
using News.Domain;
using System;

namespace NewsParser.Parser
{
    public class DefaultNewsParser : INewsParser
    {
        private readonly IContentLoader _loader;
        private readonly IContentExtractorFactory _factory;

        public DefaultNewsParser(IContentLoader loader, IContentExtractorFactory factory)
        {
            _loader = loader;
            _factory = factory;
        }

        public async Task<IEnumerable<NewsItem>> ParseAsync(IEnumerable<RawNewsItem> rawItems)
        {
            var result = new List<NewsItem>();

            foreach (var item in rawItems)
            {
                var html = await _loader.LoadContentAsync(item.Link);

                if (string.IsNullOrEmpty(html))
                {
                    continue;
                }

                var extractor = _factory.GetExtractor(item.Source);
                var content = extractor.Extract(html);

                result.Add(new NewsItem
                {
                    Title = item.Title,
                    Url = item.Link,
                    PublishedAt = item.PublishedAt,
                    Content = content,
                    Source = item.Source
                });
            }

            return result;
        }
    }
}
