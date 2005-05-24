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
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for BaseControl.
	/// </summary>

	public class BaseControl : System.Web.UI.UserControl
	{
		
		public BaseControl()
		{
			
		}

		private BlogInfo _config = null;
		protected BlogInfo CurrentBlog
		{
			get
			{
				return _config;
			}
		}

		protected virtual string ControlCacheKey
		{
			get
			{
				return string.Format("{0}:{1}",this.GetType(),CurrentBlog.BlogID);
			}
		}

		protected override void OnInit(EventArgs e)
		{
			_config = Config.CurrentBlog;
			base.OnInit (e);
		}


//		protected override void OnLoad(EventArgs e)
//		{
//			_config = Config.CurrentBlog;
//			base.OnLoad(e);
//		}

		private string skinFilePath;
		public string SkinFilePath
		{
			get
			{return skinFilePath;}
			set{skinFilePath = value;}
		}
	}
}

