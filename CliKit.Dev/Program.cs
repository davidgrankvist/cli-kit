using CliKit.Lib;

namespace CliKit.Dev
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var testArgs = new string[]
			{
				"nameOfTheProgram",
				"--count", "5",
				"--name", "Bob",
				"--flag",
			};
			var parsedArgs = Parser.Parse<Args>(testArgs);

            Console.WriteLine(parsedArgs);
        }
	}
}
