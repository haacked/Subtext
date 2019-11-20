using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestClass]
    public class ScriptTests
    {
        [TestMethod]
        public void ToStringWithScriptWithNoTokensDisplaysNoTokensMessage()
        {
            var script = new Script("/*nothing*/");
            Assert.AreEqual("Script has no tokens.", script.ToString());
        }
    }
}