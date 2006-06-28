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

namespace Subtext.Web.Admin.Pages
{
	public partial class Confirm : AdminPage
	{	
	    public Confirm() : base()
	    {
            this.TabSectionId = "Posts";
	    }
	    
		protected void Page_Load(object sender, System.EventArgs e)
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
				lkbContinue.Visible = true;
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

		}
		#endregion

		protected void Yes_Click(object sender, System.EventArgs e)
		{
			if (null != Command)
				HandleCommand(Command.Execute());
		}

		protected void No_Click(object sender, System.EventArgs e)
		{
			if (null != Command)
				HandleCommand(Command.Cancel());
		}

		protected void lkbContinue_Click(object sender, System.EventArgs e)
		{
			if (null != Command)
				Response.Redirect(Command.RedirectUrl);
		}
	}
}

