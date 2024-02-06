using TinyURL.Services;
using JsonFlatFileDataStore;

var store = new DataStore("./urlDataStore.json");
var service = new TinyUrlService(store);
var keepOpen = true;

while (keepOpen == true)
{
    // This is our UI.
    Console.WriteLine("Available actions:");
    Console.WriteLine("\tcreate <url_string>");
    Console.WriteLine("\tcreate <url_string> <custom_url_string>");
    Console.WriteLine("\tdelete <url_string>");
    Console.WriteLine("\tfetch <url_string>");
    Console.WriteLine("\tfetch_clicks <url_string>\n");
    Console.WriteLine("Enter \"exit\" to quit.");
    Console.Write(": ");

    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("No input received");
        continue;
    }

    var inputArgs = input.Split(' ');
    var command = inputArgs[0];
    var urlArgs = inputArgs.Length > 1 ? inputArgs.Skip(1).ToArray() : Array.Empty<string>();

    switch (command)
    {
        case "create":
            if (urlArgs.Length == 2)
                service.Save(urlArgs[0], urlArgs[1]);
            else
                service.Save(urlArgs[0]);
            break;

        case "delete":
            service.Delete(urlArgs[0]);
            break;

        case "fetch":
            service.Get(urlArgs[0]);
            break;

        case "fetch-clicks":
            service.GetClickCount(urlArgs[0]);
            break;

        case "exit":
            Console.WriteLine("Closing Application!");
            keepOpen = false;
            break;

        default:
            Console.WriteLine("Invalid Option!");
            break;
    }

    Console.WriteLine("\n");
}

// This line can be uncommented if we want the store to clear on close
// otherwise data will persist in the json file

// store.Dispose();