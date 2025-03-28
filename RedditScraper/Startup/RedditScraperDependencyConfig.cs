using RedditScraper.Services.Converter;
using RedditScraper.Services.Environment;
using RedditScraper.Services.Reddit;
using RedditScraper.Services.RedditAuth;
using RedditScraper.Services.RedditClient;
using RedditScraper.Services.RedditPosts;

namespace RedditScraper.Startup;

public static class RedditScraperDependencyConfig
{
    public static void AddRedditScraperDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IEnvService, EnvService>();
        services.AddSingleton<IRedditService, RedditService>();
        services.AddSingleton<IRedditAuthService, RedditAuthService>();
        services.AddSingleton<IRedditClient, RedditClient>();
        services.AddSingleton<IConverterService, ConverterService>();
        services.AddSingleton<IRedditPostRepo, RedditPostRepo>();
        services.AddSingleton<IRedditPostService, RedditPostService>();
    }
}
