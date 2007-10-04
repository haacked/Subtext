using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Principal;
using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Installation;
using Subtext.Scripting;
using UnitTests.Subtext;

[assembly: AssemblyCleanup(typeof(AssemblySetUpAndCleanUp))]
namespace UnitTests.Subtext
{
	public static class AssemblySetUpAndCleanUp
	{
		[SetUp]
		public static void SetUp()
		{
			Console.WriteLine("Assembly Setup beginning...");
			if (ConfigurationManager.AppSettings["connectionStringName"] == "subtextExpress")
			{
				//For use with SQL Express. If you use "subtextData", we assume you already have the database created.
				CreateAndInstallDatabase();
			}
			else
			{
				using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
				{
					connection.Open();
					using (SqlTransaction transaction = connection.BeginTransaction())
					{
						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction);
						transaction.Commit();
					}
				}
			}
		}

		[TearDown]
		public static void TearDown()
		{
			if (ConfigurationManager.AppSettings["connectionStringName"] == "subtextExpress")
			{
				try
				{
					DeleteDatabase(Config.ConnectionString.Server, "Subtext_Tests");
				}
				catch(Exception e)
				{
					Console.WriteLine("Exception occurred while deleting the database. We'll get it the next time around.");
					Console.WriteLine(e);

				}
			}
		}

		private static void CreateAndInstallDatabase()
		{
			ConnectionString connectionString = Config.ConnectionString;

			DeleteDatabase(connectionString.Server, "Subtext_Tests");
			Console.Write("Creating Database 'Subtext_Tests'");
			CreateDatabase(connectionString.Server, "Subtext_Tests");

			Console.Write("Installing Subtext onto Subtext_Tests");

			SqlInstaller installer = new SqlInstaller(connectionString);
			installer.Install(VersionInfo.FrameworkVersion);
		}

		private static void DeleteDatabase(string serverName, string databaseName)
		{
			Console.WriteLine(@"Attempting to delete database '{0}\{1}", serverName, databaseName);
			DetachDatabase(serverName, databaseName);
			
			DeleteFile(Path.Combine(Path.GetFullPath(@"App_Data"), databaseName + ".mdf"));
			DeleteFile(Path.Combine(Path.GetFullPath(@"App_Data"), databaseName + ".ldf"));
		}

		private static void DeleteFile(string path)
		{
			try
			{
				if (File.Exists(path))
					File.Delete(path);
			}
			catch(Exception e)
			{
				Console.WriteLine("Could not delete '{0}'", path);
				Console.WriteLine(e);
			}
		}

		private static void CreateDatabase(string serverName, string databaseName)
		{
			if (!Directory.Exists("App_Data"))
				Directory.CreateDirectory("App_Data");

			Server server = null;
			try
			{
				server = new Server(serverName);
				
				Database db = CreateDb(server, databaseName);
				
				WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();

				CreateSqlUsersGroup(server, db);
				CreateSqlLoginForCurrentUser(server, db, currentIdentity);
				CreateSqlUserForCurrentUser(db, currentIdentity);

				server.Logins[currentIdentity.Name].AddToRole("sysadmin");
			}
			finally
			{
				if(server != null)
					server.ConnectionContext.SqlConnectionObject.Close();
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
				catch(Exception e)
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
			catch(Exception e)
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
					catch(FailedOperationException e)
					{
						Console.WriteLine(e);
						Console.WriteLine("Trying to continue...");
					}
				}

				if(!db.Users.Contains("Users"))
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

		private static Database CreateDb(Server server, string databaseName)
		{
			Database db = new Database(server, databaseName);
				
			db.DatabaseOptions.AutoClose = true;
			db.DatabaseOptions.AutoShrink = true;
			db.DatabaseOptions.UserAccess = DatabaseUserAccess.Multiple;

			FileGroup fileGroup = new FileGroup(db, "PRIMARY");
			db.FileGroups.Add(fileGroup);
			DataFile dataFile = new DataFile(fileGroup, databaseName + "_Data");
			fileGroup.Files.Add(dataFile);
			dataFile.FileName = Path.Combine(Path.GetFullPath(@"App_Data"), databaseName + ".mdf");
			dataFile.Size = 5.0*1024.0;
			dataFile.Growth = 10.0;
			dataFile.GrowthType = FileGrowthType.Percent;

			LogFile logFile = new LogFile(db, databaseName + "_Log");
			db.LogFiles.Add(logFile);
			logFile.FileName = Path.Combine(Path.GetFullPath(@"App_Data"), databaseName + ".ldf");
			logFile.Size = 2.5*1024.0;
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
			catch(Exception e)
			{
				Console.WriteLine("Could not alter the database.");
				Console.WriteLine(e);
			}

			try
			{
				Console.WriteLine("Detaching existing database before restore ...");
				server.DetachDatabase(db.Name, false);
			}
			catch(Exception e)
			{
				Console.WriteLine("Detach failed, let's continue anyways.");
				Console.WriteLine(e);
			}
		}
	}
}
