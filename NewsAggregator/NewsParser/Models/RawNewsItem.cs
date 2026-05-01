using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.Models
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
