using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Subtext.Scripting.Exceptions;

namespace Subtext.Scripting
{
	/// <summary>
	/// Represents a single executable script within the full SQL script.
	/// //TODO: want to implement a means to evaluate template variables 
	///			like in Sql Query Analyzer.
	/// </summary>
	public class Script : IScript
	{
		/// <summary>
		/// Helper method which given a full SQL script, returns 
		/// a <see cref="ScriptCollection"/> of individual <see cref="TemplateParameter"/> 
		/// using "GO" as the delimiter.
		/// </summary>
		/// <param name="fullScriptText">Full script text.</param>
		public static ScriptCollection ParseScripts(string fullScriptText)
		{
			Regex regex = new Regex(@"(^\s*|\s+)GO(\s+|\s*$)",  RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			string[] scriptTexts = regex.Split(fullScriptText);
			ScriptCollection scripts = new ScriptCollection(fullScriptText);
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
		/// Creates a new <see cref="TemplateParameter"/> instance.
		/// </summary>
		/// <param name="scriptText">Script text.</param>
		public Script(string scriptText)
		{
			_scriptText = scriptText;
		}

		/// <summary>
		/// Gets the script text.
		/// </summary>
		/// <value></value>
		public string ScriptText
		{
			get { return _scriptText; }
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
				throw new SqlScriptExecutionException("Error in executing the script: ", this, 0, e);
			}
		}
	}
}
