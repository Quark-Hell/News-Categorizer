using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.AIModels
{
    public class AiTopicResult
    {
        public string Name { get; set; } = string.Empty;

        public double Score { get; set; }
    }
}
