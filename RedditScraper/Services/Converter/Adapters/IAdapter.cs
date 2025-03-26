using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.Converter.Adapters;

public interface IAdapter
{
    string Adapt(List<RedditPost> posts, string fileName);
}
