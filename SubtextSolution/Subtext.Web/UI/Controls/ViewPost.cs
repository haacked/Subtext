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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Format;
using Subtext.Framework.Security;
using Subtext.Framework.Tracking;
using Subtext.Web.Controls;

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
        protected Label Enclosure;

		const string linkToComments = "<a href=\"{0}#feedback\" title=\"View and Add Comments\">{1}{2}</a>";
        const string linkToEnclosure = "<a href=\"{0}\" title = \"{1}\">{2}</a>{3}";

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
			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);			
			
			//if found
			if(entry != null)
			{
				BindCurrentEntryControls(entry, this);
				
				DisplayEditLink(entry);

				//Track this entry
				EntryTracker.Track(Context, entry.Id, CurrentBlog.Id);

				//Set the page title
				Globals.SetTitle(entry.Title, Context);

				//Sent entry properties
				TitleUrl.Text = entry.Title;
				ControlHelper.SetTitleIfNone(TitleUrl, "Title of this entry.");
				TitleUrl.NavigateUrl = entry.Url;
				Body.Text = entry.Body;
				if(PostDescription != null)
				{
					PostDescription.Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}",entry.DateCreated.ToLongDateString(),entry.DateCreated.ToShortTimeString());
				}
                Trace.Write("loading categories");
				if(Categories != null)
				{
                    Categories.LinkCategories = Links.GetLinkCategoriesByPostID(entry.Id);
                    Categories.DataBind();
				}

				if(date != null)
				{
					if(date.Attributes["Format"] != null)
					{
						date.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Url, entry.DateCreated.ToString(date.Attributes["Format"]));
						date.Attributes.Remove("Format");
					}
					else
					{
						date.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Url, entry.DateCreated.ToString("f"));
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
				
                BindEnclosure(entry);

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
			}
			else 
			{
				//No post? Deleted? Help :)
				this.Controls.Clear();
				this.Controls.Add(new LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
			}
		}

        private void BindEnclosure(Entry entry)
        {
            if (Enclosure != null)
            {

                if (entry.Enclosure != null && entry.Enclosure.ShowWithPost)
                {
                    bool displaySize = false;
                    Boolean.TryParse(Enclosure.Attributes["DisplaySize"], out displaySize);

                    string sizeStr = "";
                    if (displaySize)
                        sizeStr = " (" + entry.Enclosure.FormattedSize + ")";
                    Enclosure.Text = string.Format(linkToEnclosure, entry.Enclosure.Url, entry.Enclosure.Title, entry.Enclosure.Title, sizeStr);
                }
            }
        }

		// If the user is an admin AND the the skin 
		// contains an edit Hyperlink control, this 
		// will display the edit control.
		private void DisplayEditLink(Entry entry)
		{
			if(editLink != null)
			{
				if(SecurityHelper.IsAdmin)
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



