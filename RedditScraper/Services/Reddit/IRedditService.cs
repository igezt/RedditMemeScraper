using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.RedditClient.Models;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Reddit;

/// <summary>
/// Interface for interacting with Reddit services such as retrieving and converting Reddit posts.
/// </summary>
public interface IRedditService
{
    /// <summary>
    /// Converts a list of Reddit posts into the specified file format and returns the file path.
    /// </summary>
    /// <param name="outputFileType">The type of the output file (e.g., Markdown, PDF, HTML).</param>
    /// <param name="posts">The list of Reddit posts to include in the file.</param>
    /// <param name="fileName">The name to assign to the generated file.</param>
    /// <returns>Returns the file path of the generated file.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid file type is provided.</exception>
    Task<string> ConvertToFile(FileType outputFileType, List<RedditPost> posts, string fileName);

    /// <summary>
    /// Retrieves the top Reddit posts from a specific subreddit for the past day.
    /// </summary>
    /// <param name="subreddit">The name of the subreddit from which to retrieve posts.</param>
    /// <param name="count">The number of top posts to retrieve.</param>
    /// <returns>Returns a list of the top Reddit posts from the specified subreddit.</returns>
    /// <exception cref="RedditException">Thrown if there's an issue fetching posts from Reddit.</exception>
    Task<List<RedditPost>> GetTopPostsInPastDay(string subreddit, int count);

    /// <summary>
    /// Retrieves the top Reddit posts from a specific subreddit on a given date.
    /// </summary>
    /// <param name="subreddit">The name of the subreddit from which to retrieve posts.</param>
    /// <param name="date">The specific date for which to retrieve posts.</param>
    /// <returns>Returns a report containing the top Reddit posts for the given date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date is in the future or invalid.</exception>
    Task<RedditPostsReport> GetTopPostsOnSpecificDay(string subreddit, DateTime date);

    // <summary>
    /// Retrieves a list of dates for which top Reddit posts have been registered.
    /// </summary>
    /// <param name="count">The number of dates to retrieve.</param>
    /// <returns>Returns a list of dates for which top posts are available.</returns>
    /// <exception cref="ArgumentException">Thrown when the count parameter is invalid.</exception>
    Task<List<DateTime>> GetDatesWithTopPostsRegistered(int count);
}
