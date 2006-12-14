using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Subtext.Scripting;
using Subtext.Scripting.Exceptions;

namespace UnitTests.Subtext.Scripting
{
	[TestFixture]
	public class SqlScriptRunnerTests
	{
		[Test]
		public void CanCreateSriptWitScriptText()
		{
			SqlScriptRunner runner = new SqlScriptRunner("Select * from subtext_Content" + Environment.NewLine + "GO" + Environment.NewLine + "SELECT * FROM subtext_Feedback");
			Assert.AreEqual(2, runner.ScriptCollection.Count);
		}

		[Test]
		public void CanCreateSriptWithAssemblyAndScopingType()
		{
			SqlScriptRunner runner = new SqlScriptRunner(Assembly.GetExecutingAssembly(), typeof(UnitTestHelper), "Resources.Scripting.SqlRunnerTestScript.txt", Encoding.UTF8);
			Assert.AreEqual(2, runner.ScriptCollection.Count);
		}

		[Test]
		public void CanCreateSriptWithScopingType()
		{
			SqlScriptRunner runner = new SqlScriptRunner(typeof(UnitTestHelper), "Resources.Scripting.SqlRunnerTestScript.txt", Encoding.UTF8);
			Assert.AreEqual(2, runner.ScriptCollection.Count);
		}

		[Test]
		public void ExecuteSetsRecordsAffectedForUpdate()
		{
			string script = "UPDATE subtext_Version SET DateCreated = getdate()";
			SqlScriptRunner runner = new SqlScriptRunner(script);
			Assert.IsTrue(ExecuteScriptRunner(runner) > 0);
		}

		[Test]
		public void ExecuteSetsRecordsAffectedForToZeroForStoredProc()
		{
			string script = "CREATE PROC unittest_GetVersion AS SELECT * FROM subtext_Version";
			SqlScriptRunner runner = new SqlScriptRunner(script);
			Assert.AreEqual(0, ExecuteScriptRunner(runner));
		}

		[Test]
		public void ScriptProgressEventRaised()
		{
			SqlScriptRunner runner = new SqlScriptRunner(Assembly.GetExecutingAssembly(), "UnitTests.Subtext.Resources.Scripting.SqlRunnerTestScript.txt", Encoding.UTF8);
			Assert.AreEqual(2, runner.ScriptCollection.Count);

			bool eventRaised = false;
			int index = 0;
			runner.ScriptProgress += delegate(object sender, ScriptProgressEventArgs args)
			{
				Assert.AreEqual(-1, args.RowsAffectedCount);
				Assert.AreEqual(index + 1, args.ScriptsExecutedCount);
				if (index == 0)
					Assert.AreEqual("SELECT TOP 1 * FROM subtext_Content WITH (NOLOCK)", args.Script.ScriptText);
				if (index == 1)
					Assert.AreEqual("SELECT TOP 1 * FROM subtext_Feedback WITH (NOLOCK)", args.Script.ScriptText);

				eventRaised = true;
				index++;
			};

			ExecuteScriptRunner(runner);
			Assert.IsTrue(eventRaised, "The event was not raised.");
		}

		private static int ExecuteScriptRunner(SqlScriptRunner runner)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString))
			{
				conn.Open();
				using (SqlTransaction transaction = conn.BeginTransaction())
				{
					return runner.Execute(transaction);
				}
			}
		}

		[Test]
		[ExpectedException(typeof(SqlScriptExecutionException))]
		public void ExecuteThrowsSqlScriptExecutionException()
		{
			SqlScriptRunner runner = new SqlScriptRunner("SELECT * FROM subtext_NonExistentTable");
			ExecuteScriptRunner(runner);
		}

		[Test]
		public void SqlScriptExecutionExceptionIsSerializable()
		{
			SqlScriptExecutionException e = new SqlScriptExecutionException("Testing", new Script("Blah"), 42, null);
			Assert.AreEqual(42, e.ReturnValue);
			
			SqlScriptExecutionException roundTripped = UnitTestHelper.SerializeRoundTrip(e);
			Assert.AreEqual(e.ReturnValue, roundTripped.ReturnValue);
			Assert.AreEqual(e.Message, roundTripped.Message);			
		}

		[Test]
		public void SqlScriptExecutionExceptionConstructorTests()
		{
			SqlScriptExecutionException e = new SqlScriptExecutionException();
			Assert.IsNull(e.InnerException);

			e = new SqlScriptExecutionException("Test");
			Assert.IsNull(e.InnerException);

			e = new SqlScriptExecutionException("Test", new Exception());
			Assert.IsNotNull(e.InnerException);

			e = new SqlScriptExecutionException("message", new Script("test"), 43);
			Assert.AreEqual("test", e.Script.ScriptText);
			Assert.AreEqual(43, e.ReturnValue);
		}
	}
}
