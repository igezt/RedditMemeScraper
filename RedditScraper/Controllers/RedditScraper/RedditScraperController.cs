using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Reddit;

namespace RedditScraper.Controllers.RedditScraper;

public class RedditScraperController(IRedditService redditService) : Controller
{
    // GET: RedditScraperController
    [HttpGet("past-day")]
    public async Task<IActionResult> GetRedditPostsFromPastDay(
        string subreddit = "memes",
        int count = 20,
        FileType outputFileType = FileType.MARKDOWN
    )
    {
        var posts = await redditService.GetTopPostsInPastDay(subreddit, count);
        return Json(new { });
    }
}
