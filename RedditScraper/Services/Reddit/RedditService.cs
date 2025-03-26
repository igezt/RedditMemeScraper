using System.Net.Http.Headers;
using System.Text.Json;
using RedditScraper.Services.RedditClient;
using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.Reddit;

public class RedditService(IRedditClient redditClient) : IRedditService
{
    private readonly IRedditClient _client = redditClient;

    // redditClient = new RedditClient(
    //     appId: "gJcw8DfRPONB8cdgJZCVJw",
    //     appSecret: "URv08iiHxKRq4ZEY0pM1ZAyxnP0edQ",
    //     refreshToken: "133687568035914-eay99-XLDfP8FsgZjrrDuk-4QdNxbQ",
    //     userAgent: "RedditScrapper"
    // );

    public async Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count)
    {
        var posts = await _client.GetTopPostsInPastDay(subreddit, count);

        Console.WriteLine($"Top {count} posts from r/{subreddit}:");
        foreach (var post in posts)
        {
            var title = post.Title;
            var upvotes = post.Upvotes;
            Console.WriteLine(
                $"🔹 {title} (Upvotes: {upvotes})\n - Link: https://reddit.com{post.Permalink}\n - Thumbnail: {post.Thumbnail}"
            );
        }

        return posts;
    }
}
