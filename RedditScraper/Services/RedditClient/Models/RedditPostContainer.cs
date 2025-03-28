using System.Text.Json.Serialization;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.RedditClient.Models;

public class RedditPostContainer
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("data")]
    public RedditPost Data { get; set; }
}
