using System.Net.Http.Headers;
using System.Text.Json;
using RedditScraper.Services.RedditAuth;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.RedditClient;

public class RedditClient(HttpClient httpClient, IRedditAuthService authService) : IRedditClient
{
    private readonly HttpClient _client = httpClient;
    private readonly IRedditAuthService _authService = authService;

    public async Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count)
    {
        var accessToken = await _authService.GetAccessToken();

        // ✅ Set authorization header for Reddit API
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            accessToken
        );
        _client.DefaultRequestHeaders.Add("User-Agent", "RedditScraper/1.0");

        var url = $"https://oauth.reddit.com/r/{subreddit}/top?limit={count}&t=day";
        var response = await _client.GetAsync(url);
        var responseData = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidDataException(
                $"API Call to the {subreddit} subreddit to retrieve the top {count} posts was unsuccessful"
            );
        }

        var redditResponse =
            JsonSerializer.Deserialize<RedditApiResponse>(responseData)
            ?? throw new InvalidDataException(
                $"Deserializing the data from the api response returned null"
            );
        return redditResponse.Data.Children.Select(container => container.Data).ToList();
    }
}
