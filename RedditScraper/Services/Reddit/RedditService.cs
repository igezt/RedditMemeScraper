using System.Net.Http.Headers;
using System.Text.Json;
using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Converter;
using RedditScraper.Services.RedditClient;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Reddit;

public class RedditService(
    IRedditClient redditClient,
    IConverterService converterService,
    IRedditPostService redditPostService
) : IRedditService
{
    private readonly IRedditClient _client = redditClient;
    private readonly IConverterService _converter = converterService;
    private readonly IRedditPostService _redditPostService = redditPostService;

    public async Task<string> ConvertToFile(
        FileType outputFileType,
        List<RedditPost> posts,
        string fileName
    )
    {
        return await _converter.Convert(outputFileType, posts, fileName);
    }

    public async Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count)
    {
        var posts = await _client.GetTopPostsInPastDay(subreddit, count);

        await _redditPostService.UpsertRedditPosts(DateTime.Now.Date, posts);

        return posts;
    }

    public async Task<RedditPostsReport> GetTopPostsOnSpecificDay(string subreddit, DateTime date)
    {
        return await _redditPostService.GetReportByDate(subreddit, date);
    }

    public async Task<List<DateTime>> GetDatesWithTopPostsRegistered(int count)
    {
        return await _redditPostService.GetDatesWithReports(count);
    }
}
