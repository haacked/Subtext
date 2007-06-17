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
using System.Net.Mail;
using System.Net.Sockets;
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

		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
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
				else if(exception is SmtpException)
				{
					log.Error("Exception handled by the Error page.", exception);
					exceptionMsgs.Append("<p>Could not send email. Could be an issue with the mail server settings.</p>");
				}
				else if(exception is BlogInactiveException)
				{
					log.Info("Blog Inactive Exception", exception);
					exceptionMsgs.AppendFormat("<p>{0}</p>", exception.Message);
				}
				else
				{
					log.Error("Exception handled by the Error page.", exception);
					exceptionMsgs.AppendFormat("<p>{0}</p>", exception.Message);
				}

				Server.ClearError();
				this.ErrorMessageLabel.Text = exceptionMsgs.ToString();

				base.OnInit(e);
			}
		}
	}
}

