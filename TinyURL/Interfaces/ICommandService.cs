namespace TinyURL.Interfaces;

public interface ICommandService
{
    string? Get(string shortUrl);
    string? Save(string originalUrl);
    string? Save(string originalUrl, string customUrl);
    long? GetClickCount(string shortUrl);
    void Delete(string shortUrl);
}