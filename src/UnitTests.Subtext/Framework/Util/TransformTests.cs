using System.Collections.Generic;
using System.IO;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Util;
using Subtext.Infrastructure;

namespace UnitTests.Subtext.Framework.Util
{
    [TestFixture]
    public class TransformTests
    {
        readonly string emoticonsPath = UnitTestHelper.GetPathInExecutingAssemblyLocation("emoticons.txt");

        [Test]
        public void CanLoadEmoticonsFile()
        {
            //arrange
            UnitTestHelper.UnpackEmbeddedResource("Web.emoticons.txt", emoticonsPath);
            var cache = new Mock<ICache>();
            cache.Setup(c => c[It.IsAny<string>()]).Returns(null);

            //act
            IList<string> transforms = Transform.LoadTransformFile(cache.Object, emoticonsPath);

            //assert
            Assert.AreEqual(48, transforms.Count, "Expected 48 transformations");
            Assert.AreEqual(@"\[\(H\)]", transforms[0], "The first line does not match");
            Assert.AreEqual(@"<img src=""{0}Images/emotions/smiley-cool.gif"" border=""0"" alt=""Cool"" />"
                            , transforms[1]);
        }

        [Test]
        public void Transform_WithSmiley_TransformsSmiley()
        {
            //arrange
            UnitTestHelper.UnpackEmbeddedResource("Web.emoticons.txt", emoticonsPath);
            var cache = new Mock<ICache>();
            cache.Setup(c => c[It.IsAny<string>()]).Returns(null);

            //act
            string result = Transform.EmoticonsTransforms(cache.Object, "http://example.com/", "[:'(]", emoticonsPath);

            //assert
            Assert.AreEqual(
                @"<img src=""http://example.com/Images/emotions/smiley-cry.gif"" border=""0"" alt=""Cry"" /> ", result);
        }

        [Test]
        public void Transform_WithSmileyWithinSentence_TransformsSmiley()
        {
            //arrange
            UnitTestHelper.UnpackEmbeddedResource("Web.emoticons.txt", emoticonsPath);
            var cache = new Mock<ICache>();
            cache.Setup(c => c[It.IsAny<string>()]).Returns(null);

            //act
            string result = Transform.EmoticonsTransforms(cache.Object, "http://example.com/",
                                                          "Wocka Wocka [:'(] The Whip Master", emoticonsPath);

            //assert
            Assert.AreEqual(
                @"Wocka Wocka <img src=""http://example.com/Images/emotions/smiley-cry.gif"" border=""0"" alt=""Cry"" />  The Whip Master",
                result);
        }

        [TearDown]
        public void TearDown()
        {
            if(File.Exists(emoticonsPath))
            {
                File.Delete(emoticonsPath);
            }
        }
    }
}