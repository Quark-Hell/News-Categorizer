using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.ContentExtractor
{
    public class ContentExtractorFactory : IContentExtractorFactory
    {
        private readonly IServiceProvider _provider;

        public ContentExtractorFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IContentExtractor GetExtractor(string source)
        {
            return source switch
            {
                "Habr" => _provider.GetRequiredService<HabrContentExtractor>(),
                "RBK" => _provider.GetRequiredService<RBKContentExtractor>(),
                "Meduza" => _provider.GetRequiredService<MeduzaContentExtractor>(),
                _ => throw new NotSupportedException()
            };
        }
    }
}
