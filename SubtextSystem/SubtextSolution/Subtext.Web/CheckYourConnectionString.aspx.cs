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
		protected System.Web.UI.WebControls.Label lblStackTrace;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Remote users do not get the extra information.
			if(Security.UserIsConnectingLocally)
			{
				Exception exception = Server.GetLastError().GetBaseException();
				if(exception != null)
				{
					lblErrorMessage.Text = exception.Message;
					lblStackTrace.Text = exception.StackTrace;

					SqlException sqlexc = exception as SqlException;
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
