using News.Domain.Models;
using System;

namespace News.Domain
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public bool IsSummarized { get; set; } = false;

        public List<NewsTopic> NewsTopics { get; set; } = new();
    }
}
