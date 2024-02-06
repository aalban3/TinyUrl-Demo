namespace TinyURL.Interfaces;

public interface IHashingService
{
    string GetHash(long nextId);
}


