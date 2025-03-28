using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Converter.Adapters;

public class MarkdownAdapter : IAdapter
{
    private static readonly string OUTPUT_FILE_PATH = "./Output/Markdown";

    private static readonly MarkdownAdapter _singleton = new();

    public static MarkdownAdapter Create()
    {
        return _singleton;
    }

    public async Task<string> Adapt(List<RedditPost> posts, string fileName)
    {
        var markdownTables = string.Join(
            "\n\n",
            posts.Select((post, i) => $"# Meme {i + 1} \n {post.ToMarkdownTable()}")
        );

        var currentDateTime = DateTime.Now.ToString();

        Directory.CreateDirectory(OUTPUT_FILE_PATH);

        var outputFilePath = $"{OUTPUT_FILE_PATH}/{fileName}.md";

        File.WriteAllText(outputFilePath, markdownTables);

        return outputFilePath;
    }
}
