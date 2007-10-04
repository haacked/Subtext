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
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Control used to view a single blog post.
	/// </summary>
    public class AggPinnedPost : BaseControl
	{
		protected Literal PinnedTitle;
        protected Literal PinnedPost;

        private int _ContentID;
        /// <summary>
        /// Property to set the ID of the post to display.
        /// </summary>
        public int ContentID
        {
            get { return _ContentID; }
            set { _ContentID = value; }
        }

        private string _EntryTitle = "";
        /// <summary>
        /// Property to override the post title to display.
        /// </summary>
        public string EntryTitle
        {
            get { return _EntryTitle; }
            set { _EntryTitle = value; }
        }

		/// <summary>
		/// Loads the entry specified by the EntryName property. 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			//Get the entry
			Entry entry = Entries.GetEntry(ContentID, false);
			if(entry != null)
			{
				PinnedPost.Text = entry.Body;
                PinnedTitle.Text = (string.IsNullOrEmpty(EntryTitle)) ? PinnedTitle.Text = entry.Title : EntryTitle;
			}
			else 
			{
				//No post? Deleted? Help :)
				this.Controls.Clear();
			}
		}		
	}
}