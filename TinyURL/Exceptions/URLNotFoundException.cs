namespace TinyURL.Exceptions;

public class URLNotFoundException : Exception
{
	public URLNotFoundException(): base() { }

    public URLNotFoundException(string message) : base(message) { }
}


