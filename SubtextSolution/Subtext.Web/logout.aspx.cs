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
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;
using Subtext.Framework.Routing;

namespace Subtext.Web.Pages
{
	/// <summary>
	/// Logs a user out of the system.
	/// </summary>
	public class logout : RoutablePage
	{
		protected override void OnLoad(EventArgs e)
		{
			SecurityHelper.LogOut();
			HttpContext.Current.Response.Redirect(Url.BlogUrl());
		}
	}
}

