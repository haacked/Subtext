using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestFixture]
    public class ScriptCollectionTests
    {
        [Test]
        public void AddRangeWithNullArgumentThrowsArgumentNullException()
        {
            ScriptCollection scripts = Script.ParseScripts("/* Test */");
            UnitTestHelper.AssertThrowsArgumentNullException(() => scripts.AddRange(null));
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
            scripts.AddRange(new[] {new Script("test"), new Script("test2")});
            Assert.AreEqual(3, scripts.Count);
        }
    }
}