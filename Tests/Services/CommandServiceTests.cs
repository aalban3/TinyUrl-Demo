using JsonFlatFileDataStore;
using TinyURL.Exceptions;
using TinyURL.Interfaces;
using TinyURL.Models;
using TinyURL.Services;

namespace Tests.Services;

public class TinyUrlServiceTests
{
    private readonly ICommandService _tinyUrlService;
    private readonly IDataStore _urlDataStore;
    private readonly IDocumentCollection<UrlEntity> _collection;

    public TinyUrlServiceTests()
    {
        _urlDataStore = new DataStore("../testDataStore.json");
        _collection = _urlDataStore.GetCollection<UrlEntity>("url");
        _tinyUrlService = new CommandService(_urlDataStore);
    }

    [Fact]
    public void Get_Success()
    {
        // Arrange
        var testEntity = new UrlEntity
        {
            Id = 1,
            OriginalUrl = "https://test-url.com",
            ShortUrl = "https://tiny.test/12345"
        };
        _collection.InsertOne(testEntity);

        // Act
        var result = _tinyUrlService.Get("https://tiny.test/12345");

        // Assert
        Assert.Equal(result, testEntity.OriginalUrl);
        _urlDataStore.Dispose();
    }

    [Fact]
    public void Get_UrlNotFound()
    {
        // Arrange
        var testEntity = new UrlEntity
        {
            Id = 1,
            OriginalUrl = "https://test-url.com",
            ShortUrl = "https://tiny.test/12345"
        };
        _collection.InsertOne(testEntity);

        // Act
        var result = Assert.Throws<ItemNotFoundException>(() => _tinyUrlService.Get("http://fakeurl/22222"));

        // Assert
        Assert.Equal(result.Message, "\nUrl not found.");
        _urlDataStore.Dispose();
    }
}
