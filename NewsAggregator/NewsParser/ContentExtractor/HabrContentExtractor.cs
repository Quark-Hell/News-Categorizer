using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace NewsParser.ContentExtractor
{
    public class HabrContentExtractor : IContentExtractor
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
