using MbUnit.Framework;
using Subtext.Framework.Emoticons;

namespace UnitTests.Subtext.Framework.Emoticons
{
    [TestFixture]
    public class EmoticonTests
    {
        [Test]
        public void Transform_WithSmiley_TransformsSmiley()
        {
            //arrange
            var emoticon = new Emoticon("[:'(]", "<img src=\"{0}\" />");

            //act
            string result = emoticon.Replace("[:'(]", "http://example.com/");

            //assert
            Assert.AreEqual(@"<img src=""http://example.com/"" />", result);
        }

        [Test]
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