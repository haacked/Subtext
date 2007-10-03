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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Format;
using Subtext.Framework.Security;
using Subtext.Framework.Tracking;
using Subtext.Web.Controls;
using Subtext.Extensibility.Plugins;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Control used to view a single blog post.
	/// </summary>
	public class ViewPost : BaseControl
	{
		protected HyperLink editLink;
		protected HyperLink TitleUrl;
		protected Label date;
		protected Label commentCount;
		protected Literal Body;
		protected Literal PostDescription;
		protected PostCategoryList Categories;
		protected Literal PingBack;
		protected Literal TrackBack;

		const string linkToComments = "<a href=\"{0}#feedback\" title=\"View and Add Comments\" class=\"comments\">{1}{2}</a>";

		/// <summary>
		/// Loads the entry specified by the URL.  If the user is an 
		/// admin and the skin supports it, will also display an edit 
		/// link that navigates to the admin section and allows the 
		/// admin to edit the post.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			//Get the entry
			this.entry = Cacher.GetEntryFromRequest(CacheDuration.Short);			
			
			//if found
			if(entry != null)
			{
				//Raise event before any processing takes place
				SubtextEvents.OnSingleEntryRendering(this, new EntryEventArgs(entry));

				BindCurrentEntryControls(entry, this);
				
				DisplayEditLink();

				//Track this entry
				EntryTracker.Track(Context, entry.Id, CurrentBlog.Id);

				//Set the page title
                Globals.SetTitle(HttpUtility.HtmlEncode(entry.Title), Context);

				//Sent entry properties
                TitleUrl.Text = HttpUtility.HtmlEncode(entry.Title);
				ControlHelper.SetTitleIfNone(TitleUrl, "Title of this entry.");
				TitleUrl.NavigateUrl = entry.Url;
				Body.Text = entry.Body;

				if(PostDescription != null)
				{
					PostDescription.Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}",entry.DateSyndicated.ToLongDateString(),entry.DateSyndicated.ToShortTimeString());
				}
                Trace.Write("loading categories");
				if(Categories != null)
				{
                    Categories.LinkCategories = Links.GetLinkCategoriesByPostId(entry.Id);
                    Categories.DataBind();
				}

				if(date != null)
				{
					if(date.Attributes["Format"] != null)
					{
						date.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Url, entry.DateSyndicated.ToString(date.Attributes["Format"]));
						date.Attributes.Remove("Format");
					}
					else
					{
						date.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Url, entry.DateSyndicated.ToString("f"));
					}
				}

				if(commentCount != null)
				{
					if(CurrentBlog.CommentsEnabled && entry.AllowComments)
					{
						if(entry.FeedBackCount == 0)
						{
							commentCount.Text = string.Format(linkToComments, entry.Url, "Add Comment", "");
						}
						else if(entry.FeedBackCount == 1)
						{
							commentCount.Text = string.Format(linkToComments, entry.Url, "One Comment", "");
						}
						else if(entry.FeedBackCount > 1)
						{
							commentCount.Text = string.Format(linkToComments, entry.Url, entry.FeedBackCount, " Comments");
						}
					}
				}
				
				//Set Pingback/Trackback 
				if(PingBack == null)
				{
					PingBack = Page.FindControl("pinbackLinkTag") as Literal;
				}
				
				if(PingBack != null)
				{
					PingBack.Text = TrackHelpers.PingPackTag;
				}
				
				if(TrackBack != null)
				{
					TrackBack.Text = TrackHelpers.TrackBackTag(entry);
				}

				SubtextEvents.OnSingleEntryRendered(this, new EntryEventArgs(entry));

			}
			else 
			{
				//No post? Deleted? Help :)
				this.Controls.Clear();
				this.Controls.Add(new LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
			}
			this.DataBind();
		}

		public Entry Entry
		{
			get { return this.entry; }
		}

		private Entry entry;

		// If the user is an admin AND the the skin 
		// contains an edit Hyperlink control, this 
		// will display the edit control.
		private void DisplayEditLink()
		{
			if(editLink != null)
			{
				if (SecurityHelper.IsInRole(RoleNames.Authors) || SecurityHelper.IsInRole(RoleNames.Administrators))
				{
					editLink.Visible = true;
					editLink.NavigateUrl = UrlFormats.GetEditLink(entry);
					ControlHelper.SetTitleIfNone(editLink, "Edit this entry.");

					if(String.IsNullOrEmpty(editLink.Text) && String.IsNullOrEmpty(editLink.ImageUrl))
					{
						//We'll slap on our little pencil icon.
						editLink.ImageUrl = BlogInfo.VirtualDirectoryRoot + "Images/edit.gif";
					}
				}
				else
				{
					editLink.Visible = false;
				}
			}
		}
	}
}



