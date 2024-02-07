using JsonFlatFileDataStore;
using TinyURL.Exceptions;
using TinyURL.Interfaces;
using TinyURL.Models;

namespace TinyURL.Services;

public class CommandService: ICommandService
{
    private readonly IDataStore _urlDataStore;
    private readonly IDocumentCollection<UrlEntity> _collection;
    private readonly IHashingService _hashingService;
    public CommandService(IDataStore urlDataStore, IHashingService hashingService)
	{
        _urlDataStore = urlDataStore;
        _collection = _urlDataStore.GetCollection<UrlEntity>("url");
        _hashingService = hashingService;
    }

    public string? Get(Uri? shortUrl)
    {
        try
        {
            #region Fetch Item and handle null case
            var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);

            if (result == null)
                throw new UrlNotFoundException("\nUrl not found.");
            #endregion

            #region Update number of times the URL has been retreived
            result.Clicks++;
            _collection.UpdateOne(x => x.Id == result.Id, result);
            #endregion

            return result?.OriginalUrl?.ToString();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public string? Save(Uri? originalUrl)
    {
        try
        {
            #region Generate unique link ID an save entity
            var nextId = _collection.GetNextIdValue();
            var urlHash = _hashingService.GetHash(nextId);
            var shortUrl = new Uri($"http://aa.tinyurl/{urlHash}");

            var newEntity = new UrlEntity
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl
            };

            _collection.InsertOne(newEntity);
            #endregion

            return shortUrl.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public string? Save(Uri? originalUrl, Uri? customUrl)
    {
        try
        {
            #region Check if custom item exists
            var query = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == customUrl);
            if (query != null)
                throw new Exception("\nUrl combination already exists.");
            #endregion

            #region Generate unique link ID an save entity
            var newEntity = new UrlEntity
            {
                OriginalUrl = originalUrl,
                ShortUrl = customUrl
            };

            _collection.InsertOne(newEntity);
            #endregion

            return customUrl?.ToString();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public long? GetClickCount(Uri? shortUrl)
    {
        try
        {
            var result = _collection.AsQueryable().FirstOrDefault(x => x.ShortUrl == shortUrl);

            if (result == null)
                throw new UrlNotFoundException("\nUrl not found");

            return result.Clicks;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    public void Delete(Uri? shortUrl)
    {
        try
        {
            var didDelete = _collection.DeleteOne(x => x.ShortUrl == shortUrl);

            if (!didDelete)
                throw new UrlNotFoundException("\n Could not found URL to delete");

            Console.WriteLine($"\nDeleted URL: {shortUrl}");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}