using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using News.Domain;


namespace NewsParser.RSS
{
    public class RssClient
    {
        private readonly HttpClient _http;

        public RssClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<NewsItem>> LoadAsync(string url)
        {
            using var stream = await _http.GetStreamAsync(url);
            using var reader = XmlReader.Create(stream);

            var feed = SyndicationFeed.Load(reader);

            return feed.Items.Select(x => new NewsItem
            {
                Title = x.Title.Text,
                Url = x.Links.FirstOrDefault()?.Uri.ToString(),
                PublishedAt = x.PublishDate.UtcDateTime,
                Content = x.Summary?.Text
            });
        }
    }
}
