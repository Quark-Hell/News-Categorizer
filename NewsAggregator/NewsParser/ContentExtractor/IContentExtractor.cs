using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.ContentExtractor
{
    public interface IContentExtractor
    {
        string Extract(string html);
    }
}
