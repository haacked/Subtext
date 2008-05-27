using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Scripting.Exceptions;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestFixture]
    public class SqlScriptExecutionExceptionTests
    {
        [Test]
        public void InstantiateWithMessageSetsMessageProperty()
        {
            SqlScriptExecutionException exception = new SqlScriptExecutionException();
            exception = new SqlScriptExecutionException("Message");
            Assert.AreEqual("MessageReturn Value: 0", exception.Message);
        }

        [Test]
        public void CtorSetsProperties()
        {
            SqlScriptExecutionException exception = new SqlScriptExecutionException("Message", new Script("test"), 123);
            Assert.IsTrue(exception.Message.IndexOf("Message") > -1);
            Assert.AreEqual(123, exception.ReturnValue);
            Assert.AreEqual("test", exception.Script.OriginalScriptText);
        }
    }
}
