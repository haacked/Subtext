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
using System.Text.RegularExpressions;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using Subtext.Scripting;

namespace Subtext.DotTextUpgrader
{
	/// <summary>
	/// Imports blog data from a .TEXT 0.95 database.
	/// </summary>
	public class DotText095Upgrader
	{
		/// <summary>
		/// Upgrades the .TEXT blog to Subtext.
		/// </summary>
		/// <returns></returns>
		public bool Upgrade(string connectionString)
		{
			InstallSubtextTables(connectionString);
			TransferData(connectionString);
			DeployFiles();
			DeployWebConfig(connectionString);
		
			return true;
		}

		private void InstallSubtextTables(string connectionString)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						SqlScriptRunner schemaScriptRunner = new SqlScriptRunner(Script.ParseScripts(this.GetSchemaScriptContents()));
						schemaScriptRunner.Execute(transaction);
						transaction.Commit();
					}
					catch(Exception)
					{
						if(transaction != null)
							transaction.Rollback();
					}
				}
				
			}
		}

		private void TransferData(string connectionString)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						// Hmmm... we can't assume that the .TEXT database is on the same 
						// server (or database) as our database.  We might have to do a 
						// cross database join.  We might need to do something more tricky here.
						SqlScriptRunner scriptRunner = new SqlScriptRunner(Script.ParseScripts(GetImportScriptContents()));

						// For now, the tables are installed into the same database.
						ConnectionString connectionInfo = ConnectionString.Parse(connectionString);

						scriptRunner.TemplateParameters["subtext_db_name"].Value = connectionInfo.Database;
						scriptRunner.TemplateParameters["dottext_db_name"].Value = connectionInfo.Database;
						
						scriptRunner.Execute(transaction);
						SqlScriptRunner spScriptRunner = new SqlScriptRunner(Script.ParseScripts(GetStoredProcScriptContents()));
						spScriptRunner.Execute(transaction);
						transaction.Commit();
					}
					catch(Exception)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}
		
		/// <summary>
		/// Deploys the zipped files.
		/// </summary>
		public void DeployFiles()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(typeof(DotText095Upgrader), "SubtextDeployment.zip");
			if(stream == null)
				throw new InvalidOperationException("Ok, this shouldn't happen. We messed up the resource name.");
			ExtractArchive(stream, HttpContext.Current.Request.PhysicalApplicationPath);
		}

		/// <summary>
		/// Updates the connection string in the new Web.config
		/// </summary>
		public void DeployWebConfig(string connectionString)
		{
			string webConfig = GetEmbeddedContents("Web.config");
			Regex regex = new Regex(@"<add\s+key\s*=\s*""ConnectionString""\s*value\s*=\s*""(?<conn>[^""]+)""\s*/>", RegexOptions.IgnoreCase);
			webConfig = regex.Replace(webConfig, string.Format(@"<add key=""ConnectionString"" value=""{0}"" />", connectionString));
			WriteFile(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Web.config"), webConfig);
		}

		void ExtractArchive(Stream compressedArchive, string targetDirectory)
		{
			using(ZipInputStream inputStream = new ZipInputStream(compressedArchive))
			{
				ZipEntry nextEntry = inputStream.GetNextEntry();
				if(!Directory.Exists(targetDirectory))
				{
					Directory.CreateDirectory(targetDirectory);
				}
				while(nextEntry != null)
				{
					if(nextEntry.IsDirectory)
					{
						Directory.CreateDirectory(Path.Combine(targetDirectory, nextEntry.Name));
					}
					else
					{
						if(!Directory.Exists(Path.Combine(targetDirectory, Path.GetDirectoryName(nextEntry.Name))))
						{
							Directory.CreateDirectory(Path.Combine(targetDirectory, Path.GetDirectoryName(nextEntry.Name)));
						}

						ExtractFile(targetDirectory, nextEntry, inputStream);						
					}
					nextEntry = inputStream.GetNextEntry();
				}
			}
		}

		private static void ExtractFile(string targetDirectory, ZipEntry nextEntry, ZipInputStream inputStream)
		{
			using(FileStream fileStream = new FileStream(Path.Combine(targetDirectory, nextEntry.Name), FileMode.OpenOrCreate, FileAccess.Write))
			{
				byte[] buffer = new byte[4096];
				int count = inputStream.Read(buffer, 0, 4096);
				while(count > 0)
				{
					fileStream.Write(buffer, 0, count);
					count = inputStream.Read(buffer, 0, 4096);
				}
				fileStream.Flush();
			}
		}

		void WriteFile(string path, string contents)
		{
			using(StreamWriter writer = new StreamWriter(path))
			{
				writer.Write(contents);
			}
		}

		public string GetSchemaScriptContents()
		{
			return GetEmbeddedContents("Installation.01.00.00.sql");
		}

		public string GetImportScriptContents()
		{
			return GetEmbeddedContents("ImportDotText095.sql");
		}

		public string GetStoredProcScriptContents()
		{			
			return GetEmbeddedContents("StoredProcedures.sql");
		}

		string GetEmbeddedContents(string scriptName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(typeof(DotText095Upgrader), "Scripts." + scriptName);
			using(StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
