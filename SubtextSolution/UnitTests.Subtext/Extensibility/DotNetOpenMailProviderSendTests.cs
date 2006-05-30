using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Framework.Email;
using Subtext.UnitTesting.Servers;

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
			configValue["adminEmail"] = "admin@example.com";
			configValue["smtpServer"] = "127.0.0.1";
			configValue["port"] = "8081";
			configValue["password"] = "abracadabra";;
			configValue["username"] = "haacked";
			provider.Initialize("providerTest", configValue);
			
			TestSmtpServer receivingServer = new TestSmtpServer();
			try
			{
				receivingServer.Start("127.0.0.1", 8081);
				provider.Send("phil@example.com", "nobody@example.com", "Subject to nothing", "Mr. Watson. Come here. I need you.");
			}
			finally
			{
				receivingServer.Stop();
			}
			
			Assert.AreEqual(1, receivingServer.Inbox.Count, "Unlike the USPS, I fully expect to see this single email delivered.");
			
			// So Did It Work
			ReceivedEmailMessage received = receivingServer.Inbox[0];
			Console.Write("--->" + received.RawSmtpMessage + "<---");
			Assert.AreEqual("phil@example.com", received.ToAddress.Email, "I don't know who you were sending the email to, and neither did the mail server.");
			Assert.AreEqual("nobody@example.com", received.FromAddress.Email, "Mail Server did not parse the from email address correctly.");
			Assert.AreEqual("Subject to nothing", received.Subject, "Apparently, the subject was not that important.");
			Assert.AreEqual("Mr. Watson. Come here. I need you.", received.Body, "The email had a nice body, which was not transferred correctly.");
		}		
	}
}
