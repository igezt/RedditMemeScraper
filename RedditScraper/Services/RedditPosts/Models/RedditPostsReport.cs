using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RedditScraper.Services.RedditPosts.Models;

public class RedditPostsReport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("redditPosts")]
    public List<RedditPost> RedditPosts = [];

    [BsonElement("date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Date;

    [BsonElement("subreddit")]
    public string Subreddit { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"RedditPostsReport:\n"
            + $"ID: {Id}\n"
            + $"Subreddit: {Subreddit}\n"
            + $"Date: {Date:yyyy-MM-dd HH:mm:ss} UTC\n"
            + $"Reddit Posts Count: {RedditPosts.Count}\n";
        // + $"Posts:\n{string.Join("\n", RedditPosts)}";
    }
}
