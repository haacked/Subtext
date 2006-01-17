using System;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace Subtext.Web
{
	/// <summary>
	/// Displays the blog not active message.
	/// </summary>
	public class BlogNotActive : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.PlaceHolder plcInactiveBlogMessage;
		protected System.Web.UI.WebControls.PlaceHolder plcNothingToSeeHere;
		protected Subtext.Web.Controls.ContentRegion MPTitleBar;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.HtmlControls.HtmlAnchor hostAdminLink;
		protected System.Web.UI.HtmlControls.HtmlAnchor lnkBlog;

		private void Page_Load(object sender, System.EventArgs e)
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
					lnkBlog.HRef = Config.CurrentBlog.BlogHomeUrl;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
