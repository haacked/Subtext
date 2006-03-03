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
using System.ComponentModel;
using System.Web.Services;
using Subtext.Extensibility;
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

