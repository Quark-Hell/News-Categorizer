using System;

namespace News.Domain
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; }
        public string Content { get; set; }
    }
}
