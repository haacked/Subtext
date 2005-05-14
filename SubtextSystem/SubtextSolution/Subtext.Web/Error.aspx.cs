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
using System.Web.UI;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Pages
{
	public class Error : Page
	{		
		protected System.Web.UI.WebControls.Label ErrorMessageLabel;
		protected System.Web.UI.WebControls.Label ErrorTitle;
		protected System.Web.UI.WebControls.HyperLink HomeLink;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{				
				try
				{				
					if (null != Config.CurrentBlog)
						HomeLink.NavigateUrl = Config.CurrentBlog.FullyQualifiedUrl;
				}
				catch
				{
					HomeLink.Visible = false;
				}

				string noExceptionFound = "No error message available.";
				StringBuilder exceptionMsgs = new StringBuilder();
				
				Exception ex = Server.GetLastError().GetBaseException();//.GetBaseException();
				while (null != ex) // this is obsolete since we're grabbing base...
				{
					if (ex is System.IO.FileNotFoundException)
					{
						exceptionMsgs.Append("<p>The resource you requested could not be found.</p>");
					}
					else
					{
						exceptionMsgs.AppendFormat("<p>{0}</p>", ex.Message);
					}

					ex = ex.InnerException;
				}
			
				Server.ClearError();

				if(exceptionMsgs.Length == 0)
				{
					exceptionMsgs.Append(noExceptionFound);
				}

				
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

