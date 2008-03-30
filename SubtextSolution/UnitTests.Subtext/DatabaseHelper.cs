using System;
using System.IO;
using System.Security.Principal;
using Microsoft.SqlServer.Management.Smo;
using Subtext.Framework;
using Subtext.Installation;
using Subtext.Scripting;

namespace UnitTests.Subtext
{
	public static class DatabaseHelper
	{
		public static void CreateAndInstallDatabase(ConnectionString connectionString, string databaseName, string directory)
		{
			DeleteDatabase(connectionString.Server, databaseName, directory);
			CreateDatabase(connectionString.Server, databaseName, directory);

			SqlInstaller installer = new SqlInstaller(connectionString);
			installer.Install(VersionInfo.FrameworkVersion);
		}

		public static void CreateDatabase(string serverName, string databaseName, string directory)
		{
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			Server server = null;
			try
			{
				server = new Server(serverName);

				Database db = CreateDb(server, databaseName, directory);

				WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();

				CreateSqlUsersGroup(server, db);
				CreateSqlLoginForCurrentUser(server, db, currentIdentity);
				CreateSqlUserForCurrentUser(db, currentIdentity);

				server.Logins[currentIdentity.Name].AddToRole("sysadmin");
			}
			finally
			{
				if (server != null)
					server.ConnectionContext.SqlConnectionObject.Close();
			}
		}

		public static void InstallDatabase(string connectionString)
		{
			SqlInstaller installer = new SqlInstaller(connectionString);
			installer.Install(VersionInfo.FrameworkVersion);
		}

		public static void DeleteDatabase(string serverName, string databaseName, string directory)
		{
			DetachDatabase(serverName, databaseName);

			DeleteDatabaseFiles(databaseName, directory);
		}

		public static void DeleteDatabaseFiles(string databaseName, string directory)
		{
			Console.WriteLine(@"Attempting to delete database files '{0} and {1}", databaseName + ".mdf", databaseName + ".ldf");
			DeleteFile(Path.Combine(Path.GetFullPath(directory), databaseName + ".mdf"));
			DeleteFile(Path.Combine(Path.GetFullPath(directory), databaseName + ".ldf"));
		}

		private static void DeleteFile(string path)
		{
			try
			{
				if (File.Exists(path))
					File.Delete(path);
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not delete database file '{0}'", path);
				Console.WriteLine(e);
			}
		}

		private static void CreateSqlUserForCurrentUser(Database db, IIdentity currentIdentity)
		{
			User user = FindUserByLogin(db, currentIdentity.Name);
			if (user == null)
			{
				try
				{
					Console.WriteLine("CREATING USER");
					CreateUser(db, currentIdentity.Name, currentIdentity.Name);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					Console.WriteLine("Could not create sql user for current identity.");
				}
			}
		}

		private static void CreateSqlLoginForCurrentUser(Server server, NamedSmoObject db, IIdentity identity)
		{
			try
			{
				if (!server.Logins.Contains(identity.Name))
				{
					CreateLogin(server, db, identity.Name);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine("Could not create login for current user.");
			}
		}

		private static void CreateSqlUsersGroup(Server server, Database db)
		{
			try
			{
				if (!server.Logins.Contains(@"BUILTIN\Users"))
				{
					try
					{
						CreateLogin(server, db, @"BUILTIN\Users");
					}
					catch (FailedOperationException e)
					{
						Console.WriteLine(e);
						Console.WriteLine("Trying to continue...");
					}
				}

				if (!db.Users.Contains("Users"))
				{
					CreateUser(db, @"BUILTIN\Users", "Users");
				}
				db.Roles["db_owner"].AddMember("Users");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine("Could not create 'Users' group. Continuing...");
			}
		}

		private static Database CreateDb(Server server, string databaseName, string directory)
		{
			Database db = new Database(server, databaseName);

			db.DatabaseOptions.AutoClose = true;
			db.DatabaseOptions.AutoShrink = true;
			db.DatabaseOptions.UserAccess = DatabaseUserAccess.Multiple;

			FileGroup fileGroup = new FileGroup(db, "PRIMARY");
			db.FileGroups.Add(fileGroup);
			DataFile dataFile = new DataFile(fileGroup, databaseName + "_Data");
			fileGroup.Files.Add(dataFile);
			dataFile.FileName = Path.Combine(Path.GetFullPath(directory), databaseName + ".mdf");
			dataFile.Size = 5.0 * 1024.0;
			dataFile.Growth = 10.0;
			dataFile.GrowthType = FileGrowthType.Percent;

			LogFile logFile = new LogFile(db, databaseName + "_Log");
			db.LogFiles.Add(logFile);
			logFile.FileName = Path.Combine(Path.GetFullPath(directory), databaseName + ".ldf");
			logFile.Size = 2.5 * 1024.0;
			logFile.GrowthType = FileGrowthType.Percent;
			logFile.Growth = 10.0;

			db.Create(false);
			return db;
		}

		private static User FindUserByLogin(Database db, string login)
		{
			foreach (User user in db.Users)
			{
				if (user.Login == login)
				{
					return user;
				}
			}
			return null;
		}

		private static void CreateLogin(Server server, NamedSmoObject db, string loginName)
		{
			Login login = new Login(server, loginName);
			login.DefaultDatabase = db.Name;
			login.LoginType = LoginType.WindowsUser;
			login.AddToRole("sysadmin");
			login.Create();
		}

		private static void CreateUser(Database db, string login, string userName)
		{
			User user = new User(db, userName);
			user.Login = login;
			user.Create();
		}

		private static void DetachDatabase(string serverName, string databaseName)
		{
			Console.WriteLine(@"Attempting to detach database '{0}\{1}", serverName, databaseName);
			// Initialise server object.
			Server server = new Server(serverName);

			// Check if database is current attached to sqlexpress.
			if (!server.Databases.Contains(databaseName))
			{
				Console.WriteLine("Server does not contain db '{0}'", databaseName);
				return;
			}
			Database db = server.Databases[databaseName];

			try
			{
				DataFile dataFile = db.FileGroups[0].Files[0];
				if (!File.Exists(dataFile.FileName))
				{
					Console.WriteLine("'{0}' does not exist. Attempting to detach without altering db..", dataFile.FileName);
				}
				db.DatabaseOptions.UserAccess = DatabaseUserAccess.Single;
			}
			catch (Exception e)
			{
				Console.WriteLine("Execution failure exception. Continuing.");
				Console.WriteLine(e);
			}

			try
			{
				server.KillAllProcesses(db.Name);
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not kill all processes");
				Console.WriteLine(e);
			}

			Console.WriteLine("Altering database '{0}'", db.Name);
			try
			{
				db.Alter(TerminationClause.FailOnOpenTransactions);
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not alter the database.");
				Console.WriteLine(e);
			}

			try
			{
				Console.WriteLine("Detaching existing database before restore ...");
				server.DetachDatabase(db.Name, false);
			}
			catch (Exception e)
			{
				Console.WriteLine("Detach failed, let's continue anyways.");
				Console.WriteLine(e);
			}
		}
	}
}
