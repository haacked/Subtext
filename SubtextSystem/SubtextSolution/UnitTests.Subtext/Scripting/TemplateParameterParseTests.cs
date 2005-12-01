using System;
using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
	/// <summary>
	/// Tests parsing template parameters in a script.
	/// </summary>
	[TestFixture]
	public class TemplateParameterParseTests
	{
		/// <summary>
		/// Tests parsing simple scripts with template parameters.
		/// </summary>
		/// <param name="scriptText">The script.</param>
		/// <param name="name">The name.</param>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="defaultValue">The default value.</param>
		[RowTest]
		[Row("<name,varchar(100),'default'>", "name", "varchar(100)", "'default'")]
		[Row("XYZ <name,varchar(100),'default'> ABC", "name", "varchar(100)", "'default'")]
		[Row("<name ,  varchar(100)		, 'default' >", "name ", "varchar(100)		", "'default' ")]
		[Row("<name, int,10>", "name", "int", "10")]
		[Row("<name, int,>", "name", "int", "")]
		[Row("<name, int, 10>", "name", "int", "10")]
		public void TestParseSimpleScripts(string scriptText, string name, string dataType, string defaultValue)
		{
			Script script = new Script(scriptText);
			TemplateParameterCollection parameters = script.TemplateParameters;
			Assert.AreEqual(1, parameters.Count, "Expected one parameter.");
			TemplateParameter parameter = parameters[0];
			Assert.AreEqual(name, parameter.Name, "Parameter name was not parsed correctly.");
			Assert.AreEqual(dataType, parameter.DataType, "Data Type was not parsed correctly.");
			Assert.AreEqual(defaultValue, parameter.Value, "DefaultValue was not parsed correctly.");
		}

		/// <summary>
		/// Tests parsing simple scripts with template parameters.
		/// </summary>
		/// <param name="scriptText">The script.</param>
		/// <param name="replaceValue"></param>
		/// <param name="expectedResult"></param>
		[RowTest]
		[Row("<name,varchar(100),'default'>", "'MyValue'", "'MyValue'")]
		[Row("<name ,  varchar(100)		, default >", "default", "default")]
		[Row("ABC <name ,  varchar(100)		, default > XYZ", "default", "ABC default XYZ")]
		[Row("<name, int,10>", "15", "15")]
		[Row("ABC<name, int,10>XYZ", "15", "ABC15XYZ")]
		[Row("<name, int,>", "", "")]
		public void TestReplaceSimpleScripts(string scriptText, string replaceValue, string expectedResult)
		{
			Script script = new Script(scriptText);
			script.TemplateParameters[0].Value = replaceValue;
			Assert.AreEqual(expectedResult, script.ScriptText, "Expected a replacement to occur.");
		}

		/// <summary>
		/// Tests parsing simple scripts with template parameters that have default values.
		/// </summary>
		/// <param name="scriptText">The script.</param>
		/// <param name="expectedResult"></param>
		[RowTest]
		[Row("<name,varchar(100),'default'>", "'default'")]
		[Row("<name ,  varchar(100)		, default >", "default ")]
		[Row("ABC <name ,  varchar(100)		, default > XYZ", "ABC default  XYZ")]
		[Row("<name, int,10>", "10")]
		[Row("ABC<name, int,10>XYZ", "ABC10XYZ")]
		[Row("<name, int,>", "")]
		[Row("AND DateAdded < DateAdd(day, 1, @StopDate) AND PostConfig & 1 <> CASE ", "AND DateAdded < DateAdd(day, 1, @StopDate) AND PostConfig & 1 <> CASE ")]
		public void TestReplaceSimpleScriptsWithDefaults(string scriptText, string expectedResult)
		{
			Script script = new Script(scriptText);
			Assert.AreEqual(expectedResult, script.ScriptText, "Expected a replacement to occur.");
		}

		/// <summary>
		/// Tests the more complex script.
		/// </summary>
		/// <remarks>
		/// The script itself is non-sensical, but that's not the point.
		/// </remarks>
		[Test]
		public void TestMoreComplexScript()
		{
			string scriptText = "<name,varchar,default>SELECT * FROM <name,varchar,default> WHERE " + Environment.NewLine 
				+ "<name,varchar,default> = <name2,int,10> and <name3,decimal,>";

			Script script = new Script(scriptText);
			script.TemplateParameters.SetValue("name", "subtext_Config");
			script.TemplateParameters.SetValue("name3", "'32'");

			string expected = "subtext_ConfigSELECT * FROM subtext_Config WHERE " + Environment.NewLine 
				+ "subtext_Config = 10 and '32'";

			Assert.AreEqual(expected, script.ScriptText, "The template replacements failed");
		}
	}
}
