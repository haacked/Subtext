using System;
using System.Data.SqlClient;
using Subtext.Framework;

namespace Subtext.Web
{
	/// <summary>
	/// This page presents useful information to users connecting 
	/// to the blog via "localhost".  In otherwords, on a local 
	/// installation.
	/// </summary>
	public class CheckYourConnectionString : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblErrorMessage;
		protected Subtext.Web.Controls.ContentRegion MPTitleBar;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.Label lblStackTrace;
		protected System.Web.UI.WebControls.PlaceHolder plcDiagnosticInfo;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Remote users do not get the extra information.
			if(Security.UserIsConnectingLocally)
			{
				plcDiagnosticInfo.Visible = true;

				Exception exception = Server.GetLastError();
				Exception baseException = null;
				if(exception != null)
				{
					baseException = exception.GetBaseException();
				}

				if(baseException != null)
				{
					lblErrorMessage.Text = baseException.Message;
					lblStackTrace.Text = baseException.StackTrace;

					SqlException sqlexc = baseException as SqlException;
					if(sqlexc != null)
					{
						Response.Write(sqlexc.Number);
					}
				}
				else
				{
					lblErrorMessage.Text = "Nothing to report. There was no error.";
				}
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
