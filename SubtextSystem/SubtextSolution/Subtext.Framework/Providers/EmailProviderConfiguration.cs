using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for EmailProvider.
	/// </summary>
	[XmlRoot("EmailProvider")]
	public class EmailProviderConfiguration : BaseProvider
	{
		public EmailProviderConfiguration()
		{

		}

		private string _adminEmail;
		[XmlAttribute("adminEmail")]
		public string AdminEmail
		{
			get{return _adminEmail;}
			set{_adminEmail = value;}
		}

		private string _smtpServer = "localhost";
		[XmlAttribute("smtpServer")]
		public string SmtpServer
		{
			get{return _smtpServer;}
			set{_smtpServer = value;}
		}

		private string _password;
		[XmlAttribute("password")]
		public string Password
		{
			get{return _password;}
			set{_password = value;}
		}

		private string _userName;
		[XmlAttribute("userName")]
		public string UserName
		{
			get{return _userName;}
			set{_userName = value;}
		}
	}
}
