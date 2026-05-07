using HtmlAgilityPack;

namespace NewsParser.ContentExtractor
{
    public class VCruContentExtractor : IContentExtractor
    {
        public string Extract(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Основной контейнер статьи
            var node = doc.DocumentNode.SelectSingleNode(
                "//div[contains(@class,'content')]"
            );

            if (node != null)
                return Clean(node.InnerText);

            // fallback: article
            node = doc.DocumentNode.SelectSingleNode("//article");

            if (node != null)
                return Clean(node.InnerText);

            // fallback: все параграфы
            var paragraphs = doc.DocumentNode.SelectNodes("//p");

            if (paragraphs != null)
            {
                var text = string.Join("\n",
                    paragraphs
                        .Select(p => Clean(p.InnerText))
                        .Where(t => !string.IsNullOrWhiteSpace(t) && t.Length > 20));

                if (!string.IsNullOrWhiteSpace(text))
                    return text;
            }

            return string.Empty;
        }

        private string Clean(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return HtmlEntity.DeEntitize(text)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                .Trim();
        }
    }
}