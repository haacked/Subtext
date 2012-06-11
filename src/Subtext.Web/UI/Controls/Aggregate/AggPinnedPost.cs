#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Control used to view a single blog post.
    /// </summary>
    public class AggPinnedPost : AggregateUserControl
    {
        protected Literal PinnedPost;
        protected Literal PinnedTitle;

        /// <summary>
        /// Property to set the ID of the post to display.
        /// </summary>
        public int ContentID { get; set; }

        /// <summary>
        /// Property to override the post title to display.
        /// </summary>
        public string EntryTitle { get; set; }

        /// <summary>
        /// Loads the entry specified by the EntryName property. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Get the entry
            Entry entry = Repository.GetEntry(ContentID, true /*activeOnly*/, false /*includeCategories*/);
            if (entry != null)
            {
                PinnedPost.Text = entry.Body;
                PinnedTitle.Text = (string.IsNullOrEmpty(EntryTitle)) ? PinnedTitle.Text = entry.Title : EntryTitle;
            }
            else
            {
                //No post? Deleted? Help :)
                Controls.Clear();
            }
        }
    }
}