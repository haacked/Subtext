using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestFixture]
    public class ScriptCollectionTests
    {
        [Test]
        [ExpectedArgumentNullException]
        public void AddRangeWithNullArgumentThrowsArgumentNullException()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            scripts.AddRange(null);
        }

        [Test]
        public void FullScriptTextReturnsFullScript()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            Assert.AreEqual("/* Test */", scripts.FullScriptText);
        }

        [Test]
        public void AddRangeIncrementsScriptCountWhenAddingAScript()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            Assert.AreEqual(1, scripts.Count);
            scripts.AddRange(new Script[] {new Script("test"), new Script("test2")});
            Assert.AreEqual(3, scripts.Count);
        }
    }
}
