using System;
using MbUnit.Framework;

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
		/// <param name="script">The script.</param>
		/// <param name="name">The name.</param>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="defaultValue">The default value.</param>
		[RowTest]
		[Row("<name,varchar(100),'default'>", "name", "varchar(100)", "default")]
		public void TestParseSimpleScripts(string script, string name, string dataType, string defaultValue)
		{
			throw new NotImplementedException("//TODO: Implement test.");
		}
	}
}
