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
		[Test]
		[RollBack]
		public void CanLoadHost()
		{
			SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString, CommandType.Text, "DELETE subtext_Host");
			
			Assert.IsNull(HostInfo.Instance, "HostInfo should be Null");
			
			HostInfo.CreateHost("test", "test");
			
			Assert.IsNotNull(HostInfo.Instance, "Host should not be null.");
		}

		[Test]
		[RollBack]
		public void CanUpdateHost()
		{
			EnsureHost();
			HostInfo host = HostInfo.Instance;
			Assert.IsNotNull(host, "Host should not be null.");

			host.HostUserName = "test2";
			host.Password = "password2";
			host.Salt = "salt2";

			HostInfo.UpdateHost(host);

			host = HostInfo.LoadHost(false);
			Assert.AreEqual("test2", host.HostUserName, "Username wasn't changed.");			
		}
		
		void EnsureHost()
		{
			try
			{
				HostInfo host = HostInfo.LoadHost(true);
				if (host == null)
					HostInfo.CreateHost("test", "test");
			}
			catch(InvalidOperationException)
			{
				//Ignore.
			}
		}
	}
}
