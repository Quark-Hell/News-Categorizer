using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Models
{
    public class NewsTopic
    {
        public int NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; } = null!;

        public int TopicId { get; set; }
        public Topic Topic { get; set; } = null!;

        public double Score { get; set; } 
    }
}
