using System;

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
                "VC.ru" => _provider.GetRequiredService<VCruContentExtractor>(),
                "Meduza" => _provider.GetRequiredService<MeduzaContentExtractor>(),
                _ => throw new NotSupportedException()
            };
        }
    }
}
