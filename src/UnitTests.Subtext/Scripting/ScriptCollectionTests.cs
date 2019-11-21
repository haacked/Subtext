using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestClass]
    public class ScriptCollectionTests
    {
        [TestMethod]
        public void AddRangeWithNullArgumentThrowsArgumentNullException()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            UnitTestHelper.AssertThrowsArgumentNullException(() => scripts.AddRange(null));
        }

        [TestMethod]
        public void FullScriptTextReturnsFullScript()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            Assert.AreEqual("/* Test */", scripts.FullScriptText);
        }

        [TestMethod]
        public void AddRangeIncrementsScriptCountWhenAddingAScript()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            Assert.AreEqual(1, scripts.Count);
            scripts.AddRange(new[] {new Script("test"), new Script("test2")});
            Assert.AreEqual(3, scripts.Count);
        }
    }
}