using System.ComponentModel.DataAnnotations;

namespace Scrapper.Server.Models
{
    public class SearchRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Keyword must be at least 3 characters long.")]
        public string Keyword { get; init; }

        [Required]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Url { get; init; }
    }
}
