using TinyURL.Services;
using TinyURL.Utils;
using JsonFlatFileDataStore;

// Setup Services
var store = new DataStore("./urlDataStore.json"); // wrap in using
var hasher = new HashingService();
var service = new CommandService(store, hasher);
var app = new TinyUrlService(service, new CommandLineHelper());

// Run App Until user exits
while(app.ShouldContinue())
{
    TinyUrlService.PrintCommandOptions();
    app.Run();
}

store.Dispose();