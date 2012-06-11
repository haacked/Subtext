using System;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.Feedback
{
    public partial class EditPage : ConfirmationPage
    {
        int _feedbackId = NullValue.NullInt32;

        public EditPage()
        {
            TabSectionId = "Feedback";
        }

        bool ReturnToOriginalPost
        {
            get
            {
                string returnText = Request.QueryString["return-to-post"] ?? "false";
                return String.Equals(returnText, "true", StringComparison.OrdinalIgnoreCase);
            }
        }

        protected string CancelUrl
        {
            get
            {
                if (FeedbackId > -1 && ReturnToOriginalPost)
                {
                    // We came from outside the post, let's go there.
                    FeedbackItem updatedFeedback = Repository.Get(FeedbackId);
                    if (updatedFeedback != null)
                    {
                        return Url.FeedbackUrl(updatedFeedback);
                    }
                }
                //Go back to the list.
                return Master.ListUrl();
            }
        }

        public int FeedbackId
        {
            get
            {
                if (_feedbackId == NullValue.NullInt32)
                {
                    string feedbackIdText = Request.QueryString["FeedbackID"] ?? " ";
                    int id;
                    if (int.TryParse(feedbackIdText, out id))
                    {
                        _feedbackId = id;
                    }
                }
                return _feedbackId;
            }
        }

        protected new FeedbackMaster Master
        {
            get { return base.Master as FeedbackMaster; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (FeedbackId == NullValue.NullInt32)
            {
                Response.Redirect(CancelUrl);
                return;
            }

            if (!IsPostBack)
            {
                BindFeedbackEdit();
            }
            base.OnLoad(e);
        }

        private void BindFeedbackEdit()
        {
            FeedbackItem currentFeedback = Repository.Get(FeedbackId);
            if (currentFeedback == null)
            {
                Response.Redirect("./");
                return;
            }

            SetConfirmation();
            lblName.Text = currentFeedback.Author;
            lblEmail.Text = currentFeedback.Email;
            if (currentFeedback.Email.Length > 0)
            {
                hlAuthorEmail.NavigateUrl = "mailto:" + currentFeedback.Email;
            }

            hlEntryLink.NavigateUrl = Url.FeedbackUrl(currentFeedback);
            hlEntryLink.Text = Url.FeedbackUrl(currentFeedback);
            if (currentFeedback.SourceUrl != null)
            {
                txbWebsite.Text = currentFeedback.SourceUrl.ToString();
            }

            txbTitle.Text = currentFeedback.Title;

            richTextEditor.Text = currentFeedback.Body;
        }

        private void SetConfirmation()
        {
            var confirmPage = (ConfirmationPage)Page;
            confirmPage.IsInEdit = true;
            confirmPage.Message = "You will lose any unsaved content";

            lkbPost.Attributes.Add("OnClick", BypassFunctionName);
        }

        private void UpdateFeedback()
        {
            Uri feedbackWebsite = null;
            valtxbWebsite.IsValid = !(txbWebsite.Text.Length > 0) || Uri.TryCreate(txbWebsite.Text, UriKind.RelativeOrAbsolute, out feedbackWebsite);

            if (Page.IsValid)
            {
                FeedbackItem updatedFeedback = Repository.Get(FeedbackId);
                updatedFeedback.Title = txbTitle.Text;
                updatedFeedback.Body = richTextEditor.Text;
                if (feedbackWebsite != null)
                {
                    updatedFeedback.SourceUrl = feedbackWebsite;
                }
                Repository.Update(updatedFeedback);
                Cacher.InvalidateFeedback(updatedFeedback.Entry, SubtextContext);
                if (ReturnToOriginalPost)
                {
                    Response.Redirect(Url.FeedbackUrl(updatedFeedback));
                    return;
                }


                Messages.ShowMessage(Constants.RES_SUCCESSEDIT, false);
            }
        }

        protected void lkbPost_Click(object sender, EventArgs e)
        {
            UpdateFeedback();
        }
    }
}