using System;
using MbUnit.Framework;
using Subtext.Web.SiteMap;

namespace UnitTests.Subtext.SubtextWeb
{
    [TestFixture]
    public class Sitemap
    {
        [Test]
        public void Add_WithUrlElement_IncreasesCountByOne()
        {
            var urlCollection = new UrlCollection();
            urlCollection.Add(new UrlElement(new Uri("http://someurl.com"), DateTime.Today, ChangeFrequency.Daily, 1));
            Assert.AreEqual(1, urlCollection.Count);
        }

        [Test]
        public void UrlElement_WithPropertiesSet_ReturnsSameValuesForProperties()
        {
            var url = new UrlElement(new Uri("http://someurl.com"), DateTime.MinValue, ChangeFrequency.Never, 0);
            StringAssert.AreEqualIgnoreCase("http://someurl.com/", url.Location);
            Assert.AreEqual(DateTime.MinValue, url.LastModified);
            Assert.AreEqual(ChangeFrequency.Never, url.ChangeFrequency);
            Assert.AreEqual(0, url.Priority);
        }

        [Test]
        public void UrlElement_CtorSetsPriority_ToLessThanZero()
        {
            // using property
            var url = new UrlElement();

            UnitTestHelper.AssertThrows<ArgumentOutOfRangeException>(() => url.Priority = -0.5M);
        }
    }
}