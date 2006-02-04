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

namespace Subtext.Web.UI.Controls
{
	/// 
	/// RecentComments displays the last 5 comments
	/// ToDOo: make this number configurable
	/// 
	public class RecentComments : BaseControl
	{
		protected System.Web.UI.WebControls.Repeater feedList;
		private EntryCollection comments;

		public RecentComments()
		{
			// comments = Entries.GetRecentPosts(5, Subtext.Extensibility.PostType.Comment, true);
			if(Config.CurrentBlog.NumberOfRecentComments > 0)
			{
				comments = Entries.GetRecentPosts(Config.CurrentBlog.NumberOfRecentComments, PostType.Comment, true);
			}
			else
			{
				/*AppSettingsReader settingreader = new AppSettingsReader();
				int NumberOfRecentComments = (int) settingreader.GetValue("NumberOfRecentComments", typeof(int));
				
				comments = Entries.GetRecentPosts(NumberOfRecentComments, PostType.Comment, true);*/
				comments = Entries.GetRecentPosts(5, Subtext.Extensibility.PostType.Comment, true);
			}

		}

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

					if (entry.Body.Length > 50) 
					{
						// title.Text = entry.Body.Substring(0, 50).ToString() + "...";
						if(Config.CurrentBlog.RecentCommentsLength > 0)
						{
							title.Text = entry.Body.Substring(0, (Config.CurrentBlog.RecentCommentsLength)).ToString() + "...";
						}
						else
						{
							/*AppSettingsReader settingreader = new AppSettingsReader(); 
							int RecentCommentsLength = (int) settingreader.GetValue("RecentCommentsLength", typeof(int)); 

							title.Text = entry.Body.Substring(0, (RecentCommentsLength)).ToString() + "..."; 
							title.NavigateUrl = entry.Link;*/
							title.Text = entry.Body.Substring(0, 50).ToString() + "...";
						}
						title.NavigateUrl = entry.Link;

					} 
					else
					{
						title.Text = entry.Body;
						title.NavigateUrl = entry.Link;
					}
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