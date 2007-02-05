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
using Subtext.Framework.Services;

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
		    if (!IsPostBack)
		    {
		        BlogInfo info = Config.CurrentBlog;
			
		        this.chkEnableComments.Checked = info.CommentsEnabled;
		        this.chkEnableCommentModeration.Checked = info.ModerationEnabled;
		        this.chkEnableTrackbacks.Checked = info.TrackbacksEnabled;
		        this.chkCoCommentEnabled.Checked = info.CoCommentsEnabled;
		        this.chkAllowDuplicates.Checked = info.DuplicateCommentsEnabled;
		        this.chkEnableCaptcha.Checked = info.CaptchaEnabled;
                this.chkEnableCommentEmail.Checked = info.CommentNoficationEnabled;
                this.chkEnableTrackbackEmails.Checked = info.TrackbackNoficationEnabled;

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
                info.CommentNoficationEnabled = this.chkEnableCommentEmail.Checked;
                info.TrackbackNoficationEnabled = this.chkEnableTrackbackEmails.Checked;
				
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
				this.Messages.ShowMessage(RES_SUCCESS);
			}
		}

		bool IsPageValid
		{
			get
			{
				if (this.txtCommentDelayIntervalMinutes.Text.Length > 0)
					ValidateIntegerRange("Comment Delay", this.txtCommentDelayIntervalMinutes.Text, 0, 3600, @"""{0}"" should larger than {1}. You can't go back in time.", @"""{0}"" of {1} would block an IP for a really long time. Let's be reasonable.");
				
				if(this.txtDaysTillCommentsClosed.Text.Length > 0)
					ValidateInteger("Days Till Comments Close", this.txtDaysTillCommentsClosed.Text, 0, int.MaxValue);

				if (this.txtNumberOfRecentComments.Text.Length > 0)
					ValidateInteger("Number Of Recent Comments", this.txtNumberOfRecentComments.Text, 0, int.MaxValue);

				if (this.txtRecentCommentsLength.Text.Length > 0)
					ValidateInteger("Recent Comments Length", this.txtRecentCommentsLength.Text, 0, int.MaxValue);
				
				if(!String.IsNullOrEmpty(this.txtAkismetAPIKey.Text))
				{
					AkismetSpamService akismet = new AkismetSpamService(this.txtAkismetAPIKey.Text, Config.CurrentBlog);
					try
					{
						if (!akismet.VerifyApiKey())
						{
							this.Messages.ShowError("Sorry, could not verify that Akismet API key.");
							return false;
						}
					}
					catch(System.Security.SecurityException e)
					{
						this.Messages.ShowError(string.Format("Akismet requires <code>{0}</code> in order to make web requests. Please ask your hosting provider to <a href=\"http://weblogs.asp.net/hosterposter/archive/2006/03/22/440886.aspx\" title=\"Enabling WebPermission in Medium Trust\">enable this permission</a>.", e.PermissionType));
						return false;
					}
				}
				
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
			this.lkbPost.Click += this.lkbPost_Click;
			this.Load += this.Page_Load;

		}
		#endregion

		private void lkbPost_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		static int ValidateInteger(string fieldName, string value, int minAllowedValue, int maxAllowedValue)
		{
			return ValidateIntegerRange(fieldName, value, minAllowedValue, maxAllowedValue, @"""{0}"" should be larger than or equal to {1}", @"""{0}"" should be less than or equal to {1}");
		}

		static int ValidateIntegerRange(string fieldName, string value, int minAllowedValue, int maxAllowedValue, string tooSmallFormatMessage, string tooBigFormatMessage)
		{
			try
			{
				int theNumber = int.Parse(value);
				if (theNumber < minAllowedValue)
				{
					throw new ArgumentException(string.Format(tooSmallFormatMessage, fieldName, minAllowedValue), fieldName);
				}
				if (theNumber > maxAllowedValue)
				{
					throw new ArgumentException(string.Format(tooBigFormatMessage, fieldName, maxAllowedValue), fieldName);
				}
				return theNumber;
			}
			catch (FormatException)
			{
				throw new ArgumentException(string.Format("Please enter a valid positive number for the field \"{0}\"", fieldName), fieldName);
			}
		}
	}
}
