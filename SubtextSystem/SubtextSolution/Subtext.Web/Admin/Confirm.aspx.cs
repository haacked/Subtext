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

namespace Subtext.Web.Admin.Pages
{
	public class Confirm : AdminPage
	{
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Header;
		protected System.Web.UI.WebControls.Label lblOutput;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
		protected System.Web.UI.WebControls.LinkButton lkbYes;
		protected System.Web.UI.WebControls.HyperLink lnkContinue;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected System.Web.UI.WebControls.LinkButton lkbNo;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				Command = (Context.Handler as AdminPage).Command;
				lblOutput.Text = Command.PromptMessage;
			}
		}

		private new ConfirmCommand Command
		{
			get
			{
				if (null != ViewState["Command"])
					return (ConfirmCommand)ViewState["Command"];
				else
				{
					// could throw here but we'll eat it for now
					// throw new NotImplementedException("ConfirmCommand was not set upon transfer.");
					this.Messages.ShowError("ConfirmCommand was not set upon transfer.");
					return null;
				}
			}
			set { ViewState["Command"] = value; }
		}

		private void HandleCommand(string results)
		{
			if (Command.AutoRedirect)
				Response.Redirect(Command.RedirectUrl);
			else
			{
				lblOutput.Text = results;
				lnkContinue.NavigateUrl = Command.RedirectUrl;
				lnkContinue.Visible = true;
				lkbYes.Visible = false;
				lkbNo.Visible = false;				
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
			this.lkbYes.Click += new System.EventHandler(this.Yes_Click);
			this.lkbNo.Click += new System.EventHandler(this.No_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Yes_Click(object sender, System.EventArgs e)
		{
			if (null != Command)
				HandleCommand(Command.Execute());
		}

		private void No_Click(object sender, System.EventArgs e)
		{
			if (null != Command)
				HandleCommand(Command.Cancel());
		}
	}
}

