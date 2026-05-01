using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.ContentExtractor
{
    public interface IContentExtractorFactory
    {
        IContentExtractor GetExtractor(string source);
    }
}
