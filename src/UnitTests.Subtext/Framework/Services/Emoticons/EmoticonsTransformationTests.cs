using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Emoticons;

namespace UnitTests.Subtext.Framework.Emoticons
{
    [TestClass]
    public class EmoticonsTransformationTests
    {
        [TestMethod]
        public void Transform_WithSmiley_TransformsSmiley()
        {
            //arrange
            var emoticons = new List<Emoticon>();
            emoticons.Add(new Emoticon("[:'(]", "<img src=\"{0}\" />"));
            var emoticonsSource = new Mock<IEmoticonsSource>();
            emoticonsSource.Setup(es => es.GetEmoticons()).Returns(emoticons);
            var transformation = new EmoticonsTransformation(emoticonsSource.Object, "http://example.com/");

            //act
            string result = transformation.Transform("[:'(]");

            //assert
            Assert.AreEqual(@"<img src=""http://example.com/"" />", result);
        }
    }
}