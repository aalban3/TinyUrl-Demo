namespace TinyURL.Models;

public class UrlEntity
{
    public long Id { get; init; }
    public string ShortUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public long Clicks { get; set; }
    public DateTime CreatedOn { get; set; } = new DateTime();
}