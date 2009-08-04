using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.Pages;
using Subtext.Framework.Text;

namespace Subtext.Web.Admin.Feedback {
    public partial class EditPage : ConfirmationPage
    {
        public EditPage() 
        {
            this.TabSectionId = "Feedback";
        }

        protected override void OnLoad(EventArgs e) 
        {
            if (FeedbackId == NullValue.NullInt32) 
            {
                Response.Redirect(CancelUrl);
                return;
            }

            if (!IsPostBack)
                BindFeedbackEdit();
            base.OnLoad(e);
        }

        private void BindFeedbackEdit() 
        {
            FeedbackItem currentFeedback = FeedbackItem.Get(FeedbackId);
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
            ConfirmationPage confirmPage = (ConfirmationPage)this.Page;
            confirmPage.IsInEdit = true;
            confirmPage.Message = "You will lose any unsaved content";

            this.lkbPost.Attributes.Add("OnClick", BypassFunctionName);
        }

        bool ReturnToOriginalPost 
        {
            get 
            {
                string returnText = Request.QueryString["return-to-post"] ?? "false";
                return String.Equals(returnText, "true", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private void UpdateFeedback() {
            Uri feedbackWebsite = null;
            if (txbWebsite.Text.Length > 0) {
                valtxbWebsite.IsValid = Uri.TryCreate(txbWebsite.Text, UriKind.RelativeOrAbsolute, out feedbackWebsite);
            }
            else {
                valtxbWebsite.IsValid = true;
            }

            if (Page.IsValid) {
                try {
                    FeedbackItem updatedFeedback = FeedbackItem.Get(FeedbackId);
                    updatedFeedback.Title = txbTitle.Text;
                    updatedFeedback.Body = richTextEditor.Text;
                    if (feedbackWebsite != null)
                        updatedFeedback.SourceUrl = feedbackWebsite;
                    FeedbackItem.Update(updatedFeedback);

                    if (ReturnToOriginalPost) 
                    {
                        if (updatedFeedback != null) {
                            Response.Redirect(Url.FeedbackUrl(updatedFeedback));
                            return;
                        }
                    }

                    this.Messages.ShowMessage(Constants.RES_SUCCESSEDIT, false);
                }
                finally {
                }
            }
        }

        protected string CancelUrl 
        {
            get 
            {
                if (FeedbackId > -1 && ReturnToOriginalPost) 
                {
                    // We came from outside the post, let's go there.
                    FeedbackItem updatedFeedback = FeedbackItem.Get(FeedbackId);
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
                if (feedbackId == NullValue.NullInt32) 
                {
                    string feedbackIDText = Request.QueryString["FeedbackID"] ?? " ";
                    int id;
                    if(int.TryParse(feedbackIDText, out id))
                    {
                        feedbackId = id;
                    }
                }
                return feedbackId;
            }
        }
        int feedbackId = NullValue.NullInt32;

        protected void lkbPost_Click(object sender, EventArgs e) {
            UpdateFeedback();
        }

        protected new FeedbackMaster Master 
        {
            get 
            {
                return base.Master as FeedbackMaster;
            }
        }

    }
}
