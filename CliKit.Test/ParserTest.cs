using CliKit.Lib;

namespace CliKit.Test
{
	[TestClass]
	public class ParserTest
	{
		[TestMethod]
		[DynamicData(nameof(TestData), DynamicDataSourceType.Method)]
		public void TestParseArgs(string[] args, Args expected, string message)
		{
			var parsedArgs = ArgParser.Parse<Args>(args);
			Assert.AreEqual(expected, parsedArgs, message);
		}

		public static IEnumerable<object[]> TestData()
		{
			string[] args;
			Args parsedArgs;
			string message;

			message = "Set all";
			args = new string[]
			{
				"--count", "5",
				"--name", "Bob",
				"--flag",
			};
			parsedArgs = new Args
			{
				Count = 5,
				Name = "Bob",
				Flag = true,
			};
			yield return new object[] { args, parsedArgs, message };

			message = "Skip all";
			args = new string[]
			{
			};
			parsedArgs = new Args
			{
				Count = 0,
				Name = null,
				Flag = false,
			};
			yield return new object[] { args, parsedArgs, message };

			message = "Set all, reordered";
			args = new string[]
			{
				"--name", "Bob",
				"--flag",
				"--count", "5",
			};
			parsedArgs = new Args
			{
				Count = 5,
				Name = "Bob",
				Flag = true,
			};
			yield return new object[] { args, parsedArgs, message };

			message = "Provide a name without its value";
			args = new string[]
			{
				"--count",
				"--name","Bob",
			};
			parsedArgs = new Args
			{
				Count = 0,
				Name = "Bob",
				Flag = false,
			};
			yield return new object[] { args, parsedArgs, message };
		}
	}
}