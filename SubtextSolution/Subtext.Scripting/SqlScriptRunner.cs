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

using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Subtext.Scripting.Exceptions;

namespace Subtext.Scripting
{
	/// <summary>
	/// Class used to manage and execute SQL scripts.  
	/// Can also be used to hand
	/// </summary>
	public class SqlScriptRunner : IScript, ITemplateScript
	{
		ScriptCollection scripts;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptRunner"/> class.  
		/// Initializes the script to execute.
		/// </summary>
		/// <p>
		/// Suppose an assembly Foo.dll contains an embedded resource "Bar.sql" in a folder 
		/// named "Scripts".  To execute the embedded script, pass in any type within the 
		/// namespace "Foo" and pass the scriptname of "Scripts.Bar.sql".  Or pass in a type 
		/// in the namespace "Foo.Scripts" and pass in the scriptname of "Bar.sql".
		/// </p>
		/// <param name="scopingType">
		///	A type whose assembly contains the script as an embedded resource. 
		///	Also used to scope the script name. See remarks.
		/// </param>
		/// <param name="scriptName">Name of the script.</param>
		/// <param name="encoding">The encoding.</param>
		public SqlScriptRunner(Type scopingType, string scriptName, Encoding encoding) : this(UnpackEmbeddedScript(scopingType, scriptName), encoding)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptRunner"/> class.
		/// </summary>
		/// <p>
		/// Suppose an assembly Foo.dll contains an embedded resource "Bar.sql" in a folder 
		/// named "Scripts".  To execute the embedded script, pass in any type within the 
		/// namespace "Foo" and pass the scriptname of "Scripts.Bar.sql".  Or pass in a type 
		/// in the namespace "Foo.Scripts" and pass in the scriptname of "Bar.sql".
		/// </p>
		/// <param name="assemblyWithEmbeddedScript">The assembly containing the script as an embedded resource.</param>
		/// <param name="scopingType">
		///	Used to scope the script name within the embedded resource.
		/// </param>
		/// <param name="scriptName">Name of the script.</param>
		/// <param name="encoding">The encoding.</param>
		public SqlScriptRunner(Assembly assemblyWithEmbeddedScript, Type scopingType, string scriptName, Encoding encoding) : this(UnpackEmbeddedScript(assemblyWithEmbeddedScript, scopingType, scriptName), encoding)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptRunner"/> class.
		/// Initializes the script to execute.
		/// </summary>
		/// <param name="assemblyWithEmbeddedScript">The assembly with the script as an embedded resource.</param>
		/// <param name="fullScriptName">Fully qualified resource name of the script.</param>
		/// <param name="encoding">The encoding.</param>
		public SqlScriptRunner(Assembly assemblyWithEmbeddedScript, string fullScriptName, Encoding encoding) : this(UnpackEmbeddedScript(assemblyWithEmbeddedScript, fullScriptName), encoding)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptRunner"/> class.
		/// </summary>
		/// <param name="scriptStream">The stream containing the script to execute.</param>
		/// <param name="encoding">The encoding.</param>
		public SqlScriptRunner(Stream scriptStream, Encoding encoding) : this(ReadStream(scriptStream, encoding))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptRunner"/> class.
		/// </summary>
		/// <param name="scriptText">The full script text to execute.</param>
		public SqlScriptRunner(string scriptText) : this(Script.ParseScripts(scriptText))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptRunner"/> class.
		/// </summary>
		/// <param name="scripts">The scripts.</param>
		public SqlScriptRunner(ScriptCollection scripts)
		{
			this.scripts = scripts;
		}

		/// <summary>
		/// Gets the script collection this runner is executing.
		/// </summary>
		/// <value>The script collection.</value>
		public ScriptCollection ScriptCollection
		{
			get
			{
				return scripts;
			}
		}

		/// <summary>
		/// Executes the script.
		/// </summary>
		/// <remarks>
		/// Use script.Execute(transaction) to do the work. We will also pull the
		/// status of our script exection from here.
		/// </remarks>
		/// <param name="transaction">The current transaction.</param>
		public int Execute(SqlTransaction transaction)
		{
            int recordsAffectedTotal = 0;
            SetNoCountOff(transaction);

			// the following reg exp will be used to determine if each script is an
			// INSERT, UPDATE, or DELETE operation. The reg exp is also only looking
			// for these actions on the SubtextData database. <- do we need this last part?
			string regextStr = @"(INSERT\sINTO\s[\s\w\d\)\(\,\.\]\[\>\<]+)|(UPDATE\s[\s\w\d\)\(\,\.\]\[\>\<]+SET\s)|(DELETE\s[\s\w\d\)\(\,\.\]\[\>\<]+FROM\s[\s\w\d\)\(\,\.\]\[\>\<]+WHERE\s)";
			Regex regex = new Regex(regextStr, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
	
			scripts.ApplyTemplatesToScripts();
			foreach(Script script in scripts)
			{
				int returnValue = script.Execute(transaction);
				
				Match match = regex.Match(script.ScriptText);
				if (match.Success)
				{
					/* 
					 * For UPDATE, INSERT, and DELETE statements, the return value is the 
					 * number of rows affected by the command. For all other types of statements, 
					 * the return value is -1. If a rollback occurs, the return value is also -1. 
					 */
					if (!IsCrudScript(script))
					    continue;

                    if (returnValue > -1)
                    {
                        recordsAffectedTotal += returnValue;
                    }
                    else
                    {
                        throw new SqlScriptExecutionException("An error occurred while executing the script.", script, returnValue);
                    }
				}
			}
			return recordsAffectedTotal;
		}

        private static bool IsCrudScript(Script script)
        {
            return script.ScriptText.IndexOf("TRIGGER") == -1 && script.ScriptText.IndexOf("PROC") == -1;
        }

        /// <summary>
        /// Temporarily set NOCOUNT OFF on the connection. We must do this b/c the SqlScriptRunner 
        /// depends on all CRUD statements returning the number of effected rows to determine if an 
        /// error occured. This isn't a perfect solution, but it's what we've got.
        /// </summary>
        /// <param name="transaction"></param>
        private static void SetNoCountOff(SqlTransaction transaction)
        {
            Script noCount = new Script("SET NOCOUNT OFF");
            noCount.Execute(transaction);
        }

		#region ... Methods for unpacking embedded resources and reading from streams ...
		static string ReadStream(Stream stream, Encoding encoding)
		{
			using(StreamReader reader = new StreamReader(stream, encoding))
			{
				return reader.ReadToEnd();
			}
		}

		static Stream UnpackEmbeddedScript(Type scopingType, string scriptName)
		{
			Assembly assembly = scopingType.Assembly;
			return assembly.GetManifestResourceStream(scopingType, scriptName);
		}

		static Stream UnpackEmbeddedScript(Assembly assembly, Type scopingType, string scriptName)
		{
			return assembly.GetManifestResourceStream(scopingType, scriptName);
		}

		static Stream UnpackEmbeddedScript(Assembly assembly, string fullScriptName)
		{
			return assembly.GetManifestResourceStream(fullScriptName);
		}
		#endregion

		/// <summary>
		/// Gets the template parameters embedded in the script.
		/// </summary>
		/// <returns></returns>
		public TemplateParameterCollection TemplateParameters
		{
			get
			{
				return this.scripts.TemplateParameters;
			}
		}
	}
}
