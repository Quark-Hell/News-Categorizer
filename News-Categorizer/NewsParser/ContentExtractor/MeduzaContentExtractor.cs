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

            // 1. try article
            var article = doc.DocumentNode.SelectSingleNode("//article");
            if (article != null)
                return article.InnerText.Trim();

            // 2. fallback paragraphs
            var paragraphs = doc.DocumentNode.SelectNodes("//p");
            if (paragraphs != null)
            {
                return string.Join("\n",
                    paragraphs.Select(p => p.InnerText.Trim())
                              .Where(t => !string.IsNullOrWhiteSpace(t)));
            }

            return string.Empty;
        }
    }
}
