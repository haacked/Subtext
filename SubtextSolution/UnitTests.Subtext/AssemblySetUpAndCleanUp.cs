using System;
using System.Configuration;
using System.Data.SqlClient;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Installation;
using UnitTests.Subtext;

[assembly: AssemblyCleanup(typeof(AssemblySetUpAndCleanUp))]
namespace UnitTests.Subtext
{
	public static class AssemblySetUpAndCleanUp
	{
		[SetUp]
        [CoverageExclude]
		public static void SetUp()
		{
			Console.WriteLine("Assembly Setup beginning...");
			if (ConfigurationManager.AppSettings["connectionStringName"] == "subtextExpress")
			{
				//For use with SQL Express. If you use "subtextData", we assume you already have the database created.
				DatabaseHelper.CreateAndInstallDatabase(Config.ConnectionString, Config.ConnectionString.Database, "App_Data");
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
        [CoverageExclude]
		public static void TearDown()
		{
            try
            {
                if (ConfigurationManager.AppSettings["connectionStringName"] == "subtextExpress")
                {
                    try
                    {
                        DatabaseHelper.DeleteDatabase(Config.ConnectionString.Server, Config.ConnectionString.Database, "App_Data");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception occurred while deleting the database. We'll get it the next time around.");
                        Console.WriteLine(e);

                    }
                }
            }
            catch (Exception) { }
		}
	}
}
