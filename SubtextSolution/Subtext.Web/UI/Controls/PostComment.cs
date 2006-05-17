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
using Subtext.Common.Data;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Text;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for Comments.
	/// </summary>
	public class PostComment : BaseControl
	{
		protected System.Web.UI.WebControls.TextBox tbTitle;
		protected System.Web.UI.WebControls.TextBox tbName;
		protected System.Web.UI.WebControls.TextBox tbUrl;
		protected System.Web.UI.WebControls.TextBox tbComment;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected Subtext.Web.Controls.CompliantButton btnCompliantSubmit;
		protected System.Web.UI.WebControls.Label Message;
		protected System.Web.UI.WebControls.CheckBox chkRemember;
		protected SubtextCoComment coComment;
		
		/// <summary>
		/// Handles the OnLoad event.  Attempts to prepopulate comment 
		/// fields based on the user's cookie.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			tbComment.MaxLength = 4000;
		
			if(!IsPostBack)
			{
				HttpCookie user = Request.Cookies["CommentUser"];
				if(user != null)
				{
					tbName.Text = user.Values["Name"];
					tbUrl.Text = user.Values["Url"];
					
					// Remember by default if no-checkbox.
					if(this.chkRemember != null && this.chkRemember.Checked)
					{
						this.chkRemember.Checked = true;
					}
					
					//Check to see if email textbox is present
					if(this.tbEmail!=null && user.Values["Email"]!=null)
					{
						this.tbEmail.Text = user.Values["Email"];
					}
				}

				Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);
				if(entry == null)
				{
					//Somebody probably is messing with the url.
					Response.StatusCode = 404;
					Response.Redirect("~/SystemMessages/FileNotFound.aspx", true);
					return;
				}

				if (IsCommentsRendered(entry))
				{
					if (entry.CommentingClosed)
					{
						this.Controls.Clear();
						this.Controls.Add(new LiteralControl("<div class=\"commentsClosedMessage\"><span style=\"font-style: italic;\">Comments have been closed on this topic.</span></div>"));
					}
					else
					{
						tbTitle.Text = "re: " + entry.Title;
					}
				}
				else
				{
					this.Controls.Clear();
				}

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
					coComment.PostUrl = entry.Link;
					if(entry.Link.StartsWith("/"))
					{
						coComment.PostUrl = "http://" + Config.CurrentBlog.Host + coComment.PostUrl;
					}
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
			//this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				try
				{
					Entry currentEntry =  Cacher.GetEntryFromRequest(CacheDuration.Short);	
					if(IsCommentAllowed(currentEntry))
					{
						Entry entry = new Entry(PostType.Comment);
						entry.Author = tbName.Text;
						if(this.tbEmail!=null)
							entry.Email = tbEmail.Text;
						entry.TitleUrl =  HtmlHelper.CheckForUrl(tbUrl.Text);
						entry.Body = tbComment.Text;
						entry.Title = tbTitle.Text;
						entry.ParentID = currentEntry.EntryID;
						entry.SourceName = Subtext.Framework.Util.Globals.GetUserIpAddress(Context);
						entry.SourceUrl = currentEntry.Link;

						Entries.InsertComment(entry);

						if(chkRemember == null || chkRemember.Checked)
						{
							HttpCookie user = new HttpCookie("CommentUser");
							user.Values["Name"] = tbName.Text;
							user.Values["Url"] = tbUrl.Text;
							if(this.tbEmail!=null)
								user.Values["Email"] = tbEmail.Text;
							user.Expires = DateTime.Now.AddDays(30);
							Response.Cookies.Add(user);
						}
					}
					Response.Redirect(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?Pending=true", Request.Path));
				}
				catch(BaseCommentException exception)
				{
					Message.Text = exception.Message;
				}
			}
		}
		
		bool IsCommentsRendered(Entry entry)
		{
			return CurrentBlog.CommentsEnabled && entry != null && entry.AllowComments;
		}
		
		bool IsCommentAllowed(Entry entry)
		{
			return CurrentBlog.CommentsEnabled && entry != null && entry.AllowComments && !entry.CommentingClosed;
		}
	}
}

