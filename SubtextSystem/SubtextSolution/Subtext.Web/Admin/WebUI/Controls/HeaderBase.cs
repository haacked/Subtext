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

using System;
using System.Web;
using System.Web.UI;
using Subtext.Framework.Format;

namespace Subtext.Web.Admin.WebUI
{
	public class HeaderBase : Control
	{		
		private string _relativePath = String.Empty;
		private string _appPath;

		public HeaderBase()
		{
			_appPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + 
				HttpContext.Current.Request.ApplicationPath;
		}

		protected override void Render(HtmlTextWriter output)
		{	
			output.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture, "<base href=\"{0}{1}/\" />", AppPath, RelativePath));
		}

		public string RelativePath
		{
			get	
			{ 
				return _relativePath.StartsWith("/") ? _relativePath.Substring(1) : _relativePath;
			}
			set { _relativePath = value;}
		}

		public string AppPath
		{
			get 
			{ 
				return _appPath.EndsWith("/") ? _appPath : _appPath + "/";
			}
			set { _appPath = value; }
		}
	}
}
