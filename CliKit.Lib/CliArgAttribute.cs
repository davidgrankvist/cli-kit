namespace CliKit
{
	[AttributeUsage(AttributeTargets.Property)]
	public class CliArgAttribute : Attribute
	{
		public readonly string Name;
		public readonly string AbbrevName;

		public CliArgAttribute(string name, string abbrevName)
        {
            Name = name;
            AbbrevName = abbrevName;
		}
	}
}
