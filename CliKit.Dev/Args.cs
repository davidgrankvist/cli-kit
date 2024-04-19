using CliKit.Lib;

namespace CliKit.Dev
{
	public class Args
	{
		[CliArg("count", "c")]
		public int Count { get; set; }

		[CliArg("name", "n")]
		public string Name { get; set; }

		[CliArg("flag", "f")]
        public bool Flag { get; set; }

		public override string ToString()
		{
			return $"Count={Count}, Name={Name}, Flag={Flag}";
		}
	}
}
