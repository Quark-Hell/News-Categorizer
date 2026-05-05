using System;

namespace News.Domain
{
    public class RawNewsItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }
}
