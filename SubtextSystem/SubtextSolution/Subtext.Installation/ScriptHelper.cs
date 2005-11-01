using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using Subtext.Scripting;

namespace Subtext.Installation
{ 
	/// <summary>
	/// Helper class used to execute SQL Scripts.
	/// </summary>
	public sealed class ScriptHelper
	{
		private ScriptHelper() {}

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
			SqlScriptRunner scriptRunner = new SqlScriptRunner(UnpackEmbeddedScript(scriptName), Encoding.UTF8);
			return scriptRunner.ExecuteScript(transaction);
		}

		/// <summary>
		/// Executes the script.
		/// </summary>
		/// <remarks>
		/// Use script.Execute(transaction) to do the work. We will also pull the
		/// status of our script exection from here.
		/// </remarks>
		/// <param name="scripts">The collection of scripts to execute.</param>
		/// <param name="transaction">The current transaction.</param>
		public static bool ExecuteScript(ScriptCollection scripts, SqlTransaction transaction)
		{
			SqlScriptRunner scriptRunner = new SqlScriptRunner(scripts);
			return scriptRunner.ExecuteScript(transaction);
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
			return assembly.GetManifestResourceStream(typeof(ScriptHelper), "Scripts." + scriptName);
		}
	}
}
