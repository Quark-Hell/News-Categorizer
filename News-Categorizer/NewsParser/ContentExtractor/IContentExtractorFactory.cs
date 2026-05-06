using System;

namespace NewsParser.ContentExtractor
{
    public interface IContentExtractorFactory
    {
        IContentExtractor GetExtractor(string source);
    }
}
