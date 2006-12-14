using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Subtext.Scripting;

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

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString))
			{
				conn.Open();
				using (SqlTransaction transaction = conn.BeginTransaction())
				{
					runner.Execute(transaction);
				}
			}
			Assert.IsTrue(eventRaised, "The event was not raised.");
		}
	}
}
