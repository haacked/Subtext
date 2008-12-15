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
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Controls;
using System.Web;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Displays the most recent comments on the skin.
	/// </summary>
	public class RecentComments : BaseControl
	{
		private const int DefaultRecentPostCount = 5;
		protected Repeater feedList;
        private ICollection<FeedbackItem> comments;

		/// <summary>
		/// Initializes a new instance of the <see cref="RecentComments"/> class.
		/// </summary>
		public RecentComments()
		{
            int commentCount = Config.CurrentBlog.NumberOfRecentComments > 0 ? Config.CurrentBlog.NumberOfRecentComments : DefaultRecentPostCount;
			
		    comments = FeedbackItem.GetRecentComments(commentCount);

            comments = (from c in comments where c.EntryId > 0 select c).ToList();
		}

		/// <summary>
		/// Binds the comments <see cref="List{T}"/> to the comment list repeater.
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
				FeedbackItem comment = (FeedbackItem)e.Item.DataItem;
				
				HyperLink title = (HyperLink)e.Item.FindControl("Link");
				if(title != null)
				{
					int commentLength = CurrentBlog.RecentCommentsLength;
					if (comment.Body.Length > commentLength) 
					{
						string truncatedText = string.Empty;
						if (commentLength > 0)
						{
                            //the html encode is for extra safety.
							truncatedText = HttpUtility.HtmlEncode(HtmlHelper.RemoveHtml(comment.Body));
							if (truncatedText.Length > commentLength)
							{
								truncatedText = truncatedText.Substring(0, commentLength);
							}
						}

						title.Text = truncatedText + "...";
						title.NavigateUrl = comment.DisplayUrl.ToString();
					} 
					else
					{
                        //the html encode is for extra safety.
						title.Text = HttpUtility.HtmlEncode(HtmlHelper.RemoveHtml(comment.Body));
						title.NavigateUrl = comment.DisplayUrl.ToString();
					}
					ControlHelper.SetTitleIfNone(title, "Reader Comment.");
				}
				Literal author = (Literal)e.Item.FindControl("Author");
				if(author != null)
				{
					author.Text = "by " + comment.Author;                    
				}
			}
		}
	}
}