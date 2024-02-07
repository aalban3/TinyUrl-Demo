## URL Shortening Service DEMO

This demo is intended to show an implementation of a URL Shortener service. It is a console app written in .NET 8. For persistence I'm using a Json Data Store, which uses Json to mimic a real database. To generate URLs, I'm encoding the sequential DB "primary key" to ensure every short url will be unique. Testing was done with xUnit and Nsubstitute for mocking services.


Package References:
* JSOn Data Stoee package: https://github.com/ttu/json-flatfile-datastore
* SqId: https://sqids.org/dotnet
* NSubstitute: https://nsubstitute.github.io/