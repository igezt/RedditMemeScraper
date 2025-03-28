using MongoDB.Driver;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.RedditPosts;

public class RedditPostService(IRedditPostRepo redditPostRepo) : IRedditPostService
{
    private readonly IRedditPostRepo _redditPostRepo = redditPostRepo;

    public async Task<List<DateTime>> GetDatesWithReports(int count)
    {
        // Get all reports from the database
        var reports = await _redditPostRepo.GetReports(Builders<RedditPostsReport>.Filter.Empty);

        // Extract unique dates, sort them from latest to earliest, and take only the given count
        return reports
            .Select(report => report.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .Take(count)
            .ToList();
    }

    public async Task<RedditPostsReport> GetReportByDate(string subreddit, DateTime date)
    {
        var dateFilter = Builders<RedditPostsReport>.Filter.Eq(r => r.Date.Date, date.Date);
        var subredditFilter = Builders<RedditPostsReport>.Filter.Eq(r => r.Subreddit, subreddit);
        var filter = Builders<RedditPostsReport>.Filter.And(dateFilter, subredditFilter);
        return (await _redditPostRepo.GetReports(filter)).First();
    }

    public Task<bool> UpsertRedditPosts(DateTime date, List<RedditPost> posts)
    {
        var filter = Builders<RedditPostsReport>.Filter.Eq(r => r.Date, date.Date);

        var update = Builders<RedditPostsReport>
            .Update.Set(r => r.RedditPosts, posts)
            .Set(r => r.Date, date.Date)
            .Set(r => r.Subreddit, posts.First().Subreddit);

        var options = new UpdateOptions { IsUpsert = true };
        return _redditPostRepo.UpsertReport(filter, update, options);
    }
}
