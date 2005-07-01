using System;
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin
{
	/// <summary>
	/// Summary description for Comments.
	/// </summary>
	public class Comments : System.Web.UI.Page
	{
		private const string RES_SUCCESS = "Your comment settings were successfully updated.";
		private const string RES_FAILURE = "Comment settings update failed.";

		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected System.Web.UI.WebControls.CheckBox chkEnableComments;
		protected System.Web.UI.WebControls.TextBox txtCommentDelayIntervalMinutes;
		protected System.Web.UI.WebControls.TextBox txtDaysTillCommentsClosed;
		protected System.Web.UI.WebControls.LinkButton lkbPost;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip1;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip2;
		protected Subtext.Web.Controls.HelpToolTip Helptooltip3;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				PopulateForm();
			}
			ManageHiddenSettings();
		}

		private void PopulateForm()
		{
			BlogInfo info = Config.CurrentBlog;
			
			this.chkEnableComments.Checked = info.CommentsEnabled;
			
			if(info.DaysTillCommentsClose > -1 && info.DaysTillCommentsClose < int.MaxValue)
                this.txtDaysTillCommentsClosed.Text = info.DaysTillCommentsClose.ToString(CultureInfo.InvariantCulture);
			else
				this.txtDaysTillCommentsClosed.Text = string.Empty;
			
			if(info.CommentDelayInMinutes > 0 && info.CommentDelayInMinutes < int.MaxValue)
			{
				this.txtCommentDelayIntervalMinutes.Text = info.CommentDelayInMinutes.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				this.txtCommentDelayIntervalMinutes.Text = string.Empty;
			}
		}

		private void ManageHiddenSettings()
		{
			this.chkEnableComments.Attributes["onclick"] = "toggleHideOnCheckbox(this, 'otherSettings');";
	
			string startupScript = "<script language=\"javascript\">"
				+  Environment.NewLine + "var checkbox = document.getElementById('" + this.chkEnableComments.ClientID + "');"
				+  Environment.NewLine + " toggleHideOnCheckbox(checkbox, 'otherSettings');"
				+  Environment.NewLine +  "</script>";
	
			Page.RegisterStartupScript("startupScript", startupScript);
		}

		private void SaveSettings()
		{
			try
			{
				UpdateConfiguration();
				this.Messages.ShowMessage(RES_SUCCESS);
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
			}
		}

		private void UpdateConfiguration()
		{
			if(IsPageValid)
			{
				BlogInfo info = Config.CurrentBlog;
			
				info.CommentsEnabled = this.chkEnableComments.Checked;
				if(this.txtCommentDelayIntervalMinutes.Text.Length == 0)
				{
					info.CommentDelayInMinutes = 0;
				}
				else
				{
					info.CommentDelayInMinutes = int.Parse(this.txtCommentDelayIntervalMinutes.Text);
				}

				if(this.txtDaysTillCommentsClosed.Text.Length > 0)
				{
					info.DaysTillCommentsClose = ValidateInteger("Days Till Comments Close", txtDaysTillCommentsClosed.Text, 0, int.MaxValue);
				}
				else
				{
					info.DaysTillCommentsClose = int.MaxValue;
				}

				Config.UpdateConfigData(info);
			}
		}

		bool IsPageValid
		{
			get
			{
				string delay = this.txtCommentDelayIntervalMinutes.Text;
				if(delay.Length > 0)
				{
					try
					{
						int delayInMinutes = int.Parse(delay);
						if(delayInMinutes < 0)
						{
							this.Messages.ShowError("You can&#8217;t go back in time, the comment delay should not be a negative number.");
						}
					}
					catch(System.FormatException)
					{
						this.Messages.ShowError("Whoa there, the comment delay should be a valid integer.");
						return false;
					}
				}
				if(txtDaysTillCommentsClosed.Text.Length > 0)
					ValidateInteger("Days Till Comments Close", txtDaysTillCommentsClosed.Text, 0, int.MaxValue);
				return true;
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
			this.lkbPost.Click += new System.EventHandler(this.lkbPost_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lkbPost_Click(object sender, System.EventArgs e)
		{
			SaveSettings();
		}

		int ValidateInteger(string fieldName, string value, int minAllowedValue, int maxAllowedValue)
		{
			try
			{
				int theNumber = int.Parse(value);
				if(theNumber < minAllowedValue)
				{
					throw new ArgumentException("\"" + fieldName + "\" should be larger than or equal to " + minAllowedValue, fieldName);
				}
				if(theNumber > maxAllowedValue)
				{
					throw new ArgumentException("\"" + fieldName + "\" should be less than or equal to " + maxAllowedValue, fieldName);
				}
				return theNumber;
			}
			catch(System.FormatException)
			{
				throw new ArgumentException("Please enter a valid positive number for the field \"" + fieldName + "\"", fieldName);
			}
		}
	}
}
