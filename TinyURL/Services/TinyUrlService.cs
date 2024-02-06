using JsonFlatFileDataStore;
using TinyURL.Interfaces;
using TinyURL.Models;

namespace TinyURL.Services;

public class TinyUrlService: ITinyUrlService
{
    private readonly IDataStore _urlDataStore;
    private readonly IDocumentCollection<UrlEntity> _collection;
    private readonly IHashingService _hashingService;

    public TinyUrlService(IDataStore urlDataStore)
	{
        _urlDataStore = urlDataStore;
        _collection = _urlDataStore.GetCollection<UrlEntity>("url");
        _hashingService = new HashingService();
    }

    public void Get(string shortUrl)
    {
        #region Fetch Item and handle null case
        var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);
        if (result == null)
        {
            Console.WriteLine("\nUrl not found.");
            return;
        }
        #endregion

        #region Update number of times the URL has been retreived
        result.Clicks++;
        _collection.UpdateOne(x => x.Id == result.Id, result);
        # endregion

        Console.WriteLine($"\nOriginal URL: {result.OriginalUrl}");
    }

    public void Save(string originalUrl)
    {
        #region Check if URL format is valid
        if (!Uri.IsWellFormedUriString(originalUrl, UriKind.RelativeOrAbsolute))
        {
            Console.WriteLine("\nCannot general Short URL from invalid URL");
            return;
        }
        #endregion

        #region Generate unique link ID an save entity
        var nextId = _collection.GetNextIdValue();
        var urlHash = _hashingService.GetHash(nextId);
        var shortUrl = $"http://aa.tinyurl/{urlHash}";

        var newEntity = new UrlEntity
        {
            OriginalUrl = originalUrl,
            ShortUrl = shortUrl
        };

        _collection.InsertOne(newEntity);
        #endregion

        Console.WriteLine($"\nYour URL: {shortUrl}");
    }

    public void Save(string originalUrl, string customUrl)
    {
        #region Check if URL format is valid
        if (!Uri.IsWellFormedUriString(originalUrl, UriKind.RelativeOrAbsolute))
        {
            Console.WriteLine("\nCannot general Short URL from invalid URL");
            return;
        }
        #endregion

        # region Check if custom item exists
        var query = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == customUrl);
        if (query != null)
            Console.WriteLine("\nUrl combination already exists.");
        #endregion

        #region Generate unique link ID an save entity
        var newEntity = new UrlEntity
        {
            OriginalUrl = originalUrl,
            ShortUrl = customUrl
        };

        _collection.InsertOne(newEntity);
        # endregion

        Console.WriteLine($"\nYour URL: {customUrl}");
    }

    public void GetClickCount(string shortUrl)
    {
        #region Fetch Item and handle null case
        var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);
        if (result == null)
        {
            Console.WriteLine("\nUrl not found.");
            return;
        }
        #endregion

        Console.WriteLine($"\nURL has been used: {result.Clicks} times");
    }

    public void Delete(string shortUrl)
    {
        #region Fetch Item and handle null case
        var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);
        if (result == null)
        {
            Console.WriteLine("\nUrl not found.");
            return;
        }
        #endregion

        #region Handle Deletion
        _collection.DeleteOne(x => x.ShortUrl == shortUrl);
        #endregion

        Console.WriteLine($"\nThe URL {shortUrl} has been removed.");
    }

}