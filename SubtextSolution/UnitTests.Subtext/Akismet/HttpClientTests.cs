using System;
using System.Diagnostics;
using System.Net;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Akismet;
using Subtext.UnitTesting.Servers;

namespace UnitTests.Subtext.Akismet
{
	/// <summary>
	/// To really test this class, we probably need to run a webserver 
	/// for a short period of time.
	/// </summary>
	[TestFixture]
	public class HttpClientTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void PostRequestThrowsArgumentNullExceptionForNullFormParameters()
		{
			HttpClient client = new HttpClient();
			client.PostRequest(new Uri("http://haacked.com/"), "user-agent", 10, null);
		}

		[Test]
		[ExpectedArgumentException]
		public void PostRequestThrowsArgumentExceptionForBadUrl()
		{
			HttpClient client = new HttpClient();
			client.PostRequest(new Uri("ftp://haacked.com/"), "user-agent", 10, null);
		}

		[Test]
		public void CanPostRequestWithProxy()
		{
			MockRepository mocks = new MockRepository();
			IWebProxy webProxy = mocks.CreateMock<IWebProxy>();
			Expect.Call(webProxy.IsBypassed(new Uri("http://localhost"))).Return(true);
			mocks.ReplayAll();

			using (TestWebServer webServer = new TestWebServer())
			{
				Uri url = webServer.Start();
				webServer.ExtractResource("UnitTests.Subtext.Resources.Web.HttpClientTest.aspx", "HttpClientTest.aspx");
				HttpClient client = new HttpClient();
				Uri httpClientPage = new Uri(url, "HttpClientTest.aspx");
				Debug.WriteLine(string.Format("Making a request for {0} at {1}", httpClientPage, DateTime.Now));

				string response = client.PostRequest(httpClientPage, "user-agent", 20000, "test=true", webProxy);
				Assert.AreEqual("test=true&Done", response);
			}
		}
	}
}
