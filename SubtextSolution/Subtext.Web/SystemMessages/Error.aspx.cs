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
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;

namespace Subtext.Web.Pages
{
	public partial class Error : Page
	{
		private readonly static ILog log = new Log();

		protected Label ErrorTitle;
	
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Clear();
			if (!IsPostBack)
			{
			    Response.StatusCode = 500;
			    Response.StatusDescription = "500 Internal Server Error";

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
				
				if (exception is FileNotFoundException)
				{
					exceptionMsgs.Append("<p>The resource you requested could not be found.</p>");
				}
				else if(exception is BlogInactiveException)
				{
					log.Warn("Blog Inactive Exception", exception);
					exceptionMsgs.AppendFormat("<p>{0}</p>", exception.Message);
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

		}
		#endregion
	}
}

