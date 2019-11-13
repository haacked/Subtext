#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using MbUnit.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class MimetypeDetectionTests
    {
        [Test]
        public void CanReadMimetypeMappings()
        {
            Assert.AreEqual(6, MimeTypesMapper.Mappings.Count);
        }

        //[Test]
        //public void CanGetListOfTypes()
        //{
        //    NameValueCollection list = MimeTypesMapper.Mappings.List;
        //    Assert.AreEqual("audio/mpeg", list[0]);
        //}

        [RowTest]
        [Row(".mp3", "audio/mpeg")]
        [Row(".zip", "application/octetstream")]
        [Row(".pdf", "application/octetstream")]
        [Row(".mp4", "video/mp4")]
        [Row(".avi", null)]
        public void MimetypeAreMappedCorrectly(string ext, string expectedType)
        {
            Assert.AreEqual(expectedType, MimeTypesMapper.Mappings.GetMimeType(ext));
        }

        [Test]
        public void GetMimeType_WithNullExtension_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => MimeTypesMapper.Mappings.GetMimeType(null));
        }

        [RowTest]
        [Row("http://mywonderfulldomain.com/podcast/episode1.mp3", "audio/mpeg")]
        [Row("http://code.google.com/codeclimbercommons/items/download/linklift-src.1.0.zip", "application/octetstream")
        ]
        [Row("http://polimi.it/ingdelsoftware/Corso di primo livello/lezione1.pdf", "application/octetstream")]
        [Row("http://wekarod.com/mvcscreencasts/screencast3.mp4", "video/mp4")]
        [Row("http://wekarod.com/mvcscreencasts/screencast3", null)]
        [Row("http://wekarod.com/mvcscreencasts/screencast3.qt", null)]
        public void CanDetectCorrectMimeType(string url, string expectedType)
        {
            Assert.AreEqual(expectedType, MimeTypesMapper.Mappings.ParseUrl(url));
        }

        [Test]
        public void ParseUrl_WithNullUrl_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => MimeTypesMapper.Mappings.ParseUrl(null));
        }

        [Test]
        public void ParseUrl_WithInvalidUrl_ThrowsArgumentException()
        {
            UnitTestHelper.AssertThrows<ArgumentException>(() => MimeTypesMapper.Mappings.ParseUrl("not/a valid\\url"));
        }
    }
}