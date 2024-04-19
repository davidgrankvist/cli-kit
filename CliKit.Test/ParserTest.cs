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

		private class TestParseAction
		{
			public int ComputedSum { get; set; } = 0;

			[CliArg("first", "f")]
			public int First { get; set; }

			[CliArg("second", "s")]
			public int Second { get; set; }

			[CliAction]
			public void ComputeSum()
			{
				ComputedSum = First + Second;
			}
		}

		[TestMethod]
		public void TestParseArgsWithAction()
		{
			var args = new string[]
			{
				"--first", "100",
				"--second", "50"
			};
			var (parsedArgs, action) = ArgParser.ParseWithAction<TestParseAction>(args);
			action?.Invoke();

			Assert.IsNotNull(action);
			Assert.AreEqual(150, parsedArgs.ComputedSum);
		}

		private class TestEmpty { }

		[TestMethod]
		public void TestParseEmptyAction()
		{
			var (_, action) = ArgParser.ParseWithAction<TestEmpty>(new string[] { });

			Assert.IsNull(action);
		}
	}
}