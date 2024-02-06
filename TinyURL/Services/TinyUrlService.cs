using System;
using JsonFlatFileDataStore;
using NanoidDotNet;
using TinyURL.Interfaces;
using TinyURL.Models;

namespace TinyURL.Services;

public class TinyUrlService: ITinyUrlService
{
    private readonly IDataStore _urlDataStore;
    private readonly IDocumentCollection<UrlEntity> _collection;

    public TinyUrlService(IDataStore urlDataStore)
	{
        _urlDataStore = urlDataStore;
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

        #region Update number of time the URL has been retreived
        result.NumberOfClicks += 1;
        _collection.UpdateOne(x => x.Id == result.Id, result);
        # endregion

        Console.WriteLine($"Original URL: {result?.OriginalUrl}");
    }

    public void Save(string originalUrl)
    {
        #region Check if item exists
        var query = _collection.AsQueryable().FirstOrDefault(x => x.OriginalUrl == originalUrl);
        if (query != null)
            Console.WriteLine("\nUrl Already Exists");
        #endregion

        #region Generate unique link ID an save entity
        var urlId = Nanoid.Generate(size:9);
        var shortUrl = $"http://tiny.test/{urlId}";
        var entityToCreate = new UrlEntity
        {
            OriginalUrl = originalUrl,
            ShortUrl = shortUrl
        };
        var result = _collection.InsertOne(entityToCreate);
        #endregion

        Console.WriteLine($"\n{shortUrl}");
    }

    public void Save(string originalUrl, string customUrl)
    {
        #region Check if item exists
        var query = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == originalUrl);
        if (query != null)
            Console.WriteLine("\nUrl Already Exists");
        # endregion

        var shortUrl = $"http://tiny.test/{customUrl}";
        var entityToCreate = new UrlEntity
        {
            OriginalUrl = originalUrl,
            ShortUrl = customUrl
        };

        _collection.InsertOne(entityToCreate);

        Console.WriteLine($"\n{shortUrl}");
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

        Console.WriteLine($"\nURL has been used: {result.NumberOfClicks} times");
    }

    public void Delete(string originalUrl)
    {
        throw new NotImplementedException();
    }

}


