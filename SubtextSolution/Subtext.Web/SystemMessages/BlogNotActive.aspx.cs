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
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace Subtext.Web
{
	/// <summary>
	/// Displays the blog not active message.
	/// </summary>
	public partial class BlogNotActive : System.Web.UI.Page
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if(!Config.CurrentBlog.IsActive)
				{
					plcInactiveBlogMessage.Visible = true;
					plcNothingToSeeHere.Visible = false;
				}
				else
				{
					lnkBlog.HRef = Config.CurrentBlog.HomeVirtualUrl;
				}
			}
			catch(BlogDoesNotExistException)
			{
				plcInactiveBlogMessage.Visible = true;
				plcNothingToSeeHere.Visible = false;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
