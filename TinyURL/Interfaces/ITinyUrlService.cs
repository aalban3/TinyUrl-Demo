namespace TinyURL.Interfaces;

public interface ITinyUrlService
{
    void Get(string shortUrl);
    void Save(string originalUrl);
    void Save(string originalUrl, string customUrl);
    void GetClickCount(string shortUrl);
    void Delete(string originalUrl);
}