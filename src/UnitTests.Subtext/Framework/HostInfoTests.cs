#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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

            HostInfo.CreateHost("test", "test", "email@example.com");

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

        [Test]
        [RollBack2]
        public void CanCorrectlyStored()
        {
            //init
            EnsureHost();
            HostInfo host = HostInfo.Instance;
            Assert.IsNotNull(host, "Host should not be null.");

            host.HostUserName = "test3";
            host.Password = "password3";
            host.Salt = "salt3";

            HostInfo.UpdateHost(host);

            //act
            host = HostInfo.LoadHost(false);

            //post
            Assert.AreEqual("test3", host.HostUserName, "User name has not been correctly stored.");
            Assert.AreEqual("password3", host.Password, "Password has not been correctly stored.");
            Assert.AreEqual("salt3", host.Salt, "Salt has not been correctly stored.");
        }

        static void EnsureHost()
        {
            try
            {
                HostInfo host = HostInfo.LoadHost(true);
                if(host == null)
                {
                    HostInfo.CreateHost("test", "test", "test@example.com");
                }
            }
            catch(InvalidOperationException)
            {
                //Ignore.
            }
        }
    }
}