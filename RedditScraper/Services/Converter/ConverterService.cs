using System.Threading.Tasks;
using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Converter.Adapters;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Converter;

public class ConverterService : IConverterService
{
    private readonly Dictionary<FileType, IAdapter> adapters = new()
    {
        { FileType.MARKDOWN, new MarkdownAdapter() },
        { FileType.HTML, new HtmlAdapter() },
        { FileType.PDF, new PdfAdapter() },
    };

    public async Task<string> Convert(
        FileType outputFileType,
        List<RedditPost> posts,
        string fileName
    )
    {
        if (!adapters.TryGetValue(outputFileType, out var adapter))
        {
            throw new InvalidOperationException(
                $"The file type {outputFileType} does not have an adapter configured to it."
            );
        }

        // var fileName = DateTime.UtcNow.ToString("dd-MM-yyyy_HHmmss");

        return await adapter.Adapt(posts, fileName);
    }
}
