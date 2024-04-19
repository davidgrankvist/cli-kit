using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliKit.Lib
{
	public static class ArgParser
	{

		public static T Parse<T>(string[] args)
		{
			return ParseWithAction<T>(args).ParsedArgs;
		}

		public static (T ParsedArgs, Action? ResolvedAction) ParseWithAction<T>(string[] args)
		{
			var parsedArgsType = typeof(T);
			var parsedArgs = (T)Activator.CreateInstance(parsedArgsType)!;

			/*
			 * Map arguments names to prop info so that
			 * the annoted properties can be set later
			 */
			var propInfoByArg = new Dictionary<string, PropertyInfo>();
			foreach (var propInfo in parsedArgsType.GetProperties())
			{
				if (propInfo == null)
				{
					continue;
				}

				var cliArg = propInfo.GetCustomAttribute<CliArgAttribute>();
				if (cliArg == null)
				{
					continue;
				}

				propInfoByArg[cliArg.Name] = propInfo;
				propInfoByArg[cliArg.AbbrevName] = propInfo;
			}

			/*
			 * Loop through the given args and set the corresponding props
			 */
			PropertyInfo? nextInfo = null;
			for (int i = 0; i < args.Length; i++)
			{
				var curr = args[i];

				if (IsParam(curr))
				{
					var name = ParamToName(curr);
					var info = propInfoByArg[name];
					var t = info.PropertyType;

					if (t == typeof(bool))
					{
						info.SetValue(parsedArgs, true);
						nextInfo = null;
					}
					else
					{
						nextInfo = info;
					}
				}
				else if (nextInfo != null)
				{
					var value = curr;
					var info = nextInfo;

					var t = info.PropertyType;

					if (t == typeof(string))
					{
						info.SetValue(parsedArgs, value);
					}
					else if (t == typeof(int))
					{
						info.SetValue(parsedArgs, int.Parse(value));
					}

					nextInfo = null;
				}
				else
				{
					// nested commands are ignored for now
				}
			}

			/*
			 * Find the command action, if one exists
			 */
			var actions = parsedArgsType.GetMethods().Where(m => m.GetCustomAttribute<CliActionAttribute>() != null).ToList();
			MethodInfo? methodInfo = actions.Count > 0 ? actions.First() : null;
			Action? resolvedAction = null;

			if (methodInfo != null)
			{
				if (methodInfo.GetParameters().Length > 0)
				{
					throw new NotSupportedException("Methods with the CliAction attribute must have zero parameters");
				}
				resolvedAction = () => methodInfo.Invoke(parsedArgs, null);
			}

			return (parsedArgs, resolvedAction);
		}

		private static bool IsParam(string arg)
		{
			return arg.StartsWith('-');
		}

		private static string ParamToName(string arg)
		{
			if (arg.Length > 2 && arg.StartsWith("--"))
			{
				return arg[2..];
			}
			else if (arg.Length > 1 && arg.StartsWith('-'))
			{
				return arg[1..];
			}
			else
			{
				return arg;
			}
		}
	}
}
