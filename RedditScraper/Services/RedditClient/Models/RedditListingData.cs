using System.Text.Json.Serialization;

namespace RedditScraper.Services.RedditClient.Models;

public class RedditListingData
{
    [JsonPropertyName("children")]
    public List<RedditPostContainer> Children { get; set; }
}
