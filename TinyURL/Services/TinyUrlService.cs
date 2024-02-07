using TinyURL.Interfaces;

namespace TinyURL.Services;

public class TinyUrlService
{
	private readonly ICommandService _service;

    public TinyUrlService(ICommandService service)
	{
		_service = service;

    }

	public void Run()
	{
        var keepOpen = true;

        while (keepOpen == true)
        {
            // Display Command Options
            PrintCommandOptions();

            #region Get Input from user
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No input received");
                continue;
            }

            var inputArgs = input.Split(' ');
            var command = inputArgs[0];
            var urlArgs = inputArgs.Length > 1 ? inputArgs.Skip(1).ToArray() : Array.Empty<string>();
            # endregion

            #region Handle command actions
            switch (command)
            {
                case "create":
                    string? newUrl;

                    if (urlArgs.Length == 2)
                        newUrl = _service.Save(urlArgs[0], urlArgs[1]);
                    else
                        newUrl = _service.Save(urlArgs[0]);

                    if (!string.IsNullOrEmpty(newUrl))
                        Console.WriteLine($"\nShort URL: {newUrl}");

                    break;
                case "delete":
                    _service.Delete(urlArgs[0]);

                    break;
                case "fetch":
                    var originalUrl = _service.Get(urlArgs[0]);
                    if (originalUrl != null)
                        Console.WriteLine($"\nOriginal URL: {originalUrl}");

                    break;
                case "fetch-clicks":
                    var clicks = _service.GetClickCount(urlArgs[0]);

                    if (clicks > 0)
                        Console.WriteLine($"\nThis URL has been used {clicks} times");

                    break;
                case "exit":
                    Console.WriteLine("\nClosing Application!");
                    keepOpen = false;

                    break;
                case "clear":
                    Console.Clear();

                    break;
                default:
                    Console.WriteLine("\nInvalid Option!");

                    break;
            }
            # endregion

            Console.WriteLine("\n");
        }
    }

    private static void PrintCommandOptions()
    {
        // This is our UI.
        Console.WriteLine("Available actions:");
        Console.WriteLine("\tcreate <url_string>");
        Console.WriteLine("\tcreate <url_string> <custom_url_string>");
        Console.WriteLine("\tdelete <url_string>");
        Console.WriteLine("\tfetch <url_string>");
        Console.WriteLine("\tfetch-clicks <url_string>\n");
        Console.WriteLine("Enter \"clear\" to clear screen or \"exit\" to quit.");
        Console.Write(": ");
    }
}


