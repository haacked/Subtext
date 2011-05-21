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
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Implements a search control that can be added to a skin.
    /// </summary>
    public class SubtextSearch : BaseControl
    {
        protected TextBox txtSearch;
        protected Button btnSearch;

        protected override void OnInit(EventArgs e)
        {
            if (btnSearch != null)
            {
                btnSearch.Click += OnSearchClick;
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryString = SubtextContext.RequestContext.GetQueryFromRequest();
                txtSearch.Text = queryString;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSearch.Text))
                Response.Redirect(Url.SearchPageUrl(txtSearch.Text), true);
        }

        protected void OnSearchClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSearch.Text))
                Response.Redirect(Url.SearchPageUrl(txtSearch.Text), true);

        }

    }
}