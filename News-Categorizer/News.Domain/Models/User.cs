using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Models
{
    public class User
    {
        public long Id { get; set; }          // Telegram Chat ID
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }
}
