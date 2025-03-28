using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.RedditPosts;

public interface IRedditPostService
{
    Task<bool> UpsertRedditPosts(DateTime date, List<RedditPost> posts);
    Task<List<DateTime>> GetDatesWithReports(int count);
    Task<RedditPostsReport> GetReportByDate(string subreddit, DateTime date);
}
