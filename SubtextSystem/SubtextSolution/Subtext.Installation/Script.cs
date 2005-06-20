using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Subtext.Installation
{
	/// <summary>
	/// Summary description for Script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// Helper method which given a full SQL script, returns 
		/// a <see cref="ScriptCollection"/> of individual <see cref="Script"/> 
		/// using "GO" as the delimiter.
		/// </summary>
		/// <param name="fullScriptText">Full script text.</param>
		public static ScriptCollection ParseScripts(string fullScriptText)
		{
			Regex regex = new Regex(@"(^\s*|\s+)GO(\s*$|\s+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			string[] scriptTexts = regex.Split(fullScriptText);
			ScriptCollection scripts = new ScriptCollection();
			foreach(string scriptText in scriptTexts)
			{
				if(scriptText.Trim() != string.Empty)
				{
					scripts.Add(new Script(scriptText));
				}

			}
			return scripts;
		}

		string _scriptText;
		/// <summary>
		/// Creates a new <see cref="Script"/> instance.
		/// </summary>
		/// <param name="scriptText">Script text.</param>
		public Script(string scriptText)
		{
			_scriptText = scriptText;
		}

		/// <summary>
		/// Executes this script.
		/// </summary>
		public int Execute(SqlTransaction transaction)
		{
			try
			{
				return SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, this._scriptText);
			}
			catch(SqlException e)
			{
				e.Source = this._scriptText;
				throw;
			}
		}
	}
}
