using System.Text.Json.Serialization;

namespace RedditScraper.Services.RedditClient.Models;

public class RedditApiResponse
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("data")]
    public RedditListingData Data { get; set; }
}
