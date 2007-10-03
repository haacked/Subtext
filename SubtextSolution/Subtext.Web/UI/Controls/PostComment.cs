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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Framework.Security;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for Comments.
	/// </summary>
	public partial class PostComment : BaseControl, IEntryControl
	{
		private Entry entry;

		public Entry Entry
		{
			get
			{
				if(this.entry == null)
				{
					this.entry = Cacher.GetEntryFromRequest(CacheDuration.Short);
				}
				return this.entry;
			}
		}

		/// <summary>
		/// Handles the OnLoad event.  Attempts to prepopulate comment 
		/// fields based on the user's cookie.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			//TODO: Make this configurable.
			tbTitle.MaxLength = 128;
			tbEmail.MaxLength = 128;
			tbName.MaxLength = 32;
			tbUrl.MaxLength = 256;
			tbComment.MaxLength = 4000;
			SetValidationGroup();
		
			if(!IsPostBack)
			{
				if (this.Entry == null)
				{
					//Somebody probably is messing with the url.
					Response.Redirect("~/SystemMessages/FileNotFound.aspx", true);
					return;
				}
				
				ResetCommentFields();

				if(Config.CurrentBlog.CoCommentsEnabled)
				{
					if(coComment == null)
					{
						coComment = new SubtextCoComment();
						PlaceHolder coCommentPlaceHolder = Page.FindControl("coCommentPlaceholder") as PlaceHolder;
						if(coCommentPlaceHolder != null)
						{
							coCommentPlaceHolder.Controls.Add(coComment);
						}
					}
					coComment.PostTitle = entry.Title;
					coComment.PostUrl = entry.Url;
					if(entry.Url.StartsWith("/"))
					{
						coComment.PostUrl = "http://" + Config.CurrentBlog.Host + coComment.PostUrl;
					}
				}
			}

			this.DataBind();
		}
		
		void SetValidationGroup()
		{
			foreach(Control control in this.Controls)
			{
				BaseValidator validator = control as BaseValidator;
				if(validator != null)
				{
					validator.ValidationGroup = "SubtextComment";
					continue;
				}

				Button btn = control as Button;
				if (btn != null)
				{
					btn.ValidationGroup = "SubtextComment";
					continue;
				}

				TextBox textbox = control as TextBox;
				if (textbox != null)
				{
					textbox.ValidationGroup = "SubtextComment";
					continue;
				}
			}
		}

		/// <summary>
		/// Called when an approved comment is added.
		/// </summary>
		protected virtual void OnCommentApproved(FeedbackItem feedback)
		{
			if (feedback.Approved)
			{
				EventHandler<EventArgs> theEvent = this.CommentApproved;
				if (theEvent != null)
					theEvent(this, EventArgs.Empty);
			}
		}

		private void RemoveCommentControls()
		{
			for (int i = this.Controls.Count - 1; i >= 0; i--)
			{
				this.Controls.RemoveAt(i);
			}
		}

		public event EventHandler<EventArgs> CommentApproved;

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			if(this.btnSubmit != null)
			{
				this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			}

			if(this.btnCompliantSubmit != null)
			{
				this.btnCompliantSubmit.Click += new EventHandler(this.btnSubmit_Click);
			}

			//Captcha should not be given to admin.
			if (!SecurityHelper.IsAdmin)
			{
				int btnIndex = 0;
				btnIndex = Controls.IndexOf(this.btnSubmit);
				if (btnIndex < 0)
					btnIndex = Controls.IndexOf(this.btnCompliantSubmit);

				AddCaptchaIfNecessary(ref captcha, ref invisibleCaptchaValidator, btnIndex);
			}
			else
			{
				RemoveCaptcha();
			}
		}

		
		#endregion

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				LastDitchValidation();
				try
				{
					Entry currentEntry =  Cacher.GetEntryFromRequest(CacheDuration.Short);	
					if(IsCommentAllowed)
					{
						FeedbackItem feedbackItem = CreateFeedbackInstanceFromFormInput(currentEntry);
						FeedbackItem.Create(feedbackItem, new CommentFilter(HttpContext.Current.Cache));
						
						if(chkRemember == null || chkRemember.Checked)
						{
							SetRememberedUserCookie();
						}

						DisplayResultMessage(feedbackItem);
					}
				}
				catch(BaseCommentException exception)
				{
					Message.Text = exception.Message;
				}
			}
		}
		
		private void LastDitchValidation()
		{
			//The validation controls and otherwise should catch everything.
			//This is here to be extra safe.
			//Anything triggering these exceptions is probably malicious.
			if (this.tbComment.Text.Length > 4000
				|| this.tbTitle.Text.Length > 128
				|| this.tbEmail.Text.Length > 128
				|| this.tbName.Text.Length > 32
				|| this.tbUrl.Text.Length > 256)
				throw new InvalidOperationException("Sorry, but we cannot accept this comment.");
		}

		private void SetRememberedUserCookie()
		{
			HttpCookie user = new HttpCookie("CommentUser");
			user.Values["Name"] = this.tbName.Text;
			user.Values["Url"] = this.tbUrl.Text;
			if(this.tbEmail!=null)
				user.Values["Email"] = this.tbEmail.Text;
			user.Expires = DateTime.Now.AddDays(30);
			Response.Cookies.Add(user);
		}

		private void DisplayResultMessage(FeedbackItem feedbackItem)
		{
			RemoveCommentControls();
			Message = new Label();
			
			if (feedbackItem.Approved)
			{
				Message.Text = "Thanks for your comment!";
				Message.CssClass = "success";
				this.Controls.Add(Message);	//This needs to be here for ajax calls.
				Cacher.ClearCommentCache(feedbackItem.EntryId);
				OnCommentApproved(feedbackItem);
				return;
			}
			else if(feedbackItem.NeedsModeratorApproval)
			{
				Message.Text = "Thank you for your comment.  It will be displayed soon.";
				Message.CssClass = "error moderation";
			}
			else
			{
				this.Message.Text = "Sorry, but your comment was flagged as spam and will be moderated.";
				this.Message.CssClass = "error";
			}
			this.Controls.Add(Message);
		}

		private FeedbackItem CreateFeedbackInstanceFromFormInput(Entry currentEntry)
		{
			FeedbackItem feedbackItem = new FeedbackItem(FeedbackType.Comment);
			feedbackItem.Author = this.tbName.Text;
			if(this.tbEmail != null)
				feedbackItem.Email = this.tbEmail.Text;
			feedbackItem.SourceUrl =  HtmlHelper.CheckForUrl(this.tbUrl.Text);
			feedbackItem.Body = this.tbComment.Text;
			feedbackItem.Title = this.tbTitle.Text;
			feedbackItem.EntryId = currentEntry.Id;
			feedbackItem.IpAddress = HttpHelper.GetUserIpAddress(Context);
			feedbackItem.IsBlogAuthor = SecurityHelper.IsAdmin;
			return feedbackItem;
		}

		private void ResetCommentFields()
		{
			if (this.tbComment != null)
				this.tbComment.Text = string.Empty;
			
			if (this.tbEmail != null)
				this.tbEmail.Text = string.Empty;
			
			if (this.tbName != null)
				this.tbName.Text = string.Empty;
			
			if(entry == null)
				entry = Cacher.GetEntryFromRequest(CacheDuration.Short);
			
			if (this.tbTitle != null)
				this.tbTitle.Text = "re: " + HttpUtility.HtmlDecode(entry.Title);
			
			if (this.tbUrl != null)
				this.tbUrl.Text = string.Empty;

			HttpCookie user = Request.Cookies["CommentUser"];
			if (user != null)
			{
				tbName.Text = user.Values["Name"];
				tbUrl.Text = user.Values["Url"];

				// Remember by default if no-checkbox.
				if (this.chkRemember != null && this.chkRemember.Checked)
				{
					this.chkRemember.Checked = true;
				}

				//Check to see if email textbox is present
				if (this.tbEmail != null && user.Values["Email"] != null)
				{
					this.tbEmail.Text = user.Values["Email"];
				}
			}

			if (IsCommentsRendered)
			{
				if (entry.CommentingClosed)
				{
					this.Controls.Clear();
					this.Controls.Add(new LiteralControl("<div class=\"commentsClosedMessage\"><span style=\"font-style: italic;\">Comments have been closed on this topic.</span></div>"));
				}
				else
				{
					tbTitle.Text = "re: " + HttpUtility.HtmlDecode(entry.Title);
				}
			}
			else
			{
				this.Controls.Clear();
			}
		}

		bool IsCommentsRendered
		{
			get { return CurrentBlog.CommentsEnabled && Entry != null && Entry.AllowComments; }
		}
		
		bool IsCommentAllowed
		{
			get { return CurrentBlog.CommentsEnabled && Entry != null && Entry.AllowComments && !Entry.CommentingClosed; }
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			WebControl submitButton = this.btnSubmit ?? this.btnCompliantSubmit;

			string script = @"<script type=""text/javascript"">" + Environment.NewLine
							+ "function SubmitComment(button) {" + Environment.NewLine
							+ "  if (typeof(Page_ClientValidate) == 'function') { " + Environment.NewLine
							+ "    if (Page_ClientValidate() == false) { return false; }" + Environment.NewLine
							+ "  }" + Environment.NewLine
							+ "  button.disabled = true;" + Environment.NewLine
							+ "  button.value = 'Submitting...';"
							+ "  " + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(submitButton)) + ";" + Environment.NewLine
							+ "  return true;" + Environment.NewLine
							+ "}" + Environment.NewLine
							+ "</script>";

			if (!Page.ClientScript.IsClientScriptBlockRegistered("SubmitCommentScript"))
				Page.ClientScript.RegisterClientScriptBlock(typeof(PostComment), "SubmitCommentScript", script);

			submitButton.Attributes.Add("onclick", "SubmitComment(this);");
		}
	}
}


