using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using MbUnit.Framework;
using Subtext.Scripting;
using UnitTests.Subtext;
using WatinTests;
using System.ComponentModel;

[assembly: AssemblyCleanup(typeof(AssemblySetupAndTearDown))]
namespace WatinTests
{
	public static class AssemblySetupAndTearDown
	{
		private static Process webserverProcess = null;
		static readonly string subtextWebPath = Path.GetFullPath(@"..\..\..\Subtext.Web");
		static readonly string subtextDatabasePath = Path.Combine(subtextWebPath, "App_Data");
		private static readonly string webConfigPath = Path.Combine(subtextWebPath, "Web.config");

		private static readonly ConnectionString connectionString = ConnectionString.Parse(ConfigurationManager.AppSettings["connectionString"]);

		[SetUp]
		public static void SetUp()
		{
			//For Subtext.Web's config
			BackupWebConfig();

			//Point it to the db we're going to create.
			ChangeWebConfigDatabase();

			//This creates a new database in the App_Data directory of Subtext.Web
			CreateDatabase();

			//Start the web server.
			StartWebServer();

			//Run through the installation process.
			using (Browser browser = new Browser(Browser.GetUrl("/")))
			{
				browser.StepThroughInstallation("username", "password");
				browser.Login("username", "password");
				Assert.IsTrue(browser.ContainsText("Configure"), "We should be in the admin tool.");
			}
		}

		[TearDown]
		public static void TearDown()
		{
			try
			{
				if (webserverProcess != null)
				{
					webserverProcess.Kill();
					webserverProcess.Close();
					DatabaseHelper.DeleteDatabase(connectionString.Server, connectionString.Database, subtextDatabasePath);
				}
			}
			finally
			{
				RestoreWebConfigBackup();
			}
		}

		private static void StartWebServer()
		{
            string webServerWebDevPath = @"C:\Program Files\Common Files\microsoft shared\DevServer\9.0\Webdev.WebServer.exe";
			string commandLineArgs = String.Format(@"/port:{0} /path:""{1}""", ConfigurationManager.AppSettings["Port"], subtextWebPath);
			ProcessStartInfo startInfo = new ProcessStartInfo(webServerWebDevPath, commandLineArgs);
			Console.WriteLine("Starting WebDev.WebServer on port 2733 pointing to '{0}'", subtextWebPath);
            try
            {
                webserverProcess = Process.Start(startInfo);
            }
            catch (Win32Exception)
            { 
                //I really need to look this up in the registry somehow.
                string dotNetDir = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727";
                webServerWebDevPath = Path.Combine(dotNetDir, "WebDev.WebServer.exe");
                startInfo = new ProcessStartInfo(webServerWebDevPath, commandLineArgs);
                webserverProcess = Process.Start(startInfo);
            }
			Console.WriteLine("Webserver Started");
		}

		private static void ChangeWebConfigDatabase()
		{
			XmlHelper.Poke(webConfigPath, "//appSettings/add[@key='connectionStringName']/@value", "subtextData");
            XmlHelper.Poke(webConfigPath, "//connectionStrings/add[@name='subtextData']/@connectionString", connectionString);
			XmlHelper.Poke(webConfigPath, "//BlogEntryEditor/@defaultProvider", "PlainTextBlogEntryEditorProvider");
		}

		private static void BackupWebConfig()
		{
			if (!File.Exists(webConfigPath + ".bak"))
				File.Copy(webConfigPath, webConfigPath + ".bak");
		}

		private static void RestoreWebConfigBackup()
		{
			File.Copy(webConfigPath + ".bak", webConfigPath, true);
		}

		private static void CreateDatabase()
		{
			DatabaseHelper.DeleteDatabase(connectionString.Server, connectionString.Database, subtextDatabasePath);
			DatabaseHelper.CreateDatabase(connectionString.Server, connectionString.Database, subtextDatabasePath);
		}
	}
}
