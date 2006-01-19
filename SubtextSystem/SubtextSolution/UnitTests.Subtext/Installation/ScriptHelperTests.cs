using System;
using System.IO;
using MbUnit.Framework;
using Subtext.Installation;
using Subtext.Scripting;

namespace UnitTests.Subtext.Installation
{
	/// <summary>
	/// Summary description for ScriptHelperTests.
	/// </summary>
	[TestFixture]
	public class ScriptHelperTests
	{
		/// <summary>
		/// Makes sure that ParseScript parses correctly.
		/// </summary>
		[Test]
		[RollBack]
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
