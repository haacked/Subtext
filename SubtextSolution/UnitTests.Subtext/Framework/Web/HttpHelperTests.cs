using System;
using System.Net;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Util;
using Subtext.Framework.Web;

namespace UnitTests.Subtext.Framework.Web
{
	/// <summary>
	/// Contains tests of our handling of Http.
	/// </summary>
	[TestFixture]
	public class HttpHelperTests
	{
		/// <summary>
		/// Tests that we can create a proxy. This is based on some 
		/// settings in Web.config, which we populated in App.config 
		/// for this unit test.
		/// </summary>
		[Test]
		public void CanCreateProxy()
		{
			WebRequest request = HttpWebRequest.Create("http://subtextproject.com/");
			HttpHelper.SetProxy(request);
			Assert.IsNotNull(request.Proxy, "Proxy should not be null.");
		}
		
		/// <summary>
		/// Tests that we correctly parse if-modified-since from the request.
		/// Unfortunately, this unit test is time-zone sensitive.
		/// </summary>
		[RowTest]
		[Row("4/12/2006", "04/11/2006 5:00 PM")]
		[Row("12 Apr 2006 06:59:33 GMT", "4/11/2006 11:59:33 PM")]
		[Row("Wed, 12 Apr 2006 06:59:33 GMT", "04-11-2006 23:59:33")]
		public void TestIfModifiedSinceExtraction(string received, string expected)
		{
			SimulatedHttpRequest workerRequest = UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "");
            workerRequest.Headers.Add("If-Modified-Since", received);

			DateTime expectedDate = DateTimeHelper.ParseUnknownFormatUTC(expected);
			Console.WriteLine("{0}\t{1}\t{2}", received, expected, expectedDate.ToUniversalTime());

			DateTime result = HttpHelper.GetIfModifiedSinceDateUTC();
			//Convert to PST:
			const int PacificTimeZoneId = -2037797565;
			WindowsTimeZone timeZone = WindowsTimeZone.GetById(PacificTimeZoneId);
			result = timeZone.ToLocalTime(result);

			Assert.AreEqual(expectedDate, result);
		}
		
		[RowTest]
		[Row("test.css", true)]
		[Row("test.js", true)]
		[Row("test.png", true)]
		[Row("test.gif", true)]
		[Row("test.jpg", true)]
		[Row("test.html", true)]
		[Row("test.xml", true)]
		[Row("test.htm", true)]
		[Row("test.txt", true)]
		[Row("test.aspx", false)]
		[Row("test.asmx", false)]
		[Row("test.ashx", false)]
		public void CanDeterimineIsStaticFileRequest(string filename, bool expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "", filename);
			Assert.AreEqual(expected, HttpHelper.IsStaticFileRequest());
		}
	}
}
