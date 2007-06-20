using System;
using System.Globalization;
using System.Net;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Web;
using Subtext.Framework.Util.TimeZoneUtil;
using Subtext.TestLibrary;

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
		[Row("20070123T120102", "1/23/2007 4:01:02 AM")]
		[Row("12 Apr 2006 06:59:33 GMT", "4/11/2006 11:59:33 PM")]
		[Row("Wed, 12 Apr 2006 06:59:33 GMT", "04-11-2006 23:59:33")]
		public void TestIfModifiedSinceExtraction(string received, string expected)
		{
			using (new HttpSimulator().SetHeader("If-Modified-Since", received).SimulateRequest())
			{
				DateTime expectedDate = DateTimeHelper.ParseUnknownFormatUTC(expected);
				Console.WriteLine("{0}\t{1}\t{2}", received, expected, expectedDate.ToUniversalTime());

				DateTime result = HttpHelper.GetIfModifiedSinceDateUTC();
				//Convert to PST:
				const int PacificTimeZoneId = -2037797565;
				WindowsTimeZone timeZone = WindowsTimeZone.GetById(PacificTimeZoneId);
				result = timeZone.ToLocalTime(result);

				Assert.AreEqual(expectedDate, result);
			}
		}

		[Test]
		public void ParseUnknownFormatUTCReturnsNullDateForUnparesableDate()
		{
			Assert.AreEqual(NullValue.NullDateTime, DateTimeHelper.ParseUnknownFormatUTC("poop"));
		}

		[RowTest]
		[Row("4/12/2006", "04/12/2006 00:00:00 AM")]
		[Row("20070123T120102", "01/23/2007 12:01:02 PM")]
		[Row("12 Apr 2006 06:59:33 GMT", "04/12/2006 06:59:33 AM")]
		[Row("Wed, 12 Apr 2006 06:59:33 GMT", "04/12/2006 06:59:33 AM")]
		[MultipleCulture("en-US,en-NZ,it-IT")]
		public void CanParseUnknownFormatUTC(string received, string expected)
		{
			DateTime expectedDate = DateTimeHelper.ParseUnknownFormatUTC(received);
			Assert.AreEqual(DateTime.ParseExact(expected, "MM/dd/yyyy HH:mm:ss tt", new CultureInfo("en-US")), expectedDate);
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
					using (new HttpSimulator().SimulateRequest(new Uri("http://localhost/" + filename)))
					{
						Assert.AreEqual(expected, HttpHelper.IsStaticFileRequest());
					}
				}
			}
		}
