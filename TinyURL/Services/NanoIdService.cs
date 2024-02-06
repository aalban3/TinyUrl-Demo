using JsonFlatFileDataStore;
using NanoidDotNet;
using TinyURL.Interfaces;

namespace TinyURL.Services;

public class NanoIdService: INanoIdService
{
    private readonly IDataStore _urlDataStore;
    private readonly ISet<string> _idSet;

    public NanoIdService(IDataStore urlDataStore)
	{
        _urlDataStore = urlDataStore;
        _idSet = _urlDataStore.GetItem<ISet<string>>("nanoId");
    }

    public string GetNewId()
    {
        string urlId = string.Empty;

        while (string.IsNullOrEmpty(urlId))
        {
            var tempId = Nanoid.Generate(size: 9);
            if (!_idSet.Contains(tempId))
            {
                urlId = tempId;
            }
        }

        return urlId;
    }
}


