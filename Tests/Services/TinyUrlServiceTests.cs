using NSubstitute;
using TinyURL.Interfaces;
using TinyURL.Services;

namespace Tests.Services;

public class TinyUrlServiceTests
{
    private readonly TinyUrlService _tinyURLService;
    private readonly ICommandService _mockCommandService;
    private readonly ICommandLineHelper _mockCommandLineHelper;

    public TinyUrlServiceTests()
    {
        _mockCommandService = Substitute.For<ICommandService>();
        _mockCommandLineHelper = Substitute.For<ICommandLineHelper>();
        _tinyURLService = new TinyUrlService(_mockCommandService, _mockCommandLineHelper);
    }

    [Fact]
    public void Run_NoUserInput()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);
        _mockCommandLineHelper.GetInput().Returns(string.Empty);

        // Act
        _tinyURLService.Run();

        // Assert
        Assert.Contains("No input received", consoleText.ToString());
    }

    [Fact]
    public void Run_InvalidInputCommand()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);
        _mockCommandLineHelper.GetInput().Returns("fetch-fetch");

        // Act
        _tinyURLService.Run();

        // Assert
        Assert.Contains("Invalid Option!", consoleText.ToString());
    }

    [Fact]
    public void Run_Create_NewShortUrl()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);
        _mockCommandLineHelper.GetInput().Returns("create http://www.test.io");
        _mockCommandService.Save(new Uri("http://www.test.io")).Returns("https://tiny.url/123abc");

        // Act
        _tinyURLService.Run();

        // Assert
        Assert.Contains("Short URL: https://tiny.url/123abc", consoleText.ToString());
    }

    [Fact]
    public void Run_Fetch_ExistingUrl()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);
        _mockCommandLineHelper.GetInput().Returns("fetch https://tiny.url/123abc");
        _mockCommandService.Get(new Uri("https://tiny.url/123abc")).Returns("https://alan-alban.com");

        // Act
        _tinyURLService.Run();

        // Assert
        Assert.Contains("Original URL: https://alan-alban.com", consoleText.ToString());
    }

    [Fact]
    public void Run_Delete_ExistingUrl()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);
        _mockCommandLineHelper.GetInput().Returns("delete https://tiny.url/123abc");

        // Act
        _tinyURLService.Run();

        // Assert
        _mockCommandService.Received(1).Delete(new Uri("https://tiny.url/123abc"));
    }

    [Fact]
    public void Run_Fetch_NumberOfURLClicks()
    {
        // Arrange
        var consoleText = new StringWriter();
        Console.SetOut(consoleText);
        _mockCommandLineHelper.GetInput().Returns("fetch-clicks https://tiny.url/123abc");
        _mockCommandService.GetClickCount(new Uri("https://tiny.url/123abc")).Returns(5);

        // Act
        _tinyURLService.Run();

        // Assert
        Assert.Contains("This URL has been used 5 times", consoleText.ToString());
    }
}
