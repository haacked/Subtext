using System;
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
    class ExceptionTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteThrowsArgumentExceptionForNullTransaction()
        {
            var script = new Script("");
            script.Execute(null);
        }

        [Test]
        [ExpectedException(typeof(SqlScriptExecutionException))]
        public void ExecuteThrowsScriptExceptionForBadSql()
        {
            var script = new Script("SELECT * FROM BLAHBLAH");
            using(var connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();
                script.Execute(connection.BeginTransaction());
            }
        }

        [Test]
        [ExpectedException(typeof(SqlScriptExecutionException))]
        public void ExecuteThrowsProperScriptExceptionForBadSql()
        {
            var script = new Script("SELECT * FROM BLAHBLAH");
            using(var connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();
                try
                {
                    script.Execute(connection.BeginTransaction());
                }
                catch(SqlScriptExecutionException e)
                {
                    Assert.IsTrue(e.Message.Length > 0);
                    Assert.AreEqual(0, e.ReturnValue);
                    Assert.AreEqual("SELECT * FROM BLAHBLAH", e.Script.ScriptText);
                    throw;
                }
            }
        }
    }
}