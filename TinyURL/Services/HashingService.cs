using Sqids;
using TinyURL.Interfaces;

namespace TinyURL.Services;

// This service's only job is to encode the DB's serial Id to ensure every single Short URL hash is unique
// I never decode it since all lookups are done by shortUrl for this POC.
public class HashingService: IHashingService
{
    public HashingService(){}

    public string GetHash(long nextId)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 9,
        });
        var hash = sqids.Encode(nextId);

        return hash;
    }
}