using MongoDB.Driver;
using RedditScraper.Helper.Environment.Enums;
using RedditScraper.Services.Environment;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.RedditPosts;

public class RedditPostRepo : IRedditPostRepo
{
    private static readonly string DATABASE_NAME = "RedditMemeScraper";
    private static readonly string COLLECTION_NAME = "RedditPostReports";
    private readonly IMongoCollection<RedditPostsReport> _collection;

    public RedditPostRepo(IEnvService envService)
    {
        var connectionString = envService.GetEnvVariable(EnvVariableKeys.MONGO_CONNECTION);
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(DATABASE_NAME);
        _collection = database.GetCollection<RedditPostsReport>(COLLECTION_NAME);
    }

    public async Task<List<RedditPostsReport>> GetReports(
        FilterDefinition<RedditPostsReport> filter
    )
    {
        var reports = (await _collection.FindAsync(filter)).ToList();
        return reports;
    }

    public async Task<bool> UpsertReport(
        FilterDefinition<RedditPostsReport> filter,
        UpdateDefinition<RedditPostsReport> update,
        UpdateOptions options
    )
    {
        var result = await _collection.UpdateOneAsync(filter, update, options);

        return result.IsAcknowledged && (result.ModifiedCount > 0 || result.UpsertedId != null);
    }
}
