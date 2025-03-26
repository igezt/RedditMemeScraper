using System.Text.Json;
using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.RedditClient;

public interface IRedditClient
{
    Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count);
}
