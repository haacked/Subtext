#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

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
	public static class ScriptHelper
	{
		/// <summary>
		/// Executes the script.
		/// </summary>
		/// <remarks>
		/// Use script.Execute(transaction) to do the work. We will also pull the
		/// status of our script exection from here.
		/// </remarks>
		/// <param name="scriptName">Name of the script.</param>
		/// <param name="transaction">The current transaction.</param>
		public static void ExecuteScript(string scriptName, SqlTransaction transaction)
		{
			ExecuteScript(scriptName, transaction, null);
		}

		/// <summary>
		/// Executes the script.
		/// </summary>
		/// <remarks>
		/// Use script.Execute(transaction) to do the work. We will also pull the
		/// status of our script exection from here.
		/// </remarks>
		/// <param name="scriptName">Name of the script.</param>
		/// <param name="transaction">The current transaction.</param>
		/// <param name="dbUserName">Name of the DB owner.</param>
		public static void ExecuteScript(string scriptName, SqlTransaction transaction, string dbUserName)
		{
			SqlScriptRunner scriptRunner = new SqlScriptRunner(UnpackEmbeddedScript(scriptName), Encoding.UTF8);
			if(!string.IsNullOrEmpty(dbUserName))
				scriptRunner.TemplateParameters.SetValue("dbUser", dbUserName);
			scriptRunner.Execute(transaction);
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
		public static void ExecuteScript(ScriptCollection scripts, SqlTransaction transaction)
		{
			ExecuteScript(scripts, transaction, null);
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
		/// <param name="dbUserName">Name of the DB owner.</param>
		public static void ExecuteScript(ScriptCollection scripts, SqlTransaction transaction, string dbUserName)
		{
			SqlScriptRunner scriptRunner = new SqlScriptRunner(scripts);
			if (!string.IsNullOrEmpty(dbUserName))
				scriptRunner.TemplateParameters.SetValue("dbUser", dbUserName);
			scriptRunner.Execute(transaction);
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