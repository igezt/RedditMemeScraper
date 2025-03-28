using MongoDB.Driver;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.RedditPosts;

public interface IRedditPostRepo
{
    Task<List<RedditPostsReport>> GetReports(FilterDefinition<RedditPostsReport> filter);
    Task<bool> UpsertReport(
        FilterDefinition<RedditPostsReport> filter,
        UpdateDefinition<RedditPostsReport> update,
        UpdateOptions options
    );
}
