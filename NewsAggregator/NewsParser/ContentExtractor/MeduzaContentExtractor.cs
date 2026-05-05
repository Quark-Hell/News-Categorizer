using HtmlAgilityPack;
using System;

namespace NewsParser.ContentExtractor
{
    public class MeduzaContentExtractor : IContentExtractor
    {
        public string Extract(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var node = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class,'article-formatted-body')]");

            return node?.InnerText.Trim() ?? string.Empty;
        }
    }
}
