using System;
using MbUnit.Framework;
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
		public void CanPostRequest()
		{
			using (TestWebServer webServer = new TestWebServer())
			{
				Uri url = webServer.Start();
				webServer.ExtractResource("UnitTests.Subtext.Resources.Web.HttpClientTest.aspx", "HttpClientTest.aspx");

				HttpClient client = new HttpClient();
				Uri httpClientPage = new Uri(url, "HttpClientTest.aspx");
				string response = client.PostRequest(httpClientPage, "user-agent", 10000, "test=true");
				Console.WriteLine(response);
			}
		}
	}

	
}
