using log4net.Appender;
using MbUnit.Framework;

namespace UnitTests.Subtext
{
    /// <summary>
    /// Various tests to make sure certain assumptions are met in this build.
    /// </summary>
    [TestFixture]
    public class IntegrityTests
    {
        public void Log4NetHasConnectionStringNameProperty()
        {
            var appender = new AdoNetAppender();
            // we really only care that this property exists. So this test 
            // must simply compile to pass!
            Assert.IsTrue(string.IsNullOrEmpty(appender.ConnectionStringName));
        }
    }
}
