using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.Models
{
    public class NewsItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; }
        public string Content { get; set; }
    }
}
