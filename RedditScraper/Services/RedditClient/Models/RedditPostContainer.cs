using System.Text.Json.Serialization;

namespace RedditScraper.Services.RedditClient.Models;

public class RedditPostContainer
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("data")]
    public RedditPost Data { get; set; }
}
