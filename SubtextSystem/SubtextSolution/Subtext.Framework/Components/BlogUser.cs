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

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for BlogUser.
	/// </summary>
	public class BlogUser : System.Web.Services.Protocols.SoapHeader
	{
		public BlogUser()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private string _userName;
		public string UserName
		{
			get{return _userName;}
			set{_userName = value;}
		}

		private string _password;
		public string Password
		{
			get{return _password;}
			set{_password = value;}
		}
	}
}

