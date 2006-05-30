#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provides a class used to handle email.
	/// </summary>
	public abstract class EmailProvider : ProviderBase
	{
		/// <summary>
		/// Returns the configured concrete instance of a <see cref="EmailProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static EmailProvider Instance()
		{
			return (EmailProvider)ProviderBase.Instance("Email");
		}

		#region EmailProvider Methods
		/// <summary>
		/// Gets or sets the admin email.  This is the email address that 
		/// emails sent to a blog owner appears to be from.  It represents 
		/// the system and might not be a real address.
		/// </summary>
		/// <value></value>
		public abstract string AdminEmail
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SMTP server.
		/// </summary>
		/// <value></value>
		public abstract string SmtpServer
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the SMTP server port.
		/// </summary>
		/// <value></value>
		public abstract int Port
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the password for SMTP servers that 
		/// require authentication.
		/// </summary>
		/// <value></value>
		public abstract string Password
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the name of the user for SMPT servers that 
		/// require authentication.
		/// </summary>
		/// <value></value>
		public abstract string UserName
		{
			get;
			set;
		}

		public abstract bool Send(string to, string from, string subject, string message);
		#endregion
	}
}
