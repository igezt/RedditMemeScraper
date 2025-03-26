namespace RedditScraper.Services.RedditAuth;

public interface IRedditAuthService
{
    Task<string> GetAccessToken();
}
