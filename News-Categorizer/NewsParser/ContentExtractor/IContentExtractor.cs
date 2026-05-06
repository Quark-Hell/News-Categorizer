using System;

namespace NewsParser.ContentExtractor
{
    public interface IContentExtractor
    {
        string Extract(string html);
    }
}
