using CliKit.Lib;

namespace CliKit.Dev
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var (parsedArgs, action) = ArgParser.ParseWithAction<Args>(args);

			Console.WriteLine($"Parsed args: {parsedArgs}");
			action?.Invoke();
		}
	}
}
