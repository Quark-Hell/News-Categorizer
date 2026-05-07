using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.AIModels
{
    public class AiSummaryResponse
    {
        public List<AiSummaryResult> Results { get; set; } = [];
    }
}
