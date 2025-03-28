using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Converter.Adapters;

public interface IAdapter
{
    Task<string> Adapt(List<RedditPost> posts, string fileName);
}
