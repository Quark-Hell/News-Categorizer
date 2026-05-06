using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.ContentLoader
{
    public interface IContentLoader
    {
        Task<string?> LoadContentAsync(string url);
    }
}
