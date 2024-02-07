using JsonFlatFileDataStore;
using TinyURL.Interfaces;
using TinyURL.Models;
using TinyURL.Services;

namespace Tests.Services;

public class CommandServiceTests
{
    private readonly ICommandService _commandService;
    private readonly IDataStore _urlDataStore;
    private readonly IDocumentCollection<UrlEntity> _collection;
    private readonly IHashingService _hashingService;

    public CommandServiceTests()
    {
        _urlDataStore = new DataStore("../testDataStore.json");
        _collection = _urlDataStore.GetCollection<UrlEntity>("url");
        _hashingService = new HashingService();
        _commandService = new CommandService(_urlDataStore, _hashingService);
    }

    [Fact]
    public void Get_Success()
    {
        // Arrange
        var testShortUrl = new Uri("https://tiny.test/12345");
        var testEntity = new UrlEntity
        {
            OriginalUrl = new Uri("https://test-url.com"),
            ShortUrl = testShortUrl
        };
        _collection.InsertOne(testEntity);

        // Act
        var result = _commandService.Get(testShortUrl);

        // Assert
        Assert.Equal(testEntity.OriginalUrl.ToString(), result);

        // Cleanup
        _collection.DeleteOne(x => x.ShortUrl == testShortUrl);
    }

    [Fact]
    public void Get_Failure_UrlNotFound()
    {
        // Arrange
        var testShortUrl = new Uri("https://tiny.test/12345");
        var testEntity = new UrlEntity
        {
            OriginalUrl = new Uri("https://test-url.com"),
            ShortUrl = testShortUrl
        };
        _collection.InsertOne(testEntity);

        var consoleText = new StringWriter();
        Console.SetOut(consoleText);

        // Act
        var result = _commandService.Get(new Uri("https://tiny.fake.test/12345658999"));

        // Assert
        Assert.Null(result);
        Assert.Equal("\nUrl not found.\n", consoleText.ToString());

        // Cleanup
        _collection.DeleteOne(x => x.ShortUrl == testShortUrl);
    }

    [Fact]
    public void Save_Success()
    {
        // Arrange
        var originalUrl = new Uri("https://alan-alban.com");

        // Act
        var result = _commandService.Save(originalUrl);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, _collection.Count);

        // Cleanup
        _collection.DeleteOne(x => x.ShortUrl == new Uri(result));
    }

    [Fact]
    public void Save_Success_CustomUrl()
    {
        // Arrange
        var originalUrl = new Uri("https://alan-alban.com");
        var customUrl = new Uri("https://alan.ly/ab5iii");

        // Act
        var result = _commandService.Save(originalUrl, customUrl);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, _collection.Count);

        // Cleanup
        _collection.DeleteOne(x => x.ShortUrl == customUrl);
    }

    [Fact]
    public void Save_Failure_InvalidURLFormatting()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);

        // Act
        try
        {
            _commandService.Save(new Uri("httpsalan-albancom"));
        }
        catch (Exception)
        {
            return;
        }

        // Assert
        Assert.Fail("Call should fail with a malformed URI");
    }

    [Fact]
    public void GetClickCount_ReturnsNumberOfClicks()
    {
        // Arrange
        var testShortUrl = new Uri("https://tiny.test/12345");
        var testEntity = new UrlEntity
        {
            OriginalUrl = new Uri("https://test-url.com"),
            ShortUrl = testShortUrl
        };
        _collection.InsertOne(testEntity);

        // Act
        _commandService.Get(testShortUrl);
        _commandService.Get(testShortUrl);
        _commandService.Get(testShortUrl);

        var result = _commandService.GetClickCount(testShortUrl);

        // Assert
        Assert.Equal(3, result);

        // Cleanup
        _collection.DeleteOne(x => x.ShortUrl == testShortUrl);
    }

    [Fact]
    public void GetClickCount_UrlNotFound()
    {
        // Arrange
        var badUrl = new Uri("https://bad.url.test/12345");
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);

        // Act
        var result = _commandService.GetClickCount(badUrl);

        // Assert
        Assert.Equal(0, result);
        Assert.Equal("\nUrl not found\n", consoleText.ToString());
    }

    [Fact]
    public void Delete_SuccessfulDeletion()
    {
        // Arrange
        var testShortUrl = new Uri("https://tiny.test/12345");
        var testEntity = new UrlEntity
        {
            OriginalUrl = new Uri("https://test-url.com"),
            ShortUrl = testShortUrl
        };
        _collection.InsertOne(testEntity);

        var consoleText = new StringWriter();
        Console.SetOut(consoleText);

        // Act
        _commandService.Delete(testShortUrl);

        // Assert
        Assert.Equal($"\nDeleted URL: {testShortUrl}\n", consoleText.ToString());
    }

    [Fact]
    public void Delete_FailedDeletion()
    {
        // Arrange
        var testShortUrl = new Uri("https://tiny.test/12345");
        var testEntity = new UrlEntity
        {
            OriginalUrl = new Uri("https://test-url.com"),
            ShortUrl = testShortUrl
        };
        _collection.InsertOne(testEntity);

        var consoleText = new StringWriter();
        Console.SetOut(consoleText);

        // Act
        _commandService.Delete(new Uri("https://alan.test/111iii"));

        // Assert
        Assert.Equal("\n Could not found URL to delete\n", consoleText.ToString());
        Assert.Equal(1, _collection.Count);

        // Cleanup
        _collection.DeleteOne(x => x.ShortUrl == testShortUrl);
    }
}
