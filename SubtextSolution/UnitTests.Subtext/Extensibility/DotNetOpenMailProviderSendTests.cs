using System;
using System.Collections.Specialized;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework.Providers;
using Subtext.TestLibrary.Servers;

namespace UnitTests.Subtext.Extensibility
{
	/// <summary>
	/// Tests the DotNetOpenMailProvider.
	/// </summary>
	[TestFixture]
	public class DotNetOpenMailProviderSendTests
	{
		[Test]
		public void CanInstantiateAndSendEmail()
		{
			DotNetOpenMailProvider provider = new DotNetOpenMailProvider();
			NameValueCollection configValue = new NameValueCollection();
			configValue["smtpServer"] = "127.0.0.1";
			configValue["port"] = "8189";
            configValue["username"] = "abracadabra";
            configValue["password"] = "haacked";
			provider.Initialize("providerTest", configValue);

			TestSmtpServer receivingServer = new TestSmtpServer();
			try
			{
				receivingServer.Start("127.0.0.1", 8189);
				provider.Send("phil@example.com", 
							"nobody@example.com", 
							"Subject to nothing", 
							"Mr. Watson. Come here. I need you.");
			}
			finally
			{
				receivingServer.Stop();
			}

			Assert.AreEqual(1, receivingServer.Inbox.Count, "Expected one email.");

			// So Did It Work
			ReceivedEmailMessage received = receivingServer.Inbox[0];
			Assert.AreEqual("phil@example.com", received.ToAddress.Email);
			Assert.AreEqual("nobody@example.com", received.FromAddress.Email, "Mail Server did not parse the from email address correctly.");
			Assert.AreEqual("Subject to nothing", received.Subject, "Apparently, the subject was not that important.");
			Assert.AreEqual("Mr. Watson. Come here. I need you.", received.Body, "The email had a nice body, which was not transferred correctly.");
		}

		[Test]
		public void CanInstantiateAndSendEmailWithDefaultPorts()
		{
			DotNetOpenMailProvider provider = new DotNetOpenMailProvider();
			NameValueCollection configValue = new NameValueCollection();
			configValue["smtpServer"] = "127.0.0.1";
			configValue["port"] = TestSmtpServer.DefaultPort.ToString(CultureInfo.InvariantCulture);
			provider.Initialize("providerTest", configValue);

			TestSmtpServer receivingServer = new TestSmtpServer();
			try
			{
				receivingServer.Start();
				provider.Send("phil@example.com",
							"nobody@example.com",
							"Subject to nothing",
							"Mr. Watson. Come here. I need you.");
			}
			finally
			{
				receivingServer.Stop();
			}

			Assert.AreEqual(1, receivingServer.Inbox.Count, "Expected one email.");

			// So Did It Work
			ReceivedEmailMessage received = receivingServer.Inbox[0];
			Assert.AreEqual("phil@example.com", received.ToAddress.Email);
			Assert.AreEqual("nobody@example.com", received.FromAddress.Email, "Mail Server did not parse the from email address correctly.");
			Assert.AreEqual("Subject to nothing", received.Subject, "Apparently, the subject was not that important.");
			Assert.AreEqual("Mr. Watson. Come here. I need you.", received.Body, "The email had a nice body, which was not transferred correctly.");
		}
	}
}
