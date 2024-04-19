using CliKit.Lib;

namespace CliKit.Test
{
	public class Args
	{
		[CliArg("count", "c")]
		public int Count { get; set; }

		[CliArg("name", "n")]
		public string? Name { get; set; }

		[CliArg("flag", "f")]
		public bool Flag { get; set; }

		public override string ToString()
		{
			return $"Count={Count}, Name={Name}, Flag={Flag}";
		}

		public override bool Equals(object? obj)
		{
			if (obj is not Args)
			{
				return false;
			}
			var arg = (Args)obj;
			return arg.Count == Count && arg.Name == Name && arg.Flag == Flag;
		}

	}
}