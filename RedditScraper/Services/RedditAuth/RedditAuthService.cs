using System.Net.Http.Headers;
using RedditScraper.Helper.Environment.Enums;
using RedditScraper.Services.Environment;

namespace RedditScraper.Services.RedditAuth;

public class RedditAuthService(HttpClient client, IEnvService envService) : IRedditAuthService
{
    private static readonly string REDDIT_ACCESS_TOKEN_API_URL =
        "https://www.reddit.com/api/v1/access_token";

    private readonly string CLIENT_ID = envService.GetEnvVariable(EnvVariableKeys.REDDIT_CLIENT_ID);
    private readonly string CLIENT_SECRET = envService.GetEnvVariable(
        EnvVariableKeys.REDDIT_CLIENT_SECRET
    );
    private readonly HttpClient _client = client;

    public async Task<string> GetAccessToken()
    {
        var authToken = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes($"{CLIENT_ID}:{CLIENT_SECRET}")
        );

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            authToken
        );
        _client.DefaultRequestHeaders.Add("User-Agent", "RedditScraper/1.0");

        var content = new FormUrlEncodedContent(
            [new KeyValuePair<string, string>("grant_type", "client_credentials")]
        );

        var response = await _client.PostAsync(REDDIT_ACCESS_TOKEN_API_URL, content);
        var responseData = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidDataException("Access token was not obtained.");
        }

        var json = System.Text.Json.JsonDocument.Parse(responseData);
        return json.RootElement.GetProperty("access_token").GetString()
            ?? throw new InvalidDataException("The access token is null");
    }
}
