using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Converter;

public interface IConverterService
{
    public Task<string> Convert(FileType outputFileType, List<RedditPost> posts);
}
