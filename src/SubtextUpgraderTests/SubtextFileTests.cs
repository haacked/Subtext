using System.IO;
using MbUnit.Framework;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class SubtextFileTests
    {
        [Test]
        public void OpenWrite_OnFileWithContents_ClearsContentsFirst()
        {
            // arrange
            using(var writer = File.CreateText(@"test.txt"))
            {
                writer.Write("0123456789");
            }
            var file = new SubtextFile(new FileInfo("test.txt"));
            
            // act
            using(var writer = new StreamWriter(file.OpenWrite()))
            {
                writer.Write("abcdef");
            }

            // assert
            using(var reader = new StreamReader(File.OpenRead("test.txt")))
            {
                string text = reader.ReadToEnd();
                Assert.AreEqual("abcdef", text);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if(File.Exists("test.txt"))
            {
                File.Delete("test.txt");
            }
        }
    }
}
