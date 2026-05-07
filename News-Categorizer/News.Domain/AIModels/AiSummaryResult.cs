using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.AIModels
{
    public class AiSummaryResult
    {
        public int Id { get; set; }

        public string Summary { get; set; } = string.Empty;

        public List<AiTopicResult> Topics { get; set; } = [];
    }
}
