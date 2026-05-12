using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public string Username { get; set; } = string.Empty;
        public TimeOnly DigestTime { get; set; }
        public List<UserTopicSubscription> Subscriptions { get; set; } = [];

    }
}
