using System;
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
	}
}
