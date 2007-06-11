using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Plugins.Core.TextLinkAds;

namespace UnitTests.Subtext.Plugins.Core
{
    [TestFixture]
    public class FeedvertisingTests
    {
		[Test]
		public void ParseGoodAdXml()
		{
			IList<Advertisement> ads;
			IFeedvertisingService service = new FeedvertisingService();

			string adXml = "<Links><Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText>RssText</RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix>RssPrefix</RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link></Links>";
			ads = service.ParseAdvertisementXml(adXml);
			Assert.AreEqual(1, ads.Count);

		    Advertisement ad = ads[0];
			Assert.AreEqual("URL", ad.Url);
			Assert.AreEqual("Text", ad.Text);
			Assert.AreEqual("BeforeText", ad.BeforeText);
			Assert.AreEqual("AfterText", ad.AfterText);
			Assert.AreEqual("RssText", ad.RssText);
			Assert.AreEqual("RssBeforeText", ad.RssBeforeText);
			Assert.AreEqual("RssAfterText", ad.RssAfterText);
			Assert.AreEqual("RssPrefix", ad.RssPrefix);
		}

		[Test]
		public void AdsWithNoRssPrefixAreNotReturned()
		{
			IList<Advertisement> ads;
			IFeedvertisingService service = new FeedvertisingService();

			string adXml = "<Links><Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText>RssText</RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix></RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link></Links>";
			ads = service.ParseAdvertisementXml(adXml);
			Assert.AreEqual(0, ads.Count);
		}

		[Test]
		public void AdsWithNoRssTextAreNotReturned()
		{
			IList<Advertisement> ads;
			IFeedvertisingService service = new FeedvertisingService();

			string adXml = "<Links><Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText></RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix>RssPrefix</RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link></Links>";
		    ads = service.ParseAdvertisementXml(adXml);
			Assert.AreEqual(0, ads.Count);
		}

		[Test]
		public void ParseMultipleAdXml()
		{
			IList<Advertisement> ads;
			IFeedvertisingService service = new FeedvertisingService();

			string adXml = "<Links><Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText>RssText</RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix>RssPrefix</RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link><Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText>RssText</RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix>RssPrefix</RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link></Links>";
			ads = service.ParseAdvertisementXml(adXml);
			Assert.AreEqual(2, ads.Count);
		}

		[Test]
		public void BadAdXmlReturnsEmptyList()
		{
			IList<Advertisement> ads;
			IFeedvertisingService service = new FeedvertisingService();

			string adXml = "<Links<Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText>RssText</RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix>RssPrefix</RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link><Link><URL>URL</URL><Text>Text</Text><BeforeText>BeforeText</BeforeText><AfterText>AfterText</AfterText><RssText>RssText</RssText><RssBeforeText>RssBeforeText</RssBeforeText><RssAfterText>RssAfterText</RssAfterText><RssPrefix>RssPrefix</RssPrefix><RssMaxAds>RssMaxAds</RssMaxAds></Link></Links>";
			ads = service.ParseAdvertisementXml(adXml);
			Assert.AreEqual(0, ads.Count);
		}

		public Advertisement GetValidAdvertisement()
		{
			Advertisement ad = new Advertisement();
			ad.Url = "Url";
			ad.RssPrefix = "Prefix";
			ad.RssText = "Text";
			ad.RssBeforeText = "Before";
			ad.RssAfterText = "After";

			return ad;
		}

		[Test]
		public void AdHtmlRendersProperly()
		{
			Advertisement ad = GetValidAdvertisement();
			IFeedvertisingService service = new FeedvertisingService();
			Assert.AreEqual("<p><strong><em>Prefix</em></strong>: Before <a href=\"Url\">Text</a><em> </em>After<br /></p>", service.GetAdvertisementHtml(ad));
		}

		[Test]
		public void AdHtmlEmptyIfNoRssPrefix()
		{
			Advertisement ad = GetValidAdvertisement();
			IFeedvertisingService service = new FeedvertisingService();
			ad.RssPrefix = string.Empty;
			Assert.AreEqual(string.Empty, service.GetAdvertisementHtml(ad));
		}

		[Test]
		public void AdHtmlEmptyIfNoRssText()
		{
			Advertisement ad = GetValidAdvertisement();
			IFeedvertisingService service = new FeedvertisingService();
			ad.RssText = string.Empty;
			Assert.AreEqual(string.Empty, service.GetAdvertisementHtml(ad));
		}

		[Test]
		public void ProblemsWithAdXmlDownloadReturnsEmptyString()
		{
			IFeedvertisingService service = new FeedvertisingService();
			string xml = service.DownloadAdvertisementXml("www.donjfjlkfejlfejflejfle.com");
			Assert.AreEqual(string.Empty, xml);
		}
   }
}
