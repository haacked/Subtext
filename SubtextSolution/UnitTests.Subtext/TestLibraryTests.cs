using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using DeftTech.DuckTyping;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.TestLibrary;
using Subtext.TestLibrary.Servers;

namespace UnitTests.Subtext
{
	public interface IHttpContext
	{
		IHttpRequest Request { get;}
	}

	public interface IHttpRequest
	{
		Uri Url { get;}
	}

	[TestFixture]
	public class TestLibraryTests
	{
		[Test]
		public void DuckTypeTest()
		{
			using (HttpSimulator simulator = new HttpSimulator())
			{
				simulator.SimulateRequest(new Uri("http://haacked.com/url/"));
				IHttpContext context = DuckTyping.Cast<IHttpContext>(HttpContext.Current);
				MyMethod(context);
			}

			MockRepository mocks = new MockRepository();
			IHttpContext mockContext;
			using (mocks.Record())
			{
				mockContext = mocks.DynamicMock<IHttpContext>();
				IHttpRequest request = mocks.DynamicMock<IHttpRequest>();
				SetupResult.For(mockContext.Request).Return(request);
				SetupResult.For(request.Url).Return(new Uri("http://mock.haacked.com/url/"));
			}
			using (mocks.Playback())
			{
				MyMethod(mockContext);
			}
		}

		public void MyMethod(IHttpContext context)
		{
			Console.WriteLine(context.Request.Url);
		}

		[Test]
		public void CanParseRawSmtp()
		{
			string rawSmtp = "Subject: appears to be agitated" + Environment.NewLine
				+ "From: Scientist In <white-coat@example.com>" + Environment.NewLine
				+ "To: Other Scientist <in-the-field@example.com>" + Environment.NewLine
				+ "Content-Type: text/plain" + Environment.NewLine + Environment.NewLine
				+ "I think we should give him a banana." + Environment.NewLine + Environment.NewLine;


			ReceivedEmailMessage message = new ReceivedEmailMessage(rawSmtp);
			Assert.AreEqual("appears to be agitated", message.Subject, "Subject incorrectly parsed");
			Assert.AreEqual("white-coat@example.com", message.FromAddress.Email, "From Address email incorrectly parsed.");
			Assert.AreEqual("Scientist In", message.FromAddress.Name, "From Address name incorrectly parsed.");
			Assert.AreEqual("in-the-field@example.com", message.ToAddress.Email, "To Address email incorrectly parsed.");
			Assert.AreEqual("Other Scientist", message.ToAddress.Name, "To Address name incorrectly parsed.");
			Assert.AreEqual("text/plain", message.ContentType, "To Content-Type incorrectly parsed.");
			Assert.AreEqual("I think we should give him a banana.", message.Body, "The message body was not parsed correctly.");
			
			//Test setting message body.
			message.Body = "test this!";
			Assert.AreEqual("test this!", message.Body);
		}

		/// <summary>
		/// Sends a single email and confirms its existence in the inbox.
		/// </summary>
		[Test]
		public void SendSingleEmailAndConfirmInbox()
		{
			// Outgoing Message

			MailMessage message = new MailMessage();
			message.BodyEncoding = Encoding.ASCII;
			message.To.Add("test@example.com");
			message.From = new MailAddress("phil@example.com", "phil");
			message.Subject = "This is a test email";
			message.Body = "Just some text. ";


			SmtpClient smtpServer = new SmtpClient("127.0.0.1", 8081);

			// Receiving Mail Server
			using (TestSmtpServer receivingServer = new TestSmtpServer())
			{
				receivingServer.Stop(); //hasn't started. This should do nothing.
				try
				{
					receivingServer.Start("127.0.0.1", 8081);
					receivingServer.Start("127.0.0.1", 8081); //should not cause problem...
					smtpServer.Send(message);
				}
				finally
				{
					receivingServer.Stop();
				}
				Assert.AreEqual(1, receivingServer.Inbox.Count,
				                "Unlike the USPS, I fully expect to see this single email delivered.");

				// So Did It Work
				ReceivedEmailMessage received = receivingServer.Inbox[0];
				Console.Write("--->" + received.RawSmtpMessage + "<---");
				Assert.AreEqual("test@example.com", received.ToAddress.Email,
				                "I don't know who you were sending the email to, and neither did the mail server.");
				Assert.AreEqual("phil", received.FromAddress.Name,
				                "The mail server didn't figure out who sent it. I'll let you in on a secret. It was 'phil'.");
				Assert.AreEqual("phil@example.com", received.FromAddress.Email,
				                "Mail Server did not parse the from email address correctly.");
				Assert.AreEqual(message.Subject, received.Subject, "Apparently, the subject was not that important.");
				Assert.AreEqual(message.Body, received.Body, "The email had a nice body, which was not transferred correctly.");
			}
		}

		[Test, Ignore("Need to figure out why this doesn't work consistently on the build server.")]
		public void CanStartStopWebServer()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();

			using(TestWebServer server = new TestWebServer())
			{
				server.Start();
			
				server.ExtractResource("UnitTests.Subtext.Resources.Web.HttpClientTest.aspx", "HttpClientTest.aspx");
				string response; 
				try
				{
					response = server.RequestPage("HttpClientTest.aspx", "my-key=my-value", 1000);
				}
				catch(WebException)
				{
					//try again
					Thread.Sleep(1000);
					response = server.RequestPage("HttpClientTest.aspx", "my-key=my-value", 10000); // up to 10 secs.
				}
				Assert.AreEqual("my-key=my-value&Done", response, "Did not get our expected response with form vars.");
				
				response = server.RequestPage("HttpClientTest.aspx");
				Assert.AreEqual("Done", response, "Did not get our expected response.");
				server.Stop();
			}
		}

		[Test]
		public void TestEmailAddressToStringReturnsCorrectFormat()
		{
			TestEmailAddress address = new TestEmailAddress("test@example.com", "Joe Schmoe");
			Assert.AreEqual("Joe Schmoe <test@example.com>", address.ToString(), "The ToString method produces the wrong output.");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ExtractResourceBeforeServerStartThrowsInvalidOperationException()
		{
			using(TestWebServer server = new TestWebServer())
			{
				server.ExtractResource("UnitTests.Subtext.Resources.Web.HttpClientTest.aspx", "HttpClientTest.aspx");
			}
		}
	}
}
