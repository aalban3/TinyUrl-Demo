using TinyURL.Services;
using JsonFlatFileDataStore;

var store = new DataStore("./urlDataStore.json");
var service = new CommandService(store);
var app = new TinyUrlService(service);

app.Run();

// This line can be uncommented if we want the store to clear on close
// otherwise data will persist in the json file

// store.Dispose();