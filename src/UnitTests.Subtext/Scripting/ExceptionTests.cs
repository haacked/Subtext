using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Configuration;
using Subtext.Scripting;
using Subtext.Scripting.Exceptions;

namespace UnitTests.Subtext.Scripting
{
    /// <summary>
    /// Some tests of various exception conditions.
    /// </summary>
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void ExecuteThrowsArgumentExceptionForNullTransaction()
        {
            var script = new Script("");
            
            UnitTestHelper.AssertThrowsArgumentNullException(() => script.Execute(null));
        }

        [TestMethod]
        public void ExecuteThrowsScriptExceptionForBadSql()
        {
            var script = new Script("SELECT * FROM BLAHBLAH");
            using(var connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();
                UnitTestHelper.AssertThrows<SqlScriptExecutionException>(() => script.Execute(connection.BeginTransaction()));
            }
        }

        [TestMethod]
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