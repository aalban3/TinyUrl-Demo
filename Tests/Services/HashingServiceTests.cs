using TinyURL.Interfaces;
using TinyURL.Services;

namespace Tests.Services;

public class HashingServiceTests
{
    private readonly IHashingService _hashingservice;

    public HashingServiceTests()
    {
        _hashingservice = new HashingService();
    }

    [Fact]
    public void GetHash_ReturnsHashCode()
    {
        // Arrange & Act
        var result = _hashingservice.GetHash(1L);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(9, result.Length);
    }
}