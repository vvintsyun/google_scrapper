using Microsoft.AspNetCore.Mvc;
using Scrapper.Server.Contracts;
using Scrapper.Server.Models;

namespace Scrapper.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ScrapperController : ControllerBase
    {
        private readonly IScrapperService _service;

        public ScrapperController(IScrapperService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetSearchResults([FromQuery] SearchRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new
            {
                searchResults = await _service.GetResultsAsStringAsync(request, ct)
            };
            return Ok(result);
        }
    }
}
