namespace TinyURL.Exceptions;

public class UrlNotFoundException : Exception
{
	public UrlNotFoundException(): base() { }

    public UrlNotFoundException(string message) : base(message) { }
}


