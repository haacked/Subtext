using System;
using System.Web;
using System.Web.UI;
using Subtext.Common.Data;
using Subtext.Framework.Components;

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

namespace Subtext.Web.UI.Controls
{
	using System;

	/// <summary>
	///		Summary description for Comments.
	/// </summary>
	public class PostComment : BaseControl
	{
		protected System.Web.UI.WebControls.TextBox tbTitle;
		protected System.Web.UI.WebControls.TextBox tbName;
		protected System.Web.UI.WebControls.TextBox tbUrl;
		protected System.Web.UI.WebControls.TextBox tbComment;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected System.Web.UI.WebControls.Label Message;
		protected System.Web.UI.WebControls.CheckBox chkRemember;
		


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
				}

				Entry entry = Cacher.GetEntryFromRequest(Context, CacheTime.Short);	

				if(CurrentBlog.EnableComments && entry != null && entry.AllowComments && !entry.CommentingClosed)
				{
					//Need to get this without a db hit?
					tbTitle.Text = "re: " + entry.Title;
				}
				else
				{
					if(!entry.CommentingClosed)
						this.Visible = false;
					else
					{
						this.Controls.Clear();
						this.Controls.Add(new LiteralControl("<div class=\"commentsClosedMessage\"><span style=\"font-style: italic;\">Comments have been closed on this topic.</span></div>"));
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
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			//this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				try
				{

					Entry currentEntry =  Cacher.GetEntryFromRequest(Context,CacheTime.Short);	
					
					Entry entry = new Entry(PostType.Comment);
					entry.Author = tbName.Text;
					entry.TitleUrl =  Framework.Util.Globals.CheckForUrl(tbUrl.Text);
					entry.Body = tbComment.Text;
					entry.Title = tbTitle.Text;
					entry.ParentID = currentEntry.EntryID;
					entry.SourceName = Subtext.Framework.Util.Globals.GetUserIpAddress(Context);
					entry.SourceUrl = currentEntry.Link;


					Subtext.Framework.Entries.InsertComment(entry);

					if(chkRemember.Checked)
					{
						HttpCookie user = new HttpCookie("CommentUser");
						user.Values["Name"] = tbName.Text;
						user.Values["Url"] = tbUrl.Text;
						user.Expires = DateTime.Now.AddDays(30);
						Response.Cookies.Add(user);
					}
					
					Response.Redirect(string.Format("{0}?Pending=true",Request.Path));
					//BindComments();
					
				}
				catch{}
			}
		}
	}
}

