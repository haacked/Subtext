using System;
using System.Configuration;
using System.Data;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class HostInfoTests
	{
		[Test, Ignore("Ignore until we properly delete the host.")]
		[RollBack]
		public void CanLoadHost()
		{
			SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString, CommandType.Text, "DELETE subtext_Host");
			
			Assert.IsNull(HostInfo.Instance, "HostInfo should be Null");
			
			HostInfo.CreateHost("test", "test");
			
			Assert.IsNotNull(HostInfo.Instance, "Host should not be null.");
		}
	}
}
