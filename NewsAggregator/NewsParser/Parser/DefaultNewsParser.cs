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

        public async Task<IEnumerable<NewsItem>> ParseAsync(IEnumerable<NewsItem> rawItems)
        {
            var result = new List<NewsItem>();

            foreach (var item in rawItems)
            {
                var html = await _loader.LoadContentAsync(item.Url);

                if (string.IsNullOrEmpty(html))
                {
                    continue;
                }

                var extractor = _factory.GetExtractor(item.Source);
                var content = extractor.Extract(html);

                item.Content = content;

                result.Add(item);
            }

            return result;
        }
    }
}
