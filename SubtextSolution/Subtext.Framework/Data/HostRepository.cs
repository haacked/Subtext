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
using System.Data;
using System.Linq;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Text;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        /// <summary>
        /// Returns the <see cref="HostInfo"/> for the Subtext installation.
        /// </summary>
        /// <returns>A <see cref="HostInfo"/> instance.</returns>
        public override HostInfo LoadHostInfo(HostInfo hostInfo)
        {
            try
            {
                using (IDataReader reader = _procedures.GetHost())
                {
                    if (reader.Read())
                    {
                        return reader.ReadObject(hostInfo);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                //When upgrading, this may occur because the old version of the 
                //database schema doesn't know about new properties.
                return new HostInfo();
            }
            return null;
        }

        /// <summary>
        /// Updates the <see cref="HostInfo"/> instance.  If the host record is not in the 
        /// database, one is created. There should only be one host record.
        /// </summary>
        /// <param name="host">The host information.</param>
        public override bool UpdateHost(HostInfo host)
        {
            return _procedures.UpdateHost(host.HostUserName, host.Password, host.Salt);
        }

        /// <summary>
        /// Clears all content (Entries, Comments, Track/Ping-backs, Statistices, etc...) 
        /// for a the current blog (sans the Image Galleries).
        /// </summary>
        /// <returns>
        ///     TRUE - At least one unit of content was cleared.
        ///     FALSE - No content was cleared.
        /// </returns>
        public override void ClearBlogContent(int blogId)
        {
            _procedures.ClearBlogContent(blogId);
        }

        /// <summary>
        /// Inserts the blog group.
        /// </summary>
        /// <param name="blogGroup">The group to insert.</param>
        /// <returns>The blog group id</returns>
        public override int InsertBlogGroup(BlogGroup blogGroup)
        {
            return _procedures.InsertBlogGroup(blogGroup.Title,
                blogGroup.IsActive,
                blogGroup.DisplayOrder.NullIfMinValue(),
                blogGroup.Description.NullIfEmpty());
        }

        /// <summary>
        /// Inserts the blog group.
        /// </summary>
        /// <param name="blogGroup">The group to insert.</param>
        /// <returns>The blog group id</returns>
        public override bool UpdateBlogGroup(BlogGroup blogGroup)
        {
            return _procedures.UpdateBlogGroup(blogGroup.Id,
                blogGroup.Title,
                blogGroup.IsActive,
                blogGroup.Description.NullIfEmpty(),
                blogGroup.DisplayOrder.NullIfMinValue());
        }

        public override bool DeleteBlogGroup(int blogGroupId)
        {
            return _procedures.DeleteBlogGroup(blogGroupId);
        }

        public override ICollection<Blog> GetBlogsByGroup(string host, int? groupId)
        {
            using (var reader = _procedures.Stats(host, groupId))
            {
                return reader.ReadCollection(r => r.ReadBlog());
            }
        }

        /// <summary>
        /// Given a list of blogs, groups blogs
        /// </summary>
        /// <param name="blogs"></param>
        /// <returns></returns>
        public override ICollection<BlogGroup> GroupBlogs(IEnumerable<Blog> blogs)
        {
            return blogs.GroupBy(blog => blog.BlogGroupId,
                (blogGroupId, blogsInGroup) => new BlogGroup
                {
                    Blogs = blogsInGroup.ToList(),
                    Title = blogs.First(b => b.BlogGroupId == blogGroupId).BlogGroupTitle
                })
                .ToList();
        }

        public override HostStats GetTotalBlogStats(string host, int? groupId)
        {
            using (IDataReader reader = _procedures.TotalStats(host, groupId))
            {
                if (!reader.Read())
                {
                    return null;
                }
                return reader.ReadObject<HostStats>();
            }
        }

        public override ICollection<Entry> GetRecentEntries(string host, int? groupId, int rowCount)
        {
            using (IDataReader reader = _procedures.GetRecentPosts(host, groupId, CurrentDateTime, rowCount))
            {
                return reader.ReadCollection(r => r.ReadEntry(new Entry(PostType.BlogPost), false /* buildLinks */, true /* includeBlog */));
            }
        }

        public override ICollection<Image> GetImages(string host, int? groupId, int rowCount)
        {
            using (IDataReader reader = _procedures.GetRecentImages(host, groupId, rowCount))
            {
                return reader.ReadCollection(r => r.ReadImage(true /* includeBlog */, true/* includeCategory */));
            }
        }
    }
}
