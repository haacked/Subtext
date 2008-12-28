using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for PreviousNext.
	/// </summary>
	public class PreviousNext : BaseControl
	{
		protected HyperLink NextLink;
		protected HyperLink PrevLink;
		protected HyperLink MainLink;
		protected Control LeftPipe;
		protected Control RightPipe;

	    protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			//Get the entry
			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);			
			
			//if found
			if(entry != null)
			{
				//Sent entry properties
				MainLink.NavigateUrl = CurrentBlog.HomeVirtualUrl;

				ICollection<Entry> entries = ObjectProvider.Instance().GetPreviousAndNextEntries(entry.Id, PostType.BlogPost);

				//Remember, the NEXT entry is the MORE RECENT entry.
				switch (entries.Count)
				{
					case 0:
					{
						//you have no entries. You should blog more
						PrevLink.Visible = false;
						NextLink.Visible = false;
						break;
					}
					case 1:
					{
						//since there is only one record, you are at an end
						//Check EntryID to see if it is greater or less than
						//the current ID
						if (entries.First().DateSyndicated > entry.DateSyndicated)
						{
							//this is the oldest blog
							PrevLink.Visible = false;
							LeftPipe.Visible = false;							
							SetNav(NextLink, entries.First());
						}
						else
						{
							//this is the latest blog
							NextLink.Visible = false;
							RightPipe.Visible = false;
							SetNav(PrevLink, entries.First());
						}
						break;
					}
					case 2:
					{
						//two records found. The first record will be NEXT
						//the second record will be PREVIOUS
						//This is because the query is sorted by EntryID
						SetNav(NextLink, entries.First());
						SetNav(PrevLink, entries.ElementAt(1));
						break;
					}
				}
			}
			else 
			{
				//No post? Deleted? Help :)
				this.Controls.Clear();
				this.Controls.Add(new LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
			}
		}


		private void SetNav(HyperLink navLink, Entry entry)
		{
			string format = navLink.Attributes["Format"];
			if(String.IsNullOrEmpty(format))
			{
			    format = "{0}";
			}
			
			navLink.Attributes.Remove("Format");

			string entryTitle = HttpUtility.HtmlDecode(entry.Title);
			string sizeLimitText = navLink.Attributes["TextSizeLimit"];
			if (!String.IsNullOrEmpty(sizeLimitText))
			{
				int sizeLimit;
				if(int.TryParse(sizeLimitText, out sizeLimit))
				{
					if (sizeLimit > 0 && sizeLimit < entryTitle.Length)
					{
						entryTitle = entryTitle.Substring(0, sizeLimit) + "...";
					}
				}
			}
			navLink.Attributes.Remove("TextSizeLimit");

            navLink.Text = HttpUtility.HtmlEncode(string.Format(format, entryTitle));
			navLink.NavigateUrl = Url.EntryUrl(entry);
		}
	}
}
