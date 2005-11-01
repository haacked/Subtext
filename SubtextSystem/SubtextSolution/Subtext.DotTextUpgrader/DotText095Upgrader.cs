using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
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
			if(UpgradeDatabase(connectionString))
			{
				DeployFiles();
			}
			return true;
		}

		private bool UpgradeDatabase(string connectionString)
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
						SqlScriptRunner scriptRunner = new SqlScriptRunner(Script.ParseScripts(GetSchemaScriptContents()));
						if(scriptRunner.ExecuteScript(transaction))
						{
							SqlScriptRunner spScriptRunner = new SqlScriptRunner(Script.ParseScripts(GetStoredProcScriptContents()));
							bool result = spScriptRunner.ExecuteScript(transaction);
							if(result)
								transaction.Commit();
							else
								transaction.Rollback();
							return result;
						}						
						transaction.Rollback();
						return false;
					}
					catch(SqlException)
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

						nextEntry = inputStream.GetNextEntry();
					}
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


		public string GetSchemaScriptContents()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(typeof(DotText095Upgrader) + "Scripts.ImportDotText095.sql");
			using(StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		public string GetStoredProcScriptContents()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(typeof(DotText095Upgrader) + "Scripts.StoredProcedures.sql");
			using(StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
