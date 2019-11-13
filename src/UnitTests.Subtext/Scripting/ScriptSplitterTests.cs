using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestFixture]
    public class ScriptSplitterTests
    {
        [Test]
        public void ScriptSplitterCanEnumerate()
        {
            var splitter = new ScriptSplitter("This is a test");
            IEnumerable<string> enumerable = splitter;
            int i = 0;
            foreach(string s in enumerable)
            {
                i++;
            }
            Assert.AreEqual(1, i);
        }
    }
}