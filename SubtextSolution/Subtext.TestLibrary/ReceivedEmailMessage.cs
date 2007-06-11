using System;
using System.Text.RegularExpressions;

namespace Subtext.TestLibrary.Servers
{
	/// <summary>
	/// Summary description for SimulatedEmailMessage.
	/// </summary>
	public class ReceivedEmailMessage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReceivedEmailMessage"/> class.
		/// </summary>
		public ReceivedEmailMessage(string rawSmtpMessage)
		{
			this.rawSmtpMessage = rawSmtpMessage;
			ParseRawSmtpMessage();
		}
		
		private void ParseRawSmtpMessage()
		{
			ParseSmtpSubject();
			ParseFromAddress();
			ParseToAddress();
			ParseContentType();
			ParseBody();
		}
		
		private void ParseBody()
		{
			string bodyDelimiter = Environment.NewLine + Environment.NewLine;
			int indexOfFirstNewline = this.rawSmtpMessage.IndexOf(Environment.NewLine + Environment.NewLine);
			if(indexOfFirstNewline > 0)
			{
				this.body = this.rawSmtpMessage.Substring(indexOfFirstNewline + bodyDelimiter.Length);
				this.body = this.body.Substring(0, this.body.Length - bodyDelimiter.Length); //Trim off last CRLF.
			}
		}

		private void ParseSmtpSubject()
		{
			ParseSmtpHeader("Subject", out this.subject);
		}
		
		private void ParseContentType()
		{
			ParseSmtpHeader("Content-type", out this.contentType);
		}
		
		private void ParseToAddress()
		{
			ParseEmailAddress("To", out this.toAddress);
		}
		
		private void ParseFromAddress()
		{
			ParseEmailAddress("From", out this.fromAddress);
		}
		
		private void ParseEmailAddress(string addressFieldName, out TestEmailAddress address)
		{
			Regex regex = new Regex(string.Format(@"^{0}:\s*(?<name>.+?)?\s*<(?<email>.*?)>\s*$", addressFieldName), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
			Match match = regex.Match(this.rawSmtpMessage);
			if(match.Success)
			{
				address = new TestEmailAddress(match.Groups["email"].Value, match.Groups["name"].Value);
				return;
			}
			address = null;
		}
		
		private void ParseSmtpHeader(string fieldName, out string value)
		{
			Regex regex = new Regex(string.Format(@"^{0}:\s*(?<subject>.+?)$", fieldName), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
			Match match = regex.Match(this.rawSmtpMessage);
			if(match.Success)
			{
				value = match.Groups["subject"].Value.Trim();
				return;
			}
			value = string.Empty;
		}

		/// <summary>
		/// Gets from address.
		/// </summary>
		/// <value>From address.</value>
		public TestEmailAddress FromAddress
		{
			get { return this.fromAddress; }
		}

		TestEmailAddress fromAddress;
		
		/// <summary>
		/// Gets to address.
		/// </summary>
		/// <value>To address.</value>
		public TestEmailAddress ToAddress
		{
			get { return this.toAddress; }
		}

		TestEmailAddress toAddress;

		/// <summary>
		/// Gets or sets the subject of the email.
		/// </summary>
		/// <value>The subject.</value>
		public string Subject
		{
			get { return this.subject; }
		}

		string subject;

		/// <summary>
		/// Gets the raw SMTP message.
		/// </summary>
		/// <value>The raw SMTP message.</value>
		public string RawSmtpMessage
		{
			get { return this.rawSmtpMessage; }
		}

		string rawSmtpMessage;

		/// <summary>
		/// Gets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public string ContentType
		{
			get { return this.contentType; }
		}

		string contentType;

		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		/// <value>The body.</value>
		public string Body
		{
			get { return this.body; }
			set { this.body = value; }
		}

		string body;
	}
}
