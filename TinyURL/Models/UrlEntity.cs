namespace TinyURL.Models;

public class UrlEntity
{
    public long Id { get; set; }
    public string ShortUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public long NumberOfClicks { get; set; }
}