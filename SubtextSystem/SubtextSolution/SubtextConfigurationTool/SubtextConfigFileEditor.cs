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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtextConfigurationTool
{
	/// <summary>
	/// Summary description for SubtextConfigFileEditor.
	/// </summary>
	public class SubtextConfigFileEditor
	{
		string configFilePath;
		string contents;
		string oldConnectionString;
		int connectionStringIndex;
		int connectionStringLength;
		SqlConnectionString connectionString;

		/// <summary>
		/// Creates a new <see cref="SubtextConfigFileEditor"/> instance.
		/// </summary>
		/// <param name="configFilePath">Config file path.</param>
		public SubtextConfigFileEditor(string configFilePath)
		{
			if(configFilePath == null)
				throw new ArgumentNullException("configFilePath", "Path is null");
			
			if(configFilePath.Length == 0)
				throw new ArgumentException("Path is empty.", "configFilePath");
			
			if(!File.Exists(configFilePath))
				throw new FileNotFoundException("Could not find the configuration file", configFilePath);

			this.configFilePath = configFilePath;

			this.contents = ReadFile(configFilePath);
			// we'll take a shortcut here. There's only one place where we configure 
			// a connection string in the form of connectionString="..."
			Regex connectStrRegex = new Regex(@"\sconnectionString\s*=\s*""(?<connStr>[^""]*)""", RegexOptions.Compiled);
			Match match = connectStrRegex.Match(this.contents);
			if(match.Success)
			{
				this.connectionStringIndex = match.Groups["connStr"].Index;
				this.connectionStringLength = match.Groups["connStr"].Length;
				this.connectionString = new SqlConnectionString(match.Groups["connStr"].Value);
			}
			oldConnectionString = this.connectionString.ConnectionString;
		}

		string ReadFile(string path)
		{
			using(StreamReader reader = new StreamReader(path))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <value></value>
		public SqlConnectionString ConnectionString
		{
			get
			{
				return connectionString;
			}
		}

		/// <summary>
		/// Overwrites the config file with the new connection string.
		/// </summary>
		public void Save()
		{
			if(oldConnectionString != this.connectionString.ConnectionString)
			{
				string newContents = this.contents.Substring(0, this.connectionStringIndex)
					+ this.connectionString.ConnectionString
					+ this.contents.Substring(this.connectionStringIndex + this.connectionStringLength);

				using(StreamWriter writer = new StreamWriter(this.configFilePath, false, Encoding.UTF8))
				{
					writer.Write(newContents);
				}
			}
		}
	}
}
