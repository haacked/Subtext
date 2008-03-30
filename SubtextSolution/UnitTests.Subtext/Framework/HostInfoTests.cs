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
using System.Data;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class HostInfoTests
	{
		[Test]
		[RollBack2]
		public void CanLoadHost()
		{
			SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, "DELETE subtext_Host");

		    HostInfo.LoadHost(false);

			Assert.IsNull(HostInfo.Instance, "HostInfo should be Null");
			
			HostInfo.CreateHost("test", "test");
			
			Assert.IsNotNull(HostInfo.Instance, "Host should not be null.");
		}

		[Test]
		[RollBack2]
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
