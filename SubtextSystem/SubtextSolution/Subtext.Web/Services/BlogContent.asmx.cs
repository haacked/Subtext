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
using System.ComponentModel;
using System.Web.Services;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace Subtext.Web.Services
{
	/// <summary>
	/// Summary description for BlogContent.
	/// </summary>
	[ WebService(Name=".Text Content",Description=".Text content provider service",Namespace="http://ScottWater.com/DotText/services/blogcontent/")]
	public class BlogContent : System.Web.Services.WebService
	{
		public BlogContent()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod(MessageName="GetEntries",Description="Requests the last X number of Blog Entries. The number is limited by the settings in the blog.config file. The return type, is an an Array of Entries",EnableSession=false)]
		public EntryCollection GetEntries(int ItemCount)
		{
			return Entries.GetRecentPosts(Globals.AllowedItemCount(ItemCount),PostType.BlogPost,true);
		}

		[WebMethod(MessageName="GetEntriesByCategoryID",Description="Requests the last X number of Blog Entries By a specific category. The number is limited by the settings in the blog.config file. The return type, is an an Array of Entries",EnableSession=false)]
		public EntryCollection GetEntries(int ItemCount, int CategoryID)
		{
			return Entries.GetEntriesByCategory(Globals.AllowedItemCount(ItemCount),CategoryID,true);
		}

	}
}

