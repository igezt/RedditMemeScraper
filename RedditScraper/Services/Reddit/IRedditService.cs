using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.Reddit;

public interface IRedditService
{
    Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count);
}
