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
						HomeLink.NavigateUrl = Config.CurrentBlog.HomeVirtualUrl;
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

