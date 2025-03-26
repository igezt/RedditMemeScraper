using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.Reddit;

public interface IRedditService
{
    string ConvertToFile(FileType outputFileType, List<RedditPost> posts);
    Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count);
}
