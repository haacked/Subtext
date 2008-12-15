using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Text;
using Subtext.Framework.Components;
using Subtext.Extensibility;

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
            using (IDataReader reader = _procedures.GetHost())
            {
                if (reader.Read())
                {
                    DataHelper.LoadHost(reader, hostInfo);
                    reader.Close();
                    return hostInfo;
                }
                reader.Close();
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

        public override ICollection<BlogInfo> GetBlogsByGroup(string host, int? groupId)
        {
            List<BlogInfo> blogs = new List<BlogInfo>();
            using (var reader = _procedures.Stats(host, groupId)) {
                while (reader.Read())
                {
                    blogs.Add(DataHelper.LoadBlogInfo(reader));
                }
            }
            return blogs;
        }

        /// <summary>
        /// Given a list of blogs, groups blogs
        /// </summary>
        /// <param name="blogs"></param>
        /// <returns></returns>
        public override ICollection<BlogGroup> GroupBlogs(IEnumerable<BlogInfo> blogs)
        {
            return blogs.GroupBy(blog => blog.BlogGroupId,
                (blogGroupId, blogsInGroup) => new BlogGroup
                {
                    Blogs = blogsInGroup.ToList(),
                    Title = blogs.First(b => b.BlogGroupId == blogGroupId).BlogGroupTitle
                })
                .ToList();
        }

        public override HostStats GetTotalBlogStats(string host, int groupId) {
            using (IDataReader reader = _procedures.TotalStats(host, groupId)) {
                if (!reader.Read()) {
                    return null;
                }
                return reader.LoadObject<HostStats>();
            }
        }

        public override ICollection<Entry> GetRecentEntries(string host, int? groupId, int rowCount) { 
            var entries = new List<Entry>();
            using(IDataReader reader = _procedures.GetRecentPosts(host, groupId, CurrentDateTime, rowCount)) {
                while(reader.Read()) {
                    entries.Add(reader.LoadEntry(new Entry(PostType.BlogPost), false /* buildLinks */, true /* includeBlog */));
                }
            }
            return entries;
        }

        public override ICollection<Image> GetImages(string host, int? groupId, int rowCount)
        {
            var images = new List<Image>();
            using (IDataReader reader = _procedures.GetRecentImages(host, groupId, rowCount))
            {
                while (reader.Read())
                {
                    images.Add(reader.LoadImage(true /* includeBlog */, true/* includeCategory */));
                }
            }
            return images;
        }

        
    }
}
