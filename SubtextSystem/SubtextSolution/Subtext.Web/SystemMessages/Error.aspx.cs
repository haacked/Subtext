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
using System.Text;
using System.Web;
using System.Web.UI;
using log4net;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Pages
{
	public class Error : Page
	{
		private readonly static ILog log = new Subtext.Framework.Logging.Log();

		protected System.Web.UI.WebControls.Label ErrorMessageLabel;
		protected System.Web.UI.WebControls.Label ErrorTitle;
		protected Subtext.Web.Controls.ContentRegion MPTitleBar;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.HyperLink HomeLink;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{				
				try
				{				
					if (null != Config.CurrentBlog)
						HomeLink.NavigateUrl = Config.CurrentBlog.BlogHomeUrl;
				}
				catch
				{
					HomeLink.Visible = false;
				}				
				
				Exception exception = Server.GetLastError();
				if(exception == null || exception is HttpUnhandledException)
				{
					if(exception == null || exception.InnerException == null)
					{
						// There is no exception. User probably browsed here.
						this.ErrorMessageLabel.Text = "No error message available.";
						return;
					}
					exception = exception.InnerException;
				}

				StringBuilder exceptionMsgs = new StringBuilder();
				
				if (exception is System.IO.FileNotFoundException)
				{
					exceptionMsgs.Append("<p>The resource you requested could not be found.</p>");
				}
				else
				{
					log.Error("Exception handled by the Error page.", exception);
					exceptionMsgs.AppendFormat("<p>{0}</p>", exception.Message);
				}

				Server.ClearError();
				this.ErrorMessageLabel.Text = exceptionMsgs.ToString();
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

