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
using System.Web.UI;

namespace Subtext.Web.Admin.WebUI
{
	/// <summary>
	/// Renders a link tag for CSS.
	/// </summary>
	public class HeaderLink : Control
	{		
		private string _rel;
		private string _href;
		private string _linkType;
		private string _title;
		private string _media = "screen";

		protected override void Render(HtmlTextWriter output)
		{	
			output.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture, "<link href=\"{1}\" rel=\"{0}\" type=\"{2}\" title=\"{3}\" media=\"{4}\" />", 
				_rel, Href, _linkType, _title, _media));
		}

		public string Rel
		{
			get	{ return _rel; }
			set { _rel = value;}
		}

		public string Href
		{
			get { return Utilities.ResourcePath + _href; }
			set { _href = value; }
		}

		public string LinkType
		{
			get { return _linkType; }
			set { _linkType = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string Media
		{
			get { return _media; }
			set { _media = value; }
		}
	}
}

