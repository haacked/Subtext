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
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Extensibility;
using Subtext.Framework.Configuration;    
using Subtext.Framework.Components;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Displays the most recent comments on the skin.
	/// </summary>
	public class RecentComments : BaseControl
	{
		private const int DefaultRecentPostCount = 5;
		protected System.Web.UI.WebControls.Repeater feedList;
		private EntryCollection comments;

		/// <summary>
		/// Initializes a new instance of the <see cref="RecentComments"/> class.
		/// </summary>
		public RecentComments()
		{
			if(Config.CurrentBlog.NumberOfRecentComments > 0)
			{
				comments = Entries.GetRecentPosts(Config.CurrentBlog.NumberOfRecentComments, PostType.Comment, true);
			}
			else
			{
				comments = Entries.GetRecentPosts(DefaultRecentPostCount, Subtext.Extensibility.PostType.Comment, true);
			}

			for(int i = 0; i < comments.Count; i++)
			{
				if(comments[i].ParentID <= 0)
				{
					comments.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Binds the comments <see cref="EntryCollection"/> to the comment list repeater.
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(comments != null)
			{
				feedList.DataSource = comments;
				feedList.DataBind();
			}
			else
			{
				this.Controls.Clear();
				this.Visible = false;
			}

		}

		protected void EntryCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Entry entry = (Entry)e.Item.DataItem;
				
				HyperLink title = (HyperLink)e.Item.FindControl("Link");
				if(title != null)
				{
					int commentLength = CurrentBlog.RecentCommentsLength;
					if (entry.Body.Length > commentLength) 
					{
						string truncatedText = string.Empty;
						if (commentLength > 0)
						{
							truncatedText = entry.Body.Substring(0, commentLength);
						}

						title.Text = truncatedText + "...";
						title.NavigateUrl = entry.Link;
					} 
					else
					{
						title.Text = entry.Body;
						title.NavigateUrl = entry.Link;
					}
					ControlHelper.SetTitleIfNone(title, "Reader Comment.");
				}
				Literal author = (Literal)e.Item.FindControl("Author");
				if(author != null)
				{
					author.Text = "by " + entry.Author;                    
				}
			}
		}
	}
}