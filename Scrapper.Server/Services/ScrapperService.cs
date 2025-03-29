using HtmlAgilityPack;
using Scrapper.Server.Contracts;
using Scrapper.Server.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Scrapper.Server.Services
{
    public class ScrapperService : IScrapperService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ScrapperService> _logger;
        private const string defaultResponse = "0";

        public ScrapperService(ILogger<ScrapperService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string> GetResultsAsStringAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var searchQuery = HttpUtility.UrlEncode(request.Keyword);
            var url = $"search?num=100&q={searchQuery}";

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error reading request with keyword {Keyword}: {Code}", request.Keyword, response.StatusCode);
                return defaultResponse;
            }

            var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

            var positions = ExtractPositionsAsString(htmlContent, request.Url);

            return positions;
        }

        //to use cookies paste its value into `cookies` variable
        //public async Task<string> GetResultsAsStringAsync(SearchRequest request, CancellationToken cancellationToken)
        //{
        //    var searchQuery = HttpUtility.UrlEncode(request.Keyword);
        //    var url = $"https://www.google.co.uk/search?num=100&q={searchQuery}";

        //    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

        //    var handler = new HttpClientHandler();
        //    handler.CookieContainer = new CookieContainer();

        //    const string cookies = "paste your value here";
        //    AddCookiesFromString(handler.CookieContainer, cookies);

        //    _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
        //    var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        _logger.LogError("Error reading request with keyword {Keyword}: {Code}", request.Keyword, response.StatusCode);
        //        return defaultResponse;
        //    }

        //    var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

        //    var positions = ExtractPositionsAsString(htmlContent, request.Url);

        //    return positions;
        //}

        //private void AddCookiesFromString(CookieContainer cookieContainer, string cookieString)
        //{
        //    var cookies = cookieString.Split(';');

        //    foreach (var cookie in cookies)
        //    {
        //        var parts = cookie.Split('=', 2, StringSplitOptions.TrimEntries);
        //        if (parts.Length == 2)
        //        {
        //            string name = parts[0];
        //            string value = parts[1];
        //            cookieContainer.Add(new Cookie(name, value, "/", "www.google.co.uk"));
        //        }
        //    }
        //}

        private string ExtractPositionsAsString(string html, string targetUrl)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var searchResults = doc.DocumentNode.SelectNodes("//div[@id='search']//a[@data-ved][@href][h3][not(ancestor::g-section-with-header) and not(ancestor::*[@role='complementary'])]/@href"); 

            var sb = new StringBuilder();
            if (searchResults is null) return defaultResponse;

            var index = 1;
            foreach (var result in searchResults)
            {
                var resultUrl = result.GetAttributeValue("href", "");
                if (resultUrl.StartsWith(targetUrl.Trim(), System.StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append($"{index}, ");
                }
                index++;
            }
            return sb.Length > 0
                ? sb.ToString(0, sb.Length - 2)
                : defaultResponse;
        }
    }
}
