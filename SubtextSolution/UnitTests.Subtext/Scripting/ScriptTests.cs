using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestFixture]
    public class ScriptTests
    {
        [Test]
        public void ToStringWithScriptWithNoTokensDisplaysNoTokensMessage()
        {
            Script script = new Script("/*nothing*/");
            Assert.AreEqual("Script has no tokens.", script.ToString());
        }
    }
}
