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
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework
{
    [TestFixture]
    public class HostInfoTests
    {
        [SetUp]
        public void SetUp()
        {
            DependencyResolver.SetResolver(new TestDependencyResolver());
        }

        private class TestDependencyResolver : IDependencyResolver
        {
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(ObjectRepository))
                {
                    return new DatabaseObjectProvider();
                }
                return null;
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return null;
            }
        }

        [Test]
        [RollBack2]
        public void CanLoadHost()
        {
            var repository = new DatabaseObjectProvider();
            SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, "DELETE subtext_Host");

            HostInfo.LoadHostInfoFromDatabase(repository, suppressException: false);

            Assert.IsNull(HostInfo.Instance, "HostInfo should be Null");

            HostInfo.CreateHost(repository, "test", "test", "email@example.com");

            Assert.IsNotNull(HostInfo.Instance, "Host should not be null.");
        }

        [Test]
        [RollBack2]
        public void CanUpdateHost()
        {
            var repository = new DatabaseObjectProvider();
            EnsureHost();
            HostInfo host = HostInfo.Instance;
            Assert.IsNotNull(host, "Host should not be null.");

            host.HostUserName = "test2";
            host.Password = "password2";
            host.Salt = "salt2";

            HostInfo.UpdateHost(repository, host);

            host = HostInfo.LoadHostInfoFromDatabase(repository, false);
            Assert.AreEqual("test2", host.HostUserName, "Username wasn't changed.");
        }

        [Test]
        [RollBack2]
        public void CanCorrectlyStored()
        {
            //init
            var repository = new DatabaseObjectProvider();
            EnsureHost();
            HostInfo host = HostInfo.Instance;
            Assert.IsNotNull(host, "Host should not be null.");

            host.HostUserName = "test3";
            host.Password = "password3";
            host.Salt = "salt3";

            HostInfo.UpdateHost(repository, host);

            //act
            host = HostInfo.LoadHostInfoFromDatabase(repository, false);

            //post
            Assert.AreEqual("test3", host.HostUserName, "User name has not been correctly stored.");
            Assert.AreEqual("password3", host.Password, "Password has not been correctly stored.");
            Assert.AreEqual("salt3", host.Salt, "Salt has not been correctly stored.");
        }

        static void EnsureHost()
        {
            try
            {
                var repository = new DatabaseObjectProvider();
                HostInfo host = HostInfo.LoadHostInfoFromDatabase(repository, true);
                if (host == null)
                {
                    HostInfo.CreateHost(repository, "test", "test", "test@example.com");
                }
            }
            catch (InvalidOperationException)
            {
                //Ignore.
            }
        }
    }
}