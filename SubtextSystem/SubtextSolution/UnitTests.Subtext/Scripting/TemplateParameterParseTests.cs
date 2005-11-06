using System;
using System.Text.RegularExpressions;
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
		[Row("<name ,  varchar(100)		, 'default' >", "name ", "varchar(100)		", "'default' ")]
		[Row("<name, int,10>", "name", "int", "10")]
		[Row("<name, int,>", "name", "int", "")]
		[Row("<name, int, 10>", "name", "int", "10")]
		public void TestParseSimpleScripts(string scriptText, string name, string dataType, string defaultValue)
		{
			Regex regex = new Regex(@"<\s*(?<name>[^>,]*),\s*(?<type>[^>,]*),\s*(?<default>[^>,]*)>", RegexOptions.Compiled);
			Assert.IsTrue(regex.IsMatch("<name,varchar(100),'default'>"));

			Script script = new Script(scriptText);
			TemplateParameterCollection parameters = script.GetTemplateParameters();
			Assert.AreEqual(1, parameters.Count, "Expected one parameter.");
			TemplateParameter parameter = parameters[0];
			Assert.AreEqual(name, parameter.Name, "Parameter name was not parsed correctly.");
			Assert.AreEqual(dataType, parameter.DataType, "Data Type was not parsed correctly.");
			Assert.AreEqual(defaultValue, parameter.Value, "DefaultValue was not parsed correctly.");
		}
	}
}
