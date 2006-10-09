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
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Summary description for Comments.
	/// </summary>
	public partial class Comments : AdminOptionsPage
	{
		private const string RES_SUCCESS = "Your comment settings were successfully updated.";
		private const string RES_FAILURE = "Comment settings update failed.";
	    
		protected override void Page_Load(object sender, EventArgs e)
		{
			base.Page_Load(sender, e);
			ManageHiddenSettings();
		}

		protected override void BindLocalUI()
		{
			BlogInfo info = Config.CurrentBlog;
			
			this.chkEnableComments.Checked = info.CommentsEnabled;
			this.chkEnableCommentModeration.Checked = info.ModerationEnabled;
			this.chkEnableTrackbacks.Checked = info.TrackbacksEnabled;
			this.chkCoCommentEnabled.Checked = info.CoCommentsEnabled;
			this.chkAllowDuplicates.Checked = info.DuplicateCommentsEnabled;
			this.chkEnableCaptcha.Checked = info.CaptchaEnabled;

			this.txtAkismetAPIKey.Text = info.FeedbackSpamServiceKey;
			
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

			if(info.NumberOfRecentComments > 0 && info.NumberOfRecentComments < int.MaxValue)
			{
				this.txtNumberOfRecentComments.Text = info.NumberOfRecentComments.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				this.txtNumberOfRecentComments.Text = string.Empty;
			}

			if(info.RecentCommentsLength > 0 && info.RecentCommentsLength < int.MaxValue)
			{
				this.txtRecentCommentsLength.Text = info.RecentCommentsLength.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				this.txtRecentCommentsLength.Text = string.Empty;
			}
			
			base.BindLocalUI();
		}

		private void ManageHiddenSettings()
		{
			this.chkEnableComments.Attributes["onclick"] = "toggleHideOnCheckbox(this, 'otherSettings');";
	
			string startupScript = "<script type=\"text/javascript\">"
				+  Environment.NewLine + "var checkbox = document.getElementById('" + this.chkEnableComments.ClientID + "');"
				+  Environment.NewLine + " toggleHideOnCheckbox(checkbox, 'otherSettings');"
				+  Environment.NewLine +  "</script>";
	
			Type ctype = this.GetType();
			Page.ClientScript.RegisterStartupScript(ctype,"startupScript", startupScript);
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
				info.ModerationEnabled = this.chkEnableCommentModeration.Checked;
				info.CoCommentsEnabled = this.chkCoCommentEnabled.Checked;
				info.TrackbacksEnabled = this.chkEnableTrackbacks.Checked;
				info.DuplicateCommentsEnabled = this.chkAllowDuplicates.Checked;
				info.CaptchaEnabled = this.chkEnableCaptcha.Checked;
				
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

				if(this.txtNumberOfRecentComments.Text.Length > 0)
				{
					info.NumberOfRecentComments = ValidateInteger("Number of Recent Comments to Display", txtNumberOfRecentComments.Text, 0, int.MaxValue);
				}
				else
				{
					info.NumberOfRecentComments = int.MaxValue;
				}

				if(this.txtRecentCommentsLength.Text.Length > 0)
				{
					info.RecentCommentsLength = ValidateInteger("Length of Recent Comments to Display (Number of characters)", txtRecentCommentsLength.Text, 0, int.MaxValue);
				}
				else
				{
					info.RecentCommentsLength = int.MaxValue;
				}

				info.FeedbackSpamServiceKey = this.txtAkismetAPIKey.Text;
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
					catch(FormatException)
					{
						this.Messages.ShowError("Whoa there, the comment delay should be a valid integer.");
						return false;
					}
				}
				if(txtDaysTillCommentsClosed.Text.Length > 0)
					ValidateInteger("Days Till Comments Close", txtDaysTillCommentsClosed.Text, 0, int.MaxValue);
				return true;

/*				if(txtNumberOfRecentComments.Text.Length > 0)
					ValidateInteger("Number of Recent Comments to Display", txtNumberOfRecentComments.Text, 0, int.MaxValue);
				return true;

				if(txtRecentCommentsLength.Text.Length > 0)
					ValidateInteger("Length of Recent Comments to Display (Number of characters)", txtRecentCommentsLength.Text, 0, int.MaxValue);
				return true;*/
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
			this.lkbPost.Click += this.lkbPost_Click;
			this.Load += this.Page_Load;

		}
		#endregion

		private void lkbPost_Click(object sender, EventArgs e)
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
			catch(FormatException)
			{
				throw new ArgumentException("Please enter a valid positive number for the field \"" + fieldName + "\"", fieldName);
			}
		}
	}
}
