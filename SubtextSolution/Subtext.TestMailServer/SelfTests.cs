using System;
using DotNetOpenMail;
using MbUnit.Framework;

namespace Subtext.UnitTesting.Servers
{
	/// <summary>
	/// Summary description for SelfTests.
	/// </summary>
	[TestFixture]
	public class SelfTests
	{
		/// <summary>
		/// Sends a single email and confirms its existence in the inbox.
		/// </summary>
		[Test]
		public void SendSingleEmailAndConfirmInbox()
		{
			// Outgoing Message
			EmailMessage message = new EmailMessage();
			message.ToAddresses.Add(new EmailAddress("test@example.com"));
			message.FromAddress = new EmailAddress("phil@example.com", "phil");
			message.Subject = "This is a test email";
			message.BodyText = "Just some text. ";
			
			SmtpServer smtpServer = new SmtpServer("127.0.0.1", 8081);
			
			// Receiving Mail Server
			TestSmtpServer receivingServer = new TestSmtpServer();
			try
			{
				receivingServer.Start("127.0.0.1", 8081);
				message.Send(smtpServer);
			}
			finally
			{
				receivingServer.Stop();
			}
			Assert.AreEqual(1, receivingServer.Inbox.Count, "Unlike the USPS, I fully expect to see this single email delivered.");
			
			// So Did It Work
			ReceivedEmailMessage received = receivingServer.Inbox[0];
			Console.Write("--->" + received.RawSmtpMessage + "<---");
			Assert.AreEqual("test@example.com", received.ToAddress.Email, "I don't know who you were sending the email to, and neither did the mail server.");
			Assert.AreEqual("phil", received.FromAddress.Name, "The mail server didn't figure out who sent it. I'll let you in on a secret. It was 'phil'.");
			Assert.AreEqual("phil@example.com", received.FromAddress.Email, "Mail Server did not parse the from email address correctly.");
			Assert.AreEqual(message.Subject, received.Subject, "Apparently, the subject was not that important.");
			Assert.AreEqual(message.BodyText, received.Body, "The email had a nice body, which was not transferred correctly.");
		}
	}
}
