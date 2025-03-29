using Moq.Protected;
using Moq;
using System.Net;
using Microsoft.Extensions.Logging;
using Scrapper.Server.Services;
using Scrapper.Server.Models;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task NonSearchSectionsAreIgnored()
        {
            var htmlContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../TestData", "chelsea search.html"));

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(htmlContent)
                });
            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.google.co.uk")
            };

            var logger = Mock.Of<ILogger<ScrapperService>>();
            var service = new ScrapperService(logger, client);

            var instagramResult = await service.GetResultsAsStringAsync(new SearchRequest { Keyword = "chelsea", Url = "https://www.instagram.com/chelseafc/" }, CancellationToken.None);
            var websiteResult = await service.GetResultsAsStringAsync(new SearchRequest { Keyword = "chelsea", Url = "https://www.chelseafc.com/" }, CancellationToken.None);

            Assert.Equal("2", instagramResult);
            Assert.Equal("1", websiteResult);
        }

        [Fact]
        public async Task AbsentWebsiteReturns0()
        {
            var htmlContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../TestData", "land registry.html"));

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(htmlContent)
                });
            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.google.co.uk")
            };

            var logger = Mock.Of<ILogger<ScrapperService>>();
            var service = new ScrapperService(logger, client);

            var absentResult = await service.GetResultsAsStringAsync(new SearchRequest { Keyword = "land registry search", Url = "https://www.mccaskiebutcher.co.uk/" }, CancellationToken.None);

            Assert.Equal("0", absentResult);
        }
        
        [Fact]
        public async Task InfotrackWebsiteReturns2Results()
        {
            var htmlContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../TestData", "land registry.html"));

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(htmlContent)
                });
            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.google.co.uk")
            };

            var logger = Mock.Of<ILogger<ScrapperService>>();
            var service = new ScrapperService(logger, client);

            var results = await service.GetResultsAsStringAsync(new SearchRequest { Keyword = "land registry search", Url = "https://www.infotrack.co.uk/" }, CancellationToken.None);

            Assert.Equal("14, 98", results);
        }
    }
}