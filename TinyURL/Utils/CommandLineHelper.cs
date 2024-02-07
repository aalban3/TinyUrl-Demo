using TinyURL.Interfaces;

namespace TinyURL.Utils;

public class CommandLineHelper: ICommandLineHelper
{
	public string? GetInput()
	{
		return Console.ReadLine();
	}
}