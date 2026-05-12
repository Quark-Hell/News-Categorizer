using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Models
{
    public class UserTopicSubscription
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int TopicId { get; set; }

        public Topic Topic { get; set; } = null!;
    }
}
