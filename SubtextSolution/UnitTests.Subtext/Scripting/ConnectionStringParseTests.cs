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
using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
	/// <summary>
	/// Summary description for ConnectionStringParseTests.
	/// </summary>
	[TestFixture]
	public class ConnectionStringParseTests
	{
		[RowTest]
		[Row("Data Source=TEST;Initial Catalog=pubs;User Id=sa;Password=asdasd;", "TEST", "pubs", "sa", "asdasd")]
		[Row("Data Source=;Initial Catalog=;User Id=;Password=;", "", "", "", "")]
		[Row("Data Source = TEST;Initial Catalog = pubs;User Id = sa;Password = asdasd", "TEST", "pubs", "sa", "asdasd")]
		[Row("Data Source = TEST;User Id = sa;Password = asdasd;Initial Catalog = pubs", "TEST", "pubs", "sa", "asdasd")]
		[Row("Server=127.0.0.1;Database=pubs;User ID=sa;Password=asdasd;Trusted_Connection=False", "127.0.0.1", "pubs", "sa", "asdasd")]
		[Row("Server= 127.0.0.1 ; Database = SubtextData; User ID = sa ; Password = asdasd ; Trusted_Connection = False", "127.0.0.1", "SubtextData", "sa", "asdasd")]
		public void CanVariousStandardSecurityConnectionStrings(string connectionString, string dataSource, string database, string userId, string password)
		{
			ConnectionString connectionInfo = ConnectionString.Parse(connectionString);
			Assert.AreEqual(database, connectionInfo.Database, "Did not parse the database string correctly.");
			Assert.AreEqual(dataSource, connectionInfo.Server, "Did not parse the server string correctly.");
			Assert.AreEqual(userId, connectionInfo.UserId, "Did not parse the user id correctly.");
			Assert.AreEqual(password, connectionInfo.Password, "Did not parse the password correctly.");

			//Test the implicit operation
			ConnectionString connectionInfo2 = connectionString;
			Assert.AreEqual(database, connectionInfo2.Database, "Did not parse the database string correctly.");
			Assert.AreEqual(dataSource, connectionInfo2.Server, "Did not parse the server string correctly.");
			Assert.AreEqual(userId, connectionInfo2.UserId, "Did not parse the user id correctly.");
			Assert.AreEqual(password, connectionInfo2.Password, "Did not parse the password correctly.");
		}

		[Test]
		public void CanImplicitlyConvertConnectionStringToString()
		{
			ConnectionString connection = ConnectionString.Parse("Data Source=TEST;Initial Catalog=pubs;User Id=sa;Password=asdasd;");
			string s = connection;
			Assert.AreEqual("Data Source=TEST;Initial Catalog=pubs;User ID=sa;Password=asdasd;", s, "Strings are still equal.");
		}
	}
}
