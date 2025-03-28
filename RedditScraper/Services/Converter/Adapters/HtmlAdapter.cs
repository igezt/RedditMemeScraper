using System.Text;
using Markdig;
using Markdig.Renderers;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Converter.Adapters;

public class HtmlAdapter : IAdapter
{
    private static readonly string OUTPUT_FILE_PATH = "./Output/Html";

    private static readonly HtmlAdapter _singleton = new();

    public static HtmlAdapter Create()
    {
        return _singleton;
    }

    public async Task<string> Adapt(List<RedditPost> posts, string fileName)
    {
        var htmlString = ConvertToHtmlString(posts);

        Directory.CreateDirectory(OUTPUT_FILE_PATH);

        var outputFilePath = $"{OUTPUT_FILE_PATH}/{fileName}.html";

        File.WriteAllText(outputFilePath, htmlString);

        return outputFilePath;
    }

    public static string ConvertToHtmlString(List<RedditPost> posts, bool withPageBreaks = false)
    {
        var html = new StringBuilder();

        html.AppendLine("<html>");
        AddStyle(html);
        html.AppendLine("<body>");
        html.AppendLine(
            string.Join(
                withPageBreaks ? "<div break-after: page></div>" : "\n\n",
                posts.Select(
                    (post, i) => $"<h1>Reddit Post {i + 1} Details</h1> \n {post.ToHtml()}"
                )
            )
        );

        AddRankByCommentsTable(html, posts);

        AddRankByTimeTable(html, posts);

        html.AppendLine("</body>");
        html.AppendLine("</html>");
        return html.ToString();
    }

    private static void AddStyle(StringBuilder html)
    {
        html.AppendLine("<style>");
        html.AppendLine(
            "body { font-family: Arial, sans-serif; margin: 20px; background-color: #f4f4f4; color: #333; padding: 40px; }"
        );
        html.AppendLine("h1 { color: #ff4500; text-align: center; }");
        html.AppendLine("table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
        html.AppendLine("th, td { padding: 10px; text-align: left; border: 1px solid #ddd; }");
        html.AppendLine("th { background-color: #ff4500; color: white; }");
        html.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
        html.AppendLine("tr:hover { background-color: #ddd; }");
        html.AppendLine("td a { color: #ff4500; text-decoration: none; }");
        html.AppendLine("td a:hover { text-decoration: underline; }");
        html.AppendLine(
            @"
        td, th {
            word-wrap: break-word;
            white-space: normal; /* Ensure that long words are broken and wrapped */
        }
        "
        );
        html.AppendLine("</style>");
        html.AppendLine("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");
    }

    private static void AddRankByCommentsTable(StringBuilder html, List<RedditPost> posts)
    {
        var sortedPosts = posts.OrderByDescending(post => post.NumComments).ToList();

        html.AppendLine("<h1>Top 20 Memes Ranked by Number of Comments</h1>");

        html.AppendLine("<table>");
        html.AppendLine(
            "<tr><th>Rank</th><th>Title</th><th>Author</th><th>Subreddit</th><th>Upvotes</th><th>Downvotes</th><th>Score</th><th>Comments</th><th>Post Link</th></tr>"
        );

        // Generate a row for each post
        int rank = 1;
        foreach (var post in sortedPosts)
        {
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{rank}</td>");
            html.AppendLine($"<td>{post.Title}</td>");
            html.AppendLine($"<td>{post.Author}</td>");
            html.AppendLine($"<td>{post.Subreddit}</td>");
            html.AppendLine($"<td>{post.Upvotes}</td>");
            html.AppendLine($"<td>{post.Downvotes}</td>");
            html.AppendLine($"<td>{post.Score}</td>");
            html.AppendLine($"<td>{post.NumComments}</td>");
            html.AppendLine($"<td><a href='https://reddit.com{post.Permalink}'>View Post</a></td>");
            html.AppendLine("</tr>");
            rank++;
        }

        html.AppendLine("</table>");
    }

    private static void AddRankByTimeTable(StringBuilder html, List<RedditPost> posts)
    {
        // Sort posts by creation time (ascending)
        var sortedPosts = posts
            .OrderBy(post => ConvertUtcToSingaporeTime(post.CreatedAt).TimeOfDay)
            .ToList();

        html.AppendLine("<h1>Reddit Posts Organized by Time Created</h1>");

        html.AppendLine("<table>");
        html.AppendLine(
            "<tr><th>Rank</th><th>Title</th><th>Author</th><th>Subreddit</th><th>Upvotes</th><th>Downvotes</th><th>Score</th><th>Comments</th><th>Time Created</th><th>Post Link</th></tr>"
        );

        // Generate a row for each post
        int rank = 1;
        foreach (var post in sortedPosts)
        {
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{rank}</td>");
            html.AppendLine($"<td>{post.Title}</td>");
            html.AppendLine($"<td>{post.Author}</td>");
            html.AppendLine($"<td>{post.Subreddit}</td>");
            html.AppendLine($"<td>{post.Upvotes}</td>");
            html.AppendLine($"<td>{post.Downvotes}</td>");
            html.AppendLine($"<td>{post.Score}</td>");
            html.AppendLine($"<td>{post.NumComments}</td>");
            html.AppendLine(
                $"<td>{ConvertUtcToSingaporeTime(post.CreatedAt).ToString("HH:mm:ss")}</td>"
            );
            html.AppendLine($"<td><a href='https://reddit.com{post.Permalink}'>View Post</a></td>");
            html.AppendLine("</tr>");
            rank++;
        }

        html.AppendLine("</table>");
    }

    private static DateTime ConvertUtcToSingaporeTime(DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(
            utcTime,
            TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time")
        );
    }
}
