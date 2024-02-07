namespace TinyURL.Interfaces;

public interface ICommandService
{
    string? Get(Uri shortUrl);
    string? Save(Uri originalUrl);
    string? Save(Uri originalUrl, Uri customUrl);
    long? GetClickCount(Uri shortUrl);
    void Delete(Uri shortUrl);
}