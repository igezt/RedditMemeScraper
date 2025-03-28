using System.Text;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RedditScraper.Services.RedditPosts.Models;

public class RedditPost
{
    [JsonPropertyName("id")]
    [BsonElement("reddit_id")]
    public string RedditId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("subreddit")]
    public string Subreddit { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("ups")]
    public int Upvotes { get; set; }

    [JsonPropertyName("downs")]
    public int Downvotes { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("num_comments")]
    public int NumComments { get; set; }

    [JsonPropertyName("created_utc")]
    public double CreatedUtc { get; set; }

    [JsonIgnore] // Computed property to convert UNIX timestamp to DateTime
    public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds((long)CreatedUtc).UtcDateTime;

    [JsonPropertyName("permalink")]
    public string Permalink { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("selftext")]
    public string SelfText { get; set; } = string.Empty;

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; } = string.Empty;

    public string ToMarkdownTable()
    {
        var sb = new StringBuilder();

        sb.AppendLine("| Property     | Value |");
        sb.AppendLine("|--------------|-------|");
        sb.AppendLine($"| `Meme`        | ![MemeImage]({Url}) |");
        sb.AppendLine($"| `Id`         | {RedditId} |");
        sb.AppendLine($"| `Title`      | {Title} |");
        sb.AppendLine($"| `Subreddit`  | {Subreddit} |");
        sb.AppendLine($"| `Author`     | {Author} |");
        sb.AppendLine($"| `Upvotes`    | {Upvotes} |");
        sb.AppendLine($"| `Downvotes`  | {Downvotes} |");
        sb.AppendLine($"| `Score`      | {Score} |");
        sb.AppendLine($"| `NumComments`| {NumComments} |");
        sb.AppendLine($"| `CreatedUtc` | {CreatedUtc} |");
        sb.AppendLine($"| `CreatedAt`  | {CreatedAt} |");
        sb.AppendLine($"| `Permalink`  | https://reddit.com{Permalink} |");
        sb.AppendLine($"| `SelfText`   | {SelfText} |");
        sb.AppendLine($"| `Thumbnail`  | {Thumbnail} |");

        return sb.ToString();
    }

    public string ToHtml()
    {
        var sb = new StringBuilder();

        sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
        sb.AppendLine("<tr><th>Property</th><th>Value</th></tr>");
        sb.AppendLine(
            $"<tr><td><strong>Meme</strong></td><td><img src='{Url}' width='300' height='200'/></td></tr>"
        );
        sb.AppendLine($"<tr><td><strong>Id</strong></td><td>{RedditId}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Title</strong></td><td>{Title}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Subreddit</strong></td><td>{Subreddit}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Author</strong></td><td>{Author}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Upvotes</strong></td><td>{Upvotes}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Downvotes</strong></td><td>{Downvotes}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Score</strong></td><td>{Score}</td></tr>");
        sb.AppendLine($"<tr><td><strong>NumComments</strong></td><td>{NumComments}</td></tr>");
        sb.AppendLine($"<tr><td><strong>CreatedUtc</strong></td><td>{CreatedUtc}</td></tr>");
        sb.AppendLine($"<tr><td><strong>CreatedAt</strong></td><td>{CreatedAt}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Permalink</strong></td><td>{Permalink}</td></tr>");
        sb.AppendLine($"<tr><td><strong>SelfText</strong></td><td>{SelfText}</td></tr>");
        sb.AppendLine($"<tr><td><strong>Thumbnail</strong></td><td>{Thumbnail}</td></tr>");
        sb.AppendLine("</table>");

        return sb.ToString();
    }
}
