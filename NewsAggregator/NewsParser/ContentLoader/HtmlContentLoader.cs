using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsParser.ContentLoader
{
    public class HtmlContentLoader : IContentLoader
    {
        private readonly HttpClient _http;

        public HtmlContentLoader(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> LoadContentAsync(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                request.Headers.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                request.Headers.Accept.ParseAdd("text/html");
                request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");

                var response = await _http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    // можно залогировать
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
