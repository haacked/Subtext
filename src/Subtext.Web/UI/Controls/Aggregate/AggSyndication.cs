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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    public class AggSyndication : AggregateUserControl
    {
        protected Repeater blogGroupRepeater;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ICollection<BlogGroup> groups = Repository.ListBlogGroups(true);
            blogGroupRepeater.DataSource = groups;
            blogGroupRepeater.DataBind();
        }
    }
}