using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Reddit;

public interface IRedditService
{
    Task<string> ConvertToFile(FileType outputFileType, List<RedditPost> posts);
    Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count);
    Task<RedditPostsReport> GetTopPostsOnSpecificDay(string subreddit, DateTime date);
    Task<List<DateTime>> GetDatesWithTopPostsRegistered(int count);
}
