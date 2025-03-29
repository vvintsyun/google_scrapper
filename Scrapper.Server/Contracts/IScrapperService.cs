using Scrapper.Server.Models;

namespace Scrapper.Server.Contracts
{
    public interface IScrapperService
    {
        Task<string> GetResultsAsStringAsync(SearchRequest request, CancellationToken cancellationToken);
    }
}
