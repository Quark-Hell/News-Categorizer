using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<NewsTopic> NewsTopics { get; set; } = new();
    }
}
