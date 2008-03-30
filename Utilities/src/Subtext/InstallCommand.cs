using System;
using Subtext.Framework;
using Subtext.Installation;
using Microsoft.SqlServer.Management.Smo;
using Subtext.Scripting;

namespace Subtext
{
	public class InstallCommand : Command
	{
		protected override void ExecuteCommand(Arguments arguments)
		{
			if (arguments.Contains("recreate-db"))
			{
				DropDatabase(arguments["connect"]);
			}

			if (arguments.Contains("create-db") || arguments.Contains("recreate-db"))
			{
				CreateDatabase(arguments["connect"]);
			}

			SqlInstaller installer = new SqlInstaller(arguments["connect"]);
			installer.Install(VersionInfo.FrameworkVersion);
		}

		private static void CreateDatabase(string connectionString)
		{
			ConnectionString conn = ConnectionString.Parse(connectionString);
			Server server = new Server(conn.Server);
			Database db = server.Databases[conn.Database];
			if(db != null)
				throw new InvalidOperationException(String.Format("Attempting to create database '{0}' failed because it already exists.", conn.Database));

			db = new Database(new Server(conn.Server), conn.Database);
			db.Create();
		}

		private static void DropDatabase(string connectionString)
		{
			ConnectionString conn = ConnectionString.Parse(connectionString);
			Server server = new Server(conn.Server);
			Database db = server.Databases[conn.Database];
			if (db != null)
				db.Drop();
			else
				Console.WriteLine("Attempting to drop database '{0}' failed because it does not exist. Continuing.", conn.Database);
		}
	}
}
