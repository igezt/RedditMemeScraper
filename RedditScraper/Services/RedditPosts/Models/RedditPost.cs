using System.Text;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RedditScraper.Services.RedditPosts.Models;

/// <summary>
/// Represents a Reddit post and its relevant properties.
/// Contains properties such as title, author, score, and creation timestamp,
/// as well as methods for converting the data to various formats like Markdown, HTML, and plain text.
/// </summary>
public class RedditPost
{
    /// <summary>
    /// A unique identifier for the Reddit post.
    /// </summary>
    [JsonPropertyName("id")]
    [BsonElement("reddit_id")]
    public string RedditId { get; set; } = string.Empty;

    /// <summary>
    /// The title of the Reddit post.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The name of the subreddit where the post was made.
    /// </summary>
    [JsonPropertyName("subreddit")]
    public string Subreddit { get; set; } = string.Empty;

    /// <summary>
    /// The Reddit username of the post's author.
    /// </summary>
    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// The number of upvotes the post has received.
    /// </summary>
    [JsonPropertyName("ups")]
    public int Upvotes { get; set; }

    /// <summary>
    /// The number of downvotes the post has received.
    /// </summary>
    [JsonPropertyName("downs")]
    public int Downvotes { get; set; }

    /// <summary>
    /// The total score of the post (upvotes minus downvotes).
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; }

    /// <summary>
    /// The number of comments on the post.
    /// </summary>
    [JsonPropertyName("num_comments")]
    public int NumComments { get; set; }

    /// <summary>
    /// The UTC timestamp (in seconds) when the post was created.
    /// </summary>
    [JsonPropertyName("created_utc")]
    public double CreatedUtc { get; set; }

    /// <summary>
    /// A computed property that converts the UNIX timestamp (`CreatedUtc`) into a human-readable DateTime.
    /// </summary>
    [JsonIgnore] // Computed property to convert UNIX timestamp to DateTime
    public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds((long)CreatedUtc).UtcDateTime;

    /// <summary>
    /// The permalink (URL) of the Reddit post.
    /// </summary>
    [JsonPropertyName("permalink")]
    public string Permalink { get; set; } = string.Empty;

    /// <summary>
    /// The URL to the image or content in the post.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// The textual content of the post, if any (self-post).
    /// </summary>
    [JsonPropertyName("selftext")]
    public string SelfText { get; set; } = string.Empty;

    /// <summary>
    /// The URL to the thumbnail image of the post.
    /// </summary>
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

    public override string ToString()
    {
        return $"RedditPost:\n"
            + $"ID: {RedditId}\n"
            + $"Title: {Title}\n"
            + $"Subreddit: {Subreddit}\n"
            + $"Author: {Author}\n"
            + $"Upvotes: {Upvotes}, Downvotes: {Downvotes}, Score: {Score}\n"
            + $"Comments: {NumComments}\n"
            + $"Created: {CreatedAt:yyyy-MM-dd HH:mm:ss} UTC\n"
            + $"Permalink: https://reddit.com{Permalink}\n"
            + $"Image URL: {Url}\n"
            + $"Thumbnail: {Thumbnail}\n"
            + $"SelfText: {(string.IsNullOrWhiteSpace(SelfText) ? "N/A" : SelfText.Substring(0, Math.Min(100, SelfText.Length)) + (SelfText.Length > 100 ? "..." : ""))}";
    }
}
