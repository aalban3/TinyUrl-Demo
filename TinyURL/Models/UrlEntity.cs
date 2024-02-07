namespace TinyURL.Models;

public class UrlEntity
{
    public long Id { get; init; }
    public Uri? ShortUrl { get; set; }
    public Uri? OriginalUrl { get; set; }
    public long Clicks { get; set; }
    public DateTime CreatedOn { get; set; } = new DateTime();
}