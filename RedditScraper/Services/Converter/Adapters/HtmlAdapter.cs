using System.Text;
using Markdig;
using Markdig.Renderers;
using RedditScraper.Services.RedditClient.Models;

namespace RedditScraper.Services.Converter.Adapters;

public class HtmlAdapter : IAdapter
{
    private static readonly string OUTPUT_FILE_PATH = "./Services/Converter/Output/Html";

    public string Adapt(List<RedditPost> posts, string fileName)
    {
        var html = new StringBuilder();

        html.AppendLine("<html>");
        AddStyle(html);
        html.AppendLine("<body>");
        html.AppendLine(
            string.Join(
                "\n\n",
                posts.Select(
                    (post, i) => $"<h1>Reddit Post {i + 1} Details</h1> \n {post.ToHtml()}"
                )
            )
        );
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        var currentDateTime = DateTime.Now.ToString();

        var outputFilePath = $"{OUTPUT_FILE_PATH}/{fileName}.html";

        File.WriteAllText(outputFilePath, html.ToString());

        return outputFilePath;
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
        html.AppendLine("</style>");
    }
}
