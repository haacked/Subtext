#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using System.Security;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Summary description for Comments.
    /// </summary>
    public partial class Comments : AdminOptionsPage
    {
        bool IsPageValid
        {
            get
            {
                if (txtCommentDelayIntervalMinutes.Text.Length > 0)
                {
                    ValidateIntegerRange(Resources.CommentValidation_CommentDelay, txtCommentDelayIntervalMinutes.Text,
                                         0, 3600, Resources.CommentValidation_CommentDelayTooSmall,
                                         Resources.CommentValidation_CommentDelayTooBig);
                }

                if (txtDaysTillCommentsClosed.Text.Length > 0)
                {
                    ValidateInteger(Resources.CommentValidation_DaysTillClose, txtDaysTillCommentsClosed.Text, 0,
                                    int.MaxValue);
                }

                if (txtNumberOfRecentComments.Text.Length > 0)
                {
                    ValidateInteger(Resources.CommentValidation_RecentCommentsCount, txtNumberOfRecentComments.Text, 0,
                                    int.MaxValue);
                }

                if (txtRecentCommentsLength.Text.Length > 0)
                {
                    ValidateInteger(Resources.CommentValidation_LengthOfRecentComment, txtRecentCommentsLength.Text, 0,
                                    int.MaxValue);
                }

                if (!String.IsNullOrEmpty(txtAkismetAPIKey.Text))
                {
                    var akismet = new AkismetSpamService(txtAkismetAPIKey.Text, Blog, null, Url);
                    try
                    {
                        if (!akismet.VerifyApiKey())
                        {
                            Messages.ShowError(Resources.Comments_CouldNotVerifyAkismetKey);
                            return false;
                        }
                    }
                    catch (SecurityException e)
                    {
                        Messages.ShowError(string.Format(CultureInfo.InvariantCulture,
                                                         Resources.Comments_AkismetRequiresPermissionType,
                                                         e.PermissionType));
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

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        protected override void BindLocalUI()
        {
            Blog info = Config.CurrentBlog;

            chkEnableComments.Checked = info.CommentsEnabled;
            chkEnableCommentModeration.Checked = info.ModerationEnabled;
            chkEnableTrackbacks.Checked = info.TrackbacksEnabled;
            chkCoCommentEnabled.Checked = info.CoCommentsEnabled;
            chkAllowDuplicates.Checked = info.DuplicateCommentsEnabled;
            chkEnableCaptcha.Checked = info.CaptchaEnabled;

            txtAkismetAPIKey.Text = info.FeedbackSpamServiceKey;

            if (info.DaysTillCommentsClose > -1 && info.DaysTillCommentsClose < int.MaxValue)
            {
                txtDaysTillCommentsClosed.Text = info.DaysTillCommentsClose.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                txtDaysTillCommentsClosed.Text = string.Empty;
            }

            if (info.CommentDelayInMinutes > 0 && info.CommentDelayInMinutes < int.MaxValue)
            {
                txtCommentDelayIntervalMinutes.Text = info.CommentDelayInMinutes.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                txtCommentDelayIntervalMinutes.Text = string.Empty;
            }

            if (info.NumberOfRecentComments > 0 && info.NumberOfRecentComments < int.MaxValue)
            {
                txtNumberOfRecentComments.Text = info.NumberOfRecentComments.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                txtNumberOfRecentComments.Text = string.Empty;
            }

            if (info.RecentCommentsLength > 0 && info.RecentCommentsLength < int.MaxValue)
            {
                txtRecentCommentsLength.Text = info.RecentCommentsLength.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                txtRecentCommentsLength.Text = string.Empty;
            }

            base.BindLocalUI();
        }

        private void SaveSettings()
        {
            try
            {
                UpdateConfiguration();
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION, Resources.Comments_SettingsFailed, ex.Message));
            }
        }

        private void UpdateConfiguration()
        {
            if (IsPageValid)
            {
                Blog info = Blog;

                info.CommentsEnabled = chkEnableComments.Checked;
                info.ModerationEnabled = chkEnableCommentModeration.Checked;
                info.CoCommentsEnabled = chkCoCommentEnabled.Checked;
                info.TrackbacksEnabled = chkEnableTrackbacks.Checked;
                info.DuplicateCommentsEnabled = chkAllowDuplicates.Checked;
                info.CaptchaEnabled = chkEnableCaptcha.Checked;

                info.CommentDelayInMinutes = txtCommentDelayIntervalMinutes.Text.Length == 0 ? 0 : int.Parse(txtCommentDelayIntervalMinutes.Text);

                if (txtDaysTillCommentsClosed.Text.Length > 0)
                {
                    info.DaysTillCommentsClose = ValidateInteger(Resources.CommentValidation_DaysTillClose,
                                                                 txtDaysTillCommentsClosed.Text, 0, int.MaxValue);
                }
                else
                {
                    info.DaysTillCommentsClose = int.MaxValue;
                }

                if (txtNumberOfRecentComments.Text.Length > 0)
                {
                    info.NumberOfRecentComments = ValidateInteger(Resources.CommentValidation_RecentCommentsCount,
                                                                  txtNumberOfRecentComments.Text, 0, int.MaxValue);
                }
                else
                {
                    info.NumberOfRecentComments = int.MaxValue;
                }

                if (txtRecentCommentsLength.Text.Length > 0)
                {
                    info.RecentCommentsLength = ValidateInteger(Resources.CommentValidation_LengthOfRecentComment,
                                                                txtRecentCommentsLength.Text, 0, int.MaxValue);
                }
                else
                {
                    info.RecentCommentsLength = int.MaxValue;
                }

                info.FeedbackSpamServiceKey = txtAkismetAPIKey.Text;
                Repository.UpdateConfigData(info);
                Messages.ShowMessage(Resources.Comments_SettingsUpdated);
            }
        }

        private void lkbPost_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        static int ValidateInteger(string fieldName, string value, int minAllowedValue, int maxAllowedValue)
        {
            return ValidateIntegerRange(fieldName, value, minAllowedValue, maxAllowedValue,
                                        Resources.Message_ValueTooSmall, Resources.Message_ValueTooBig);
        }

        static int ValidateIntegerRange(string fieldName, string value, int minAllowedValue, int maxAllowedValue,
                                 string tooSmallFormatMessage, string tooBigFormatMessage)
        {
            try
            {
                int theNumber = int.Parse(value);
                if (theNumber < minAllowedValue)
                {
                    throw new ArgumentException(string.Format(tooSmallFormatMessage, fieldName, minAllowedValue),
                                                fieldName);
                }
                if (theNumber > maxAllowedValue)
                {
                    throw new ArgumentException(string.Format(tooBigFormatMessage, fieldName, maxAllowedValue),
                                                fieldName);
                }
                return theNumber;
            }
            catch (FormatException)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, Resources.Message_ValueMustBePositive, fieldName),
                    fieldName);
            }
        }
    }
}