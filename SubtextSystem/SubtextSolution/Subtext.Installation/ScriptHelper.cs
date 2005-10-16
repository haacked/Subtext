using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace Subtext.Installation
{ 
	/// <summary>
	/// Summary description for ResourceHelper.
	/// </summary>
	public sealed class ScriptHelper
	{
		private ScriptHelper() {}

		private static readonly ILog log = new Subtext.Framework.Logging.Log();
		/// <summary>
		/// Executes the script.
		/// </summary>
		/// <remarks>
		/// Use script.Execute(transaction) to do the work. We will also pull the
		/// status of our script exection from here.
		/// </remarks>
		/// <param name="scriptName">Name of the script.</param>
		/// <param name="transaction">The current transaction.</param>
		public static bool ExecuteScript(string scriptName, SqlTransaction transaction)
		{
			string scriptText = UnpackEmbeddedScriptAsString(scriptName);
			ScriptCollection scripts = Script.ParseScripts(scriptText);

			// the following reg exp will be used to determine if each script is an
			// INSERT, UPDATE, or DELETE operation. The reg exp is also only looking
			// for these actions on the SubtextData database. <- do we need this last part?
			string regextStr = @"(INSERT\sINTO\sSubtextData\.[\d\w\.]+[\s\w\d]*)|(UPDATE\sSubtextData\.[\d\w\.]+\s*SET[\s\w\d]*)|(DELETE\sFROM\SubtextData\.[\d\w\.]+)";
			Regex regex = new Regex(regextStr, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Match match;
			
			foreach(Script script in scripts)
			{
				match = regex.Match(script.ScriptText);
				int returnValue = script.Execute(transaction);
				/* For UPDATE, INSERT, and DELETE statements, the return value is the 
				 * number of rows affected by the command. For all other types of statements, 
				 * the return value is -1. If a rollback occurs, the return value is also -1. 
				 * TODO:
				 * Also, we want a way to notify the user of what action we just performed... 
				 * This would be a good place to get a part of that status.  Any Ideas?  */
				if(match.Success)
				{
					if(returnValue > -1)
					{
						//at this point we know we've changed some rows, lets tell the user
					}else
					{
						//something went WRONG! LOG IT!
						log.Error("Something when wrong while executing the following SQL command:\n" + script.ScriptText + 
							"\nreturnValue =" + returnValue);
						return false;
					}
				}
				else
				{
					//we probably did a CREATE, ALTER, DROP, etc... 
				}
			}
			return true;
		}

		/// <summary>
		/// Unpacks an embedded script into a string.
		/// </summary>
		/// <param name="scriptName">The file name of the script to run.</param>
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
