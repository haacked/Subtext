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
using System.IO;
using MbUnit.Framework;
using Subtext.Installation;
using Subtext.Scripting;
using Subtext.Scripting.Exceptions;

namespace UnitTests.Subtext.Scripting
{
	/// <summary>
	/// Summary description for ScriptHelperTests.
	/// </summary>
	[TestFixture]
	public class ScriptHelperTests
	{
		[Test]
		public void CanParseGoWithDashDashCommentAfter()
		{
			string script =
@"SELECT * FROM foo;
 GO --  Hello Phil
CREATE PROCEDURE dbo.Test AS SELECT * FROM foo";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(2, scripts.Count);
		}

        [Test]
        public void CanParseNestedComments()
        {
            string script =
@"/*
select 1
/* nested comment */
go
delete from users
-- */";
            ScriptCollection scripts = Script.ParseScripts(script);
            Assert.AreEqual(1, scripts.Count, "This contains a comment and no scripts.");
        }

		[Test]
		[ExpectedException(typeof(SqlParseException))]
		public void SlashStarCommentAfterGoThrowsException()
		{
			string script = @"PRINT 'blah'
GO /* blah */";
			Script.ParseScripts(script);
		}

		[Test]
		public void CanParseSuccessiveGO()
		{
			string script = @"GO
GO";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(0, scripts.Count, "Expected no scripts since they would be empty.");
		}

		[Test]
		public void SemiColonDoesNotSplitScript()
		{
			string script = "CREATE PROC Blah AS SELECT FOO; SELECT Bar;";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(1, scripts.Count, "Expected no scripts since they would be empty.");
		}

		[Test]
		public void CanParseQuotedCorrectly()
		{
			string script = @"INSERT INTO #Indexes
	EXEC sp_helpindex 'dbo.subtext_URLs'";

			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(script, scripts[0].ScriptText, "Script text should not be modified");
		}

		[Test]
		public void CanParseSimpleScript()
		{
			string script = "Test" + Environment.NewLine + "go";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(1, scripts.Count);
			Assert.AreEqual("Test", scripts[0].ScriptText);
		}

		[Test]
		public void CanParseCommentBeforeGO()
		{
			string script = @"SELECT FOO
/*TEST*/ GO
BAR";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(2, scripts.Count);
		}

		[Test]
		public void CanParseCommentWithQuoteChar()
		{
			string script = 
@"/* Add the Url column to the subtext_Log table if it doesn't exist */
	ADD [Url] VARCHAR(255) NULL
GO
		AND		COLUMN_NAME = 'BlogGroup') IS NULL";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(2, scripts.Count);
		}

		[Test]
		public void CanParseDashDashCommentWithQuoteChar()
		{
			string script =
@"-- Add the Url column to the subtext_Log table if it doesn't exist
SELECT * FROM BLAH
GO
PRINT 'FOO'";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(2, scripts.Count);
		}

		[Test]
		public void CanParseLineEndingInDashDashComment()
		{
			string script =
@"SELECT * FROM BLAH -- Comment
GO
FOOBAR
GO";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(2, scripts.Count);
		}

		[Test]
		public void CanParseSimpleScriptEndingInNewLine()
		{
			string script = "Test" + Environment.NewLine + "GO" + Environment.NewLine;
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(1, scripts.Count);
			Assert.AreEqual("Test", scripts[0].ScriptText);
		}

		[Test]
		public void MultiLineQuoteShouldNotIgnoreDoubleQuote()
		{
			string script = "PRINT '" + Environment.NewLine
							+ "''" + Environment.NewLine
							+ "GO" + Environment.NewLine
							+ "/*" + Environment.NewLine
							+ "GO"
							+ "'";

			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(1, scripts.Count);
			UnitTestHelper.AssertStringsEqualCharacterByCharacter(script, scripts[0].ScriptText);
		}

		[Test]
		public void MultiLineQuoteShouldNotBeSplitByGoKeyword()
		{
			string script = "PRINT '" + Environment.NewLine
			                + "GO" + Environment.NewLine
			                + "SELECT * FROM BLAH" + Environment.NewLine
			                + "GO" + Environment.NewLine
			                + "'";

			ScriptCollection scripts = Script.ParseScripts(script);

			UnitTestHelper.AssertStringsEqualCharacterByCharacter(script, scripts[0].ScriptText);
			Assert.AreEqual(1, scripts.Count, "expected only one script");
		}
	
		/// <summary>
		/// Makes sure that ParseScript parses correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void ParseScriptParsesCorrectly()
		{
			string script =  
@"SET QUOTED_IDENTIFIER OFF 
-- Comment
Go
		
SET ANSI_NULLS ON 


GO

GO

SET ANSI_NULLS ON 


CREATE TABLE [<username,varchar,dbo>].[blog_Gost] (
	[HostUserName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Salt] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL
) ON [PRIMARY]
gO

";
			ScriptCollection scripts = Script.ParseScripts(script);
			Assert.AreEqual(3, scripts.Count, "This should parse to three scripts.");
			for(int i = 0; i < scripts.Count; i++)
			{
				Script sqlScript = scripts[i];
				Console.WriteLine("------------------------------");
				Console.WriteLine(sqlScript.ScriptText);
				Assert.IsFalse(sqlScript.ScriptText.StartsWith("GO"), "Script '" + i + "' failed had a GO statement");
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

		/// <summary>
		/// Unpacks the installation script and makes sure it returns a script.
		/// </summary>
		[Test]
		public void UnpackScriptReturnsScript()
		{
			Stream stream = ScriptHelper.UnpackEmbeddedScript("Installation.01.00.00.sql");
			Assert.IsNotNull(stream);
		}

        /// <summary>
        /// Unpacks the installation script and makes sure it returns a script.
        /// </summary>
        [Test]
        public void UnpackScriptAsStringReturnsScript()
        {
            string script = ScriptHelper.UnpackEmbeddedScriptAsString("Installation.01.00.00.sql");
            StringAssert.IsNonEmpty(script);
        }
	}
}
