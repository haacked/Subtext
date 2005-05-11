using System;
using System.Web.UI.WebControls;
using Subtext.Common.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Text;

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
	public class Comments : BaseControl
	{
		protected System.Web.UI.WebControls.Repeater CommentList;
		protected System.Web.UI.WebControls.Literal NoCommentMessage;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(CurrentBlog.EnableComments)
			{

				Entry entry = Cacher.GetEntryFromRequest(Context,CacheTime.Short);	

				if(entry != null && entry.AllowComments)
				{
					BindComments(entry);
				}
				else
				{
					this.Visible = false;
				}
			}
			else
			{
				this.Visible = false;
			}
			
		}


		protected void RemoveComment_ItemCommand(Object Sender, RepeaterCommandEventArgs e) 
		{
				int feedbackItem = Int32.Parse(e.CommandName);
			Entries.Delete(feedbackItem);
				Response.Redirect(string.Format("{0}?Pending=true",Request.Path));
		}

		protected void CommentsCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Entry entry = (Entry)e.Item.DataItem;
				if(entry != null)
				{
					Literal title = (Literal)(e.Item.FindControl("Title"));
					if(title != null)
					{
						// we should probably change skin format to dynamically wire up to 
						// skin located title and permalinks at some point
						title.Text = String.Format("{2}&nbsp;{0}{1}", Anchor(entry.EntryID), 
							entry.Title, Link(entry.Title, entry.Link));
					}

					HyperLink namelink = (HyperLink)e.Item.FindControl("NameLink");
					if(namelink != null)
					{
						if(entry.HasTitleUrl)
						{
							namelink.NavigateUrl = HtmlHelper.CheckForUrl(entry.TitleUrl);
						}
						if(entry.PostType == PostType.Comment)
						{
							namelink.Text = entry.Author;
						}
						else if(entry.PostType == PostType.PingTrack)
						{
							namelink.Text =  entry.Author != null ? entry.Author : "Pingback/TrackBack";
							namelink.Attributes.Add("title","PingBack/TrackBack");
						}
						 
					}

					Literal PostDate = (Literal)(e.Item.FindControl("PostDate"));
					if(PostDate != null)
					{
						PostDate.Text = entry.DateCreated.ToShortDateString() + " " + entry.DateCreated.ToShortTimeString();
					}

					Literal Post = (Literal)(e.Item.FindControl("PostText"));
					if(Post != null)
					{
						Post.Text = entry.Body;
					}
						if(Request.IsAuthenticated && Security.IsAdmin)
						{
							LinkButton editlink = (LinkButton)(e.Item.FindControl("EditLink"));
							if(editlink != null)
							{
								//editlink.CommandName = "Remove";
								editlink.Text = "Remove Comment " + entry.EntryID.ToString();
								editlink.CommandName = entry.EntryID.ToString();
								editlink.Attributes.Add("onclick","return confirm(\"Are you sure you want to delete comment " + entry.EntryID.ToString() + "?\");");
								editlink.Visible = true;
								editlink.CommandArgument = entry.EntryID.ToString();

							}
							else
							{
								editlink.Visible = false;
							}
					}
				}
			}
		}

		const string linktag = "<a title=\"permalink: {0}\" href=\"{1}\">#</a>";
		private string Link(string title, string link)
		{
			return string.Format(linktag,title,link);
		}

		// GC: xhmtl format wreaking havoc in non-xhtml pages in non-IE, changed to non nullable format
		const string anchortag = "<a name=\"{0}\"></a>";
		private string Anchor(int ID)
		{
			return string.Format(anchortag,ID);
		}

		void BindComments(Entry entry)
		{
				try
				{
					if(Request.QueryString["Pending"] != null)
					{
						Cacher.ClearCommentCache(entry.EntryID,Context);
					}
					CommentList.DataSource = Cacher.GetComments(entry,CacheTime.Short,Context);
					CommentList.DataBind();

					if(CommentList.Items.Count == 0)
					{
						CommentList.Visible = false;
						this.NoCommentMessage.Text = "No comments posted yet.";
					}

				}
				catch
				{
					this.Visible = false;
				}
		}
	}
}

