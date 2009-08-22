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

        public int? BlogGroupId
        {
            get;
            private set;
        }

        public bool ShowGroups
        {
            get;
            private set;
        }

        public IEnumerable<Blog> Blogs
        {
            get;
            private set;
        }

        public IEnumerable<BlogGroup> BlogGroups
        {
            get;
            private set;
        }

        protected override void OnLoad(EventArgs e)
        {
            BlogGroupId = GetGroupIdFromQueryString();
            Blogs = Repository.GetBlogsByGroup(HostInfo.Instance.AggregateBlog.Host, BlogGroupId);

            if (ShowGroups)
            {
                BlogGroups = Repository.GroupBlogs(Blogs);
            }
            else
            {
                var groups = new Collection<BlogGroup>();
                groups.Add(new BlogGroup { Blogs = Blogs.ToList() });
                BlogGroups = groups;
            }
            base.OnLoad(e);
        }
    }
}
