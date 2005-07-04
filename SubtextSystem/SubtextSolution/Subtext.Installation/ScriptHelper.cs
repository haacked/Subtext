using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Subtext.Installation
{ 
	/// <summary>
	/// Summary description for ResourceHelper.
	/// </summary>
	public sealed class ScriptHelper
	{
		private ScriptHelper() {}

		/// <summary>
		/// Executes the script.
		/// </summary>
		/// <param name="scriptName">Name of the script.</param>
		public static bool ExecuteScript(string scriptName, SqlTransaction transaction)
		{
			string scriptText = UnpackEmbeddedScriptAsString(scriptName);
			ScriptCollection scripts = Script.ParseScripts(scriptText);
			
			foreach(Script script in scripts)
			{
				int returnValue = script.Execute(transaction);
				if(returnValue != -1)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Unpacks an embedded script into a string.
		/// </summary>
		/// <param name="scriptName"></param>
		public static string UnpackEmbeddedScriptAsString(string scriptName)
		{
			Stream stream = UnpackEmbeddedScript(scriptName);
			using(StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Unpacks an embedded script into a Stream.
		/// </summary>
		/// <param name="scriptName">Name of the script.</param>
		public static Stream UnpackEmbeddedScript(string scriptName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			return assembly.GetManifestResourceStream("Subtext.Installation.Scripts." + scriptName);
		}
	}
}
