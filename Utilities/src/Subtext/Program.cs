using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Subtext
{
	class Program
	{
		static private Arguments arguments;
		static private StringCollection commands = null;
		
		static void Main(string[] args)
		{
			arguments = new Arguments(Environment.CommandLine);
			if (arguments.Count == 0 || args.Length == 0 || String.Equals(args[0], "help", StringComparison.InvariantCultureIgnoreCase))
			{
				DisplayArguments();
				return;
			}

			string command = args[0].ToLower();
			if(!CommandList.Contains(command))
			{
				WriteError("Command '{0}' is not valid", command);
				return;
			}

			switch(command)
			{
				case "install":
					InstallCommand installer = new InstallCommand();
					installer.Execute(arguments);
					break;
			}
		}

		static StringCollection CommandList
		{
			get
			{
				if(commands == null)
				{
					commands = new StringCollection();
					commands.Add("install");
				}
				return commands;
			}
		}

		static void DisplayArguments()
		{
			WriteVersionInformation("subtext.exe v{0} - Command Line Interface to Subtext v{1}");
			Console.WriteLine("Usage:   subtext command [options]");
			Console.WriteLine("Sample:  subtext install /connect ConnectionString");
			Console.WriteLine("Help:    subtext help");
			Console.WriteLine();
			Console.WriteLine("******************** Argument List ****************************");
			Console.WriteLine("/connect	Connection String to the database.");
			Console.WriteLine("/create-db	If specified, creates the database if it doesn't already exist.");
			Console.WriteLine("/recreate-db	If specified, drops and then creates the database.");
		}

		static void WriteVersionInformation(string formatString)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string version = assembly.GetName().Version.ToString();
			string subtextVersion = "---";

			AssemblyName[] assemblyNames = assembly.GetReferencedAssemblies();
			foreach (AssemblyName assemblyName in assemblyNames)
			{
				if (assemblyName.Name == "Subtext.Framework")
				{
					subtextVersion = assemblyName.Version.ToString();
					break;
				}
			}
			Console.WriteLine(formatString, version, subtextVersion);
		}

		static void WriteError(string message, params object[] paramters)
		{
			Console.WriteLine(message, paramters);
		}
	}
}
