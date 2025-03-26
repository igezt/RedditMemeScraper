using System.Net.Http.Headers;
using System.Text.Json;
using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Converter;
using RedditScraper.Services.RedditClient;
using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.Reddit;

public class RedditService(IRedditClient redditClient, IConverterService converterService)
    : IRedditService
{
    private readonly IRedditClient _client = redditClient;
    private readonly IConverterService _converter = converterService;

    public string ConvertToFile(FileType outputFileType, List<RedditPost> posts)
    {
        return _converter.Convert(outputFileType, posts);
    }

    public async Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count)
    {
        var posts = await _client.GetTopPostsInPastDay(subreddit, count);

        // Console.WriteLine($"Top {count} posts from r/{subreddit}:");
        // foreach (var post in posts)
        // {
        //     var title = post.Title;
        //     var upvotes = post.Upvotes;
        //     Console.WriteLine(
        //         $"🔹 {title} (Upvotes: {upvotes})\n - Link: https://reddit.com{post.Permalink}\n - Thumbnail: {post.Thumbnail}"
        //     );
        // }

        return posts;
    }
}
