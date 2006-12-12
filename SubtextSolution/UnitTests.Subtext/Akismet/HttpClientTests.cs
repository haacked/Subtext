using System;
using System.Net;
using MbUnit.Framework;
using Subtext.Akismet;

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
			//TODO: In order to finish this test, we need to start a lightweight local web server.
			HttpClient client = new HttpClient();
			try
			{
				client.PostRequest(new Uri("http://subtextproject.com/"), "user-agent", 10, "test=true");
			}
			catch(WebException)
			{
			}
		}
	}
}
