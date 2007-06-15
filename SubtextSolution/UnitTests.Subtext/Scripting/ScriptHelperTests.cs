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
using System.Globalization;
using System.IO;
using System.Threading;
using MbUnit.Framework;
using Subtext.Installation;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
	/// <summary>
	/// Summary description for ScriptHelperTests.
	/// </summary>
	[TestFixture]
	public class ScriptHelperTests
	{
		[Test]
		public void ScriptIgnoresGOWithinComments()
		{
			string script = "SELECT * FROM Foo" + Environment.NewLine
							+ "GO" + Environment.NewLine
							+ "SELECT * FROM Bar -- GO HERE" + Environment.NewLine
							+ "/* GO Here */"
							+ "WHERE Id = 1";

			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(2, scripts.Count);
		}

		[RowTest]
		[Row(1, "/* Comment */SELECT * FROM subtext_Content\r\nGO", "SELECT * FROM subtext_Content")]
		[Row(1, "/* Comment */  SELECT * FROM subtext_Content\r\nGO", "SELECT * FROM subtext_Content")]
		[Row(1, "/*\r\n Comment\r\n */\r\n  SELECT * FROM subtext_Content\r\nGO", "SELECT * FROM subtext_Content")]
		[Row(0, "-- EVERYTHING GETS STRIPPED TILL END OF LINE", "")]
		[Row(1, "-- EVERYTHING GETS STRIPPED TILL END OF LINE\r\nSELECT * FROM MyFoot\r\nGO", "SELECT * FROM MyFoot")]
		[Row(1, "SELECT * FROM -- MY FOOT EVERYTHING GETS STRIPPED TILL END OF LINE\r\nMy Foot", "SELECT * FROM My Foot")]
		public void StripsComments(int expectedScriptCount, string scriptText, string expected)
		{
			ScriptCollection scripts = Script.ParseScripts(scriptText);
			Assert.AreEqual(expectedScriptCount, scripts.Count, "This should parse to " + expectedScriptCount + " script.");
			if (expectedScriptCount > 0)
				Assert.AreEqual(expected, scripts[0].ScriptText, "Expected the multi-line comment to be stripped.");
		}
		
		[Test]
		public void ScriptToStringIncludesParameters()
		{
			ScriptCollection scripts = Script.ParseScripts("SELECT TOP <name, int, 0> * FROM Somewhere");
			Assert.AreEqual(1, scripts.Count, "Did not parse the script.");
			Assert.AreEqual(1, scripts.TemplateParameters.Count, "did not merge or parse the template params.");
			
			string expected = @"<ScriptToken length=""0"">" + Environment.NewLine 
				+ @"<ScriptToken length=""11"">" + Environment.NewLine 
				+ @"<TemplateParameter name=""name"" value=""0"" type=""int"" />" + Environment.NewLine
				+ @"<ScriptToken length=""17"">" + Environment.NewLine;


			Assert.AreEqual(expected, scripts[0].ToString());
		}

		/// <summary>
		/// Makes sure that ParseScript parses correctly.
		/// </summary>
		[Test]
		[RollBack2]
		public void ParseScriptParsesCorrectly()
		{
			string script =  @"SET QUOTED_IDENTIFIER OFF " + Environment.NewLine +
				@"Go" + Environment.NewLine + "\t\t" +
				@"SET ANSI_NULLS ON " + Environment.NewLine + Environment.NewLine +
				@"GO" + Environment.NewLine + Environment.NewLine +
				@"GO" + Environment.NewLine + 
				@"SET ANSI_NULLS ON " + Environment.NewLine +
				Environment.NewLine +
				Environment.NewLine +
				@"CREATE TABLE [<username,varchar,dbo>].[blog_Gost] (" + Environment.NewLine +
				"\t" + @"[HostUserName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ," + Environment.NewLine +
				"\t" + @"[Password] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ," + Environment.NewLine +
				"\t" + @"[Salt] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ," + Environment.NewLine +
				"\t" + @"[DateCreated] [datetime] NOT NULL" + Environment.NewLine +
				@") ON [PRIMARY]" + Environment.NewLine +
				@"gO" + Environment.NewLine +
				Environment.NewLine;

			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(3, scripts.Count, "This should parse to three scripts.");
			foreach(Script sqlScript in scripts)
			{
				Assert.IsFalse(sqlScript.ScriptText.StartsWith("GO"));
			}

			string expectedThirdScriptBeginning = "SET ANSI_NULLS ON " 
				+ Environment.NewLine 
				+ Environment.NewLine 
				+ Environment.NewLine + "CREATE TABLE [<username,varchar,dbo>].[blog_Gost]";

			Assert.AreEqual(expectedThirdScriptBeginning, scripts[2].OriginalScriptText.Substring(0, expectedThirdScriptBeginning.Length), "Script not parsed correctly");
	
			scripts.TemplateParameters.SetValue("username", "haacked");
			
			expectedThirdScriptBeginning = "SET ANSI_NULLS ON " 
				+ Environment.NewLine 
				+ Environment.NewLine 
				+ Environment.NewLine + "CREATE TABLE [haacked].[blog_Gost]";

			Assert.AreEqual(expectedThirdScriptBeginning, scripts[2].ScriptText.Substring(0, expectedThirdScriptBeginning.Length), "Script not parsed correctly");
		}

		[Test]
		public void ScriptWithEmptyTextStaysEmptyText()
		{
			Script script = new Script(string.Empty);
			Assert.AreEqual(string.Empty, script.ScriptText);
		}

		[Test]
		public void CanAddRangeToScriptCollection()
		{
			ScriptCollection scripts = Script.ParseScripts("Select * from MyTable");
			ScriptCollection scriptsToAdd = Script.ParseScripts(string.Format("Select * from SomeTable{0}GO{1}SELECT TOP 1 FROM Pork", Environment.NewLine, Environment.NewLine));
			scripts.AddRange(scriptsToAdd);
			Assert.AreEqual(3, scripts.Count);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void AddRangeToScriptCollectionThrowsArgumentNullException()
		{
			ScriptCollection scripts = Script.ParseScripts("Select * from MyTable");
			scripts.AddRange(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SetScriptTextThrowsInvalidOperationException()
		{
			Script script = new Script(null);
			Console.WriteLine(script.ScriptText);
		}

		[Test]
		public void ToStringReturnsNoTokensFoundMessage()
		{
			Script script = new Script(null);
			//make sure en-us
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Assert.AreEqual("Script has no tokens.", script.ToString());
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ExecuteThrowsArgumentNullException()
		{
			Script script = new Script(null);
			script.Execute(null);
		}

		[Test]
		public void CanGetFullScriptText()
		{
			ScriptCollection scripts = Script.ParseScripts("Select * from MyTable");
			Assert.AreEqual("Select * from MyTable", scripts.FullScriptText);
		}

		/// <summary>
		/// Unpacks the installation script and makes sure it returns a script.
		/// </summary>
		[Test]
		public void UnpackScriptReturnsScript()
		{
			Stream stream = ScriptHelper.UnpackEmbeddedScript("Installation.01.00.00.sql");
			Assert.IsNotNull(stream);
		}
	}
}
