using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Emoticons;

namespace UnitTests.Subtext.Framework.Emoticons
{
    [TestClass]
    public class EmoticonTests
    {
        [TestMethod]
        public void Transform_WithSmiley_TransformsSmiley()
        {
            //arrange
            var emoticon = new Emoticon("[:'(]", "<img src=\"{0}\" />");

            //act
            string result = emoticon.Replace("[:'(]", "http://example.com/");

            //assert
            Assert.AreEqual(@"<img src=""http://example.com/"" />", result);
        }

        [TestMethod]
        public void Transform_WithSmileyInText_TransformsSmiley()
        {
            //arrange
            var emoticon = new Emoticon("[:'(]", "<img src=\"{0}\" />");

            //act
            string result = emoticon.Replace("abc[:'(]def", "http://example.com/");

            //assert
            Assert.AreEqual(@"abc<img src=""http://example.com/"" />def", result);
        }
    }
}