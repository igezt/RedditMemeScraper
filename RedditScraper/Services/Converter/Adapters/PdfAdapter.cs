using System.IO;
using System.Text;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using RedditScraper.Services.RedditPosts.Models;

namespace RedditScraper.Services.Converter.Adapters;

public class PdfAdapter : IAdapter
{
    private static readonly string OUTPUT_FILE_PATH = "./Output/Pdf";

    private static HttpClient client;

    private static readonly PdfAdapter _singleton = new();

    public static PdfAdapter Create()
    {
        return _singleton;
    }

    public PdfAdapter()
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3"
        );
    }

    public async Task<string> Adapt(List<RedditPost> posts, string fileName)
    {
        Directory.CreateDirectory(OUTPUT_FILE_PATH);

        var outputFilePath = $"{OUTPUT_FILE_PATH}/{fileName}.pdf";
        // using var writer = new PdfWriter(outputFilePath);
        // using var pdf = new PdfDocument(writer);
        // var document = new Document(pdf);

        // foreach (var post in posts)
        // {
        //     document.Add(new Paragraph("Reddit Post Report").SimulateBold().SetFontSize(18));
        //     document.Add(new Paragraph($"Title: {post.Title}").SimulateBold());
        //     document.Add(new Paragraph($"Subreddit: {post.Subreddit}"));
        //     document.Add(new Paragraph($"Author: {post.Author}"));
        //     document.Add(new Paragraph($"Upvotes: {post.Upvotes}"));
        //     document.Add(new Paragraph($"Downvotes: {post.Downvotes}"));
        //     document.Add(new Paragraph($"Score: {post.Score}"));
        //     document.Add(new Paragraph($"Comments: {post.NumComments}"));
        //     document.Add(new Paragraph($"Created At: {post.CreatedAt}"));
        //     document.Add(new Paragraph($"Permalink: https://reddit.com{post.Permalink}"));

        //     // If there's an image
        //     if (!string.IsNullOrEmpty(post.Thumbnail))
        //     {
        //         var imageUrl = post.Thumbnail;
        //         var imageStream = await client.GetStreamAsync(imageUrl);

        //         byte[] imageBytes;
        //         using (var memoryStream = new MemoryStream())
        //         {
        //             await imageStream.CopyToAsync(memoryStream);
        //             imageBytes = memoryStream.ToArray();
        //         }

        //         var image = new iText.Layout.Element.Image(
        //             iText.IO.Image.ImageDataFactory.Create(imageBytes)
        //         );
        //         document.Add(image);
        //     }
        // }

        // document.Close();
        var htmlString = HtmlAdapter.ConvertToHtmlString(posts);

        using (var stream = new FileStream(outputFilePath, FileMode.Create))
        {
            // Step 3: Use HtmlConverter to convert HTML to PDF and write to the stream
            HtmlConverter.ConvertToPdf(htmlString, stream);
        }

        return outputFilePath;
    }
}
