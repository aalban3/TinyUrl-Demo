using JsonFlatFileDataStore;
using TinyURL.Interfaces;
using TinyURL.Services;

namespace Tests.Services;

public class TinyUrlServiceTests
{
    private readonly ITinyUrlService _tinyUrlService;
    private readonly IDataStore _urlDataStore;

    public TinyUrlServiceTests()
    {
        _urlDataStore = new DataStore("../testDataStore.json");
        _tinyUrlService = new TinyUrlService(
                _urlDataStore
            );
       
    }

    [Fact]
    public void Get_Success()
    {
        // Arrange

        // Act
        var result = _tinyUrlService.Get(null);

        // Assert
    }

    [Fact]
    public void Get_NoUrlFound()
    {
        // Arrange

        // Act

        // Assert
    }
}
