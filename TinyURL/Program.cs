using TinyURL.Services;
using TinyURL.Utils;
using JsonFlatFileDataStore;

// Setup Services
var store = new DataStore("./urlDataStore.json");
var service = new CommandService(store);
var app = new TinyUrlService(service, new CommandLineHelper());

// Run App Until user exits
while(app.ShouldContinue())
{
    TinyUrlService.PrintCommandOptions();
    app.Run();
}

store.Dispose();