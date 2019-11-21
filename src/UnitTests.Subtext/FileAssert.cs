using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTests.Subtext
{
    internal class FileAssert
    {
        public static void Exists(string fileName)
        {
            Assert.IsNotNull(fileName, "FileName is null");
            Assert.IsTrue(File.Exists(fileName), "file {0} does not exist", fileName);
        }
    }
}
