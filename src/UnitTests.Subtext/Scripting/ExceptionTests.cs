using System.Data.SqlClient;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Scripting;
using Subtext.Scripting.Exceptions;

namespace UnitTests.Subtext.Scripting
{
    /// <summary>
    /// Some tests of various exception conditions.
    /// </summary>
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void ExecuteThrowsArgumentExceptionForNullTransaction()
        {
            var script = new Script("");
            
            UnitTestHelper.AssertThrowsArgumentNullException(() => script.Execute(null));
        }

        [Test]
        public void ExecuteThrowsScriptExceptionForBadSql()
        {
            var script = new Script("SELECT * FROM BLAHBLAH");
            using(var connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();
                UnitTestHelper.AssertThrows<SqlScriptExecutionException>(() => script.Execute(connection.BeginTransaction()));
            }
        }

        [Test]
        public void ExecuteThrowsProperScriptExceptionForBadSql()
        {
            var script = new Script("SELECT * FROM BLAHBLAH");
            using(var connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                var e = UnitTestHelper.AssertThrows<SqlScriptExecutionException>(() => script.Execute(connection.BeginTransaction()));
                
                Assert.IsTrue(e.Message.Length > 0);
                Assert.AreEqual(0, e.ReturnValue);
                Assert.AreEqual("SELECT * FROM BLAHBLAH", e.Script.ScriptText);
            }
        }
    }
}