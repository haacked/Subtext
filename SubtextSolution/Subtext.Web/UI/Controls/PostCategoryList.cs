using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

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

namespace Subtext.Web.UI.Controls
{
    /// <summary>
	///		Summary description for PostCategoryList.
	/// </summary>
	public class PostCategoryList : BaseControl
	{
		protected Repeater CatList;

        private ICollection<LinkCategory> lcc;
        public ICollection<LinkCategory> LinkCategories
		{
			get{return lcc;}
			set{lcc = value;}
		}

        private bool _showEmpty = false;
        public bool ShowEmpty
        {
            get { return _showEmpty; }
            set { _showEmpty = value; }
        }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(LinkCategories != null)
			{
				CatList.DataSource = LinkCategories;
				CatList.DataBind();
			}
			else
			{
				Controls.Clear();
				Visible = false;
			}
		}

        protected override void OnPreRender(EventArgs e)
        {
            if (CatList.Items.Count == 0)
                Visible = _showEmpty;
            base.OnPreRender(e);
        }

		protected void CategoryCreated(object sender, RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkCategory linkcat = (LinkCategory)e.Item.DataItem;
				if(linkcat != null)
				{
					HyperLink Link = (HyperLink)e.Item.FindControl("Link");
                    Link.NavigateUrl = CurrentBlog.UrlFormats.PostCategoryUrl(linkcat.Title, linkcat.Id);
					if(Link.Attributes["title"] == null || Link.Attributes["title"].Length == 0)
					{
						Link.Attributes["title"] = "";
					}
					Link.Text = linkcat.Title;
				}
			}
		}
	}
}

