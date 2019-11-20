using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Scripting;
using Subtext.Scripting.Exceptions;

namespace UnitTests.Subtext.Scripting
{
    [TestClass]
    public class SqlScriptExecutionExceptionTests
    {
        [TestMethod]
        public void InstantiateWithMessageSetsMessageProperty()
        {
            var exception = new SqlScriptExecutionException();
            exception = new SqlScriptExecutionException("Message");
            Assert.AreEqual("MessageReturn Value: 0", exception.Message);
        }

        [TestMethod]
        public void CtorSetsProperties()
        {
            var exception = new SqlScriptExecutionException("Message", new Script("test"), 123);
            Assert.IsTrue(exception.Message.IndexOf("Message") > -1);
            Assert.AreEqual(123, exception.ReturnValue);
            Assert.AreEqual("test", exception.Script.OriginalScriptText);
        }
    }
}