using JsonFlatFileDataStore;
using NanoidDotNet;
using TinyURL.Interfaces;
using TinyURL.Models;

namespace TinyURL.Services;

public class TinyUrlService: ITinyUrlService
{
    private readonly IDataStore _urlDataStore;
    private readonly IDocumentCollection<UrlEntity> _collection;
    private readonly INanoIdService _nanoIdService;

    public TinyUrlService(IDataStore urlDataStore)
	{
        _urlDataStore = urlDataStore;
        _nanoIdService = new NanoIdService(_urlDataStore);
        _collection = _urlDataStore.GetCollection<UrlEntity>("url");
    }

    public void Get(string shortUrl)
    {
        #region Fetch Item and handle null case
        var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);
        if (result == null)
        {
            Console.WriteLine("\nNo URL found.");
            return;
        }
        #endregion

        #region Update number of times the URL has been retreived
        result.Clicks++;
        _collection.UpdateOne(x => x.Id == result.Id, result);
        # endregion

        Console.WriteLine($"Original URL: {result.OriginalUrl}");
    }

    public void Save(string originalUrl)
    {
        #region Check if URL is valid
        if(!Uri.IsWellFormedUriString(originalUrl, UriKind.RelativeOrAbsolute))
        {
            Console.WriteLine("\nCannot general Short URL from invalid URL");
            return;
        }
        #endregion

        #region Generate unique link ID an save entity
        var urlId = _nanoIdService.GetNewId();
        var shortUrl = $"http://tiny.url/{urlId}";

        var newEntity = new UrlEntity
        {
            Id = urlId,
            OriginalUrl = originalUrl,
            ShortUrl = shortUrl
        };

        var result = _collection.InsertOne(newEntity);
        #endregion

        Console.WriteLine($"\n{shortUrl}");
    }

    public void Save(string originalUrl, string customUrl)
    {
        # region Check if item exists
        var query = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == originalUrl);
        if (query != null)
            Console.WriteLine("\nUrl Already Exists");
        #endregion

        # region Generate unique link ID an save entity
        var urlId = _nanoIdService.GetNewId();
        var newEntity = new UrlEntity
        {
            Id = urlId,
            OriginalUrl = originalUrl,
            ShortUrl = customUrl
        };

        _collection.InsertOne(newEntity);
        # endregion

        Console.WriteLine($"\n{customUrl}");
    }

    public void GetClickCount(string shortUrl)
    {
        #region Fetch Item and handle null case
        var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);
        if (result == null)
        {
            Console.WriteLine("\nNo URL found.");
            return;
        }
        #endregion

        Console.WriteLine($"\nURL has been used: {result.Clicks} times");
    }

    public void Delete(string originalUrl)
    {
        throw new NotImplementedException();
    }

}