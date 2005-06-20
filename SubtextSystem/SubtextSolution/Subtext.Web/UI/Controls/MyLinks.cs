using System;
using Subtext.Framework;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

namespace Subtext.Web.UI.Controls
{
	using System;


	/// <summary>
	///		Summary description for Header.
	/// </summary>
	public class MyLinks : BaseControl
	{
		protected System.Web.UI.WebControls.HyperLink Admin;
		protected System.Web.UI.WebControls.HyperLink XMLLink;
		protected System.Web.UI.WebControls.HyperLink Syndication;
		protected System.Web.UI.WebControls.HyperLink HomeLink;
		protected System.Web.UI.WebControls.HyperLink ContactLink;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Context != null)
			{
				
				
				HomeLink.NavigateUrl = CurrentBlog.RootUrl;
				ContactLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}contact.aspx",CurrentBlog.RootUrl);

				if(Request.IsAuthenticated && Security.IsAdmin)
				{
					Admin.Text = "Admin";
					Admin.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}admin/default.aspx",CurrentBlog.RootUrl);
				}
				else
				{
					Admin.Text = "Login";
					Admin.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}login.aspx",CurrentBlog.RootUrl);
				}

				Syndication.NavigateUrl = XMLLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}Rss.aspx",CurrentBlog.RootUrl);
					

			}
		}

		
	}
}

