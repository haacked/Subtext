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
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    public class AggBloggers : AggregateUserControl
    {
        protected Repeater Bloggers;

        public int? BlogGroupId { get; private set; }

        public bool ShowGroups { get; private set; }

        public IEnumerable<Blog> Blogs { get; private set; }

        public IEnumerable<BlogGroup> BlogGroups { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            BlogGroupId = GetGroupIdFromQueryString();
            Blogs = Repository.GetBlogsByGroup(HostInfo.AggregateBlog.Host, BlogGroupId);

            if (ShowGroups)
            {
                BlogGroups = Repository.GroupBlogs(Blogs);
            }
            else
            {
                var groups = new Collection<BlogGroup> { new BlogGroup { Blogs = Blogs.ToList() } };
                BlogGroups = groups;
            }
            base.OnLoad(e);
        }
    }
}