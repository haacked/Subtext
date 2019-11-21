using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Scripting;

namespace UnitTests.Subtext.Scripting
{
    [TestClass]
    public class ScriptSplitterTests
    {
        [TestMethod]
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