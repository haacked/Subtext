using System;
using System.Collections.Generic;
using System.Data;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        /// <summary>
        /// Gets the blog by id.
        /// </summary>
        /// <param name="blogId">Blog id.</param>
        /// <returns></returns>
        public override Blog GetBlogById(int blogId)
        {
            using (IDataReader reader = _procedures.GetBlogById(blogId))
            {
                if (reader.Read())
                {
                    Blog info = reader.ReadBlog();
                    return info;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a <see cref="Blog"/> instance containing 
        /// the configuration settings for the blog specified by the 
        /// Hostname and Application.
        /// </summary>
        /// <remarks>
        /// Until Subtext supports multiple blogs again (if ever), 
        /// this will always return the same instance.
        /// </remarks>
        /// <param name="hostname">Hostname.</param>
        /// <param name="subfolder">Subfolder.</param>
        /// <returns></returns>
        public override Blog GetBlog(string hostname, string subfolder)
        {
            using (IDataReader reader = _procedures.GetConfig(hostname, subfolder ?? string.Empty))
            {
                Blog info = null;
                while (reader.Read())
                {
                    info = reader.ReadBlog();
                    break;
                }
                return info;
            }
        }

        public override BlogAlias GetBlogAliasById(int aliasId)
        {
            BlogAlias alias = null;
            using (IDataReader reader = _procedures.GetDomainAliasById(aliasId))
            {
                if (reader.Read())
                {
                    alias = reader.ReadObject<BlogAlias>();
                }
                reader.Close();
            }
            return alias;
        }

        public override Blog GetBlogByDomainAlias(string host, string subfolder, bool strict)
        {
            using (IDataReader reader = _procedures.GetBlogByDomainAlias(host, subfolder, strict))
            {
                if (reader.Read())
                {
                    return reader.ReadBlog();
                }
            }
            return null;
        }

        public override BlogStatistics GetBlogStatistics(int blogId)
        {
            BlogStatistics stats = null;
            using (IDataReader reader = _procedures.GetBlogStats(blogId))
            {
                if (reader.Read())
                {
                    stats = reader.ReadObject<BlogStatistics>();
                }
            }
            return stats;
        }

        /// <summary>
        /// Gets a pageable Collection of <see cref="Blog"/> instances.
        /// </summary>
        /// <param name="host">The host filter. Set to null to return all blogs.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        /// <param name="flags"></param>
        public override IPagedCollection<Blog> GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlags flags)
        {
            using (IDataReader reader = _procedures.GetPagedBlogs(host, pageIndex, pageSize, flags))
            {
                return reader.ReadPagedCollection(r => r.ReadBlog());
            }
        }

        public override IPagedCollection<BlogAlias> GetPagedBlogDomainAlias(Blog blog, int pageIndex, int pageSize)
        {
            using (IDataReader reader = _procedures.GetPageableDomainAliases(pageIndex, pageSize, blog.Id))
            {
                return reader.ReadPagedCollection(r => r.ReadObject<BlogAlias>());
            }
        }

        /// <summary>
        /// Adds the initial blog configuration.  This is a convenience method for
        /// allowing a user with a freshly installed blog to immediately gain access
        /// to the admin section to edit the blog.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password.</param>
        /// <param name="host"></param>
        /// <param name="subfolder"></param>
        /// <param name="blogGroupId"></param>
        /// <returns></returns>
        public override int CreateBlogInternal(string title, string userName, string password, string host, string subfolder, int blogGroupId)
        {
            const ConfigurationFlags flag = ConfigurationFlags.IsActive
                                            | ConfigurationFlags.CommentsEnabled
                                            | ConfigurationFlags.CompressSyndicatedFeed
                                            | ConfigurationFlags.IsAggregated
                                            | ConfigurationFlags.IsPasswordHashed
                                            | ConfigurationFlags.AutoFriendlyUrlEnabled
                                            | ConfigurationFlags.CommentNotificationEnabled
                                            | ConfigurationFlags.RFC3229DeltaEncodingEnabled
                                            | ConfigurationFlags.CaptchaEnabled;

            return _procedures.UTILITYAddBlog(title, userName, password, string.Empty, host, subfolder ?? string.Empty, (int)flag, blogGroupId, CurrentDateTimeUtc);
        }

        public override bool UpdateBlog(Blog blog)
        {
            if (blog == null)
            {
                throw new ArgumentNullException("blog");
            }

            if (!blog.DateModifiedUtc.IsNull() && blog.DateModifiedUtc.Kind != DateTimeKind.Utc)
            {
                throw new InvalidOperationException("DateModified must be UTC");
            }

            int? daysTillCommentsClose = null;
            if (blog.DaysTillCommentsClose > -1 && blog.DaysTillCommentsClose < int.MaxValue)
            {
                daysTillCommentsClose = blog.DaysTillCommentsClose;
            }

            int? commentDelayInMinutes = null;
            if (blog.CommentDelayInMinutes > 0 && blog.CommentDelayInMinutes < int.MaxValue)
            {
                commentDelayInMinutes = blog.CommentDelayInMinutes;
            }

            int? numberOfRecentComments = null;
            if (blog.NumberOfRecentComments > 0 && blog.NumberOfRecentComments < int.MaxValue)
            {
                numberOfRecentComments = blog.NumberOfRecentComments;
            }

            int? recentCommentsLength = null;
            if (blog.RecentCommentsLength > 0 && blog.RecentCommentsLength < int.MaxValue)
            {
                recentCommentsLength = blog.RecentCommentsLength;
            }

            return _procedures.UpdateConfig(blog.UserName,
                blog.Password,
                blog.Email,
                blog.Title,
                blog.SubTitle,
                blog.Skin.TemplateFolder,
                blog.Subfolder,
                blog.Host,
                blog.Author,
                blog.Language,
                blog.TimeZoneId,
                blog.TimeZone.Offset,
                blog.ItemCount,
                blog.CategoryListPostCount,
                blog.News.NullIfEmpty(),
                blog.TrackingCode.NullIfEmpty(),
                blog.DateModifiedUtc,
                blog.Skin.CustomCssText.NullIfEmpty(),
                blog.Skin.SkinStyleSheet.NullIfEmpty(),
                (int)blog.Flag,
                blog.Id,
                blog.LicenseUrl,
                daysTillCommentsClose,
                commentDelayInMinutes,
                numberOfRecentComments,
                recentCommentsLength,
                blog.FeedbackSpamServiceKey.NullIfEmpty(),
                blog.RssProxyUrl.NullIfEmpty(),
                blog.BlogGroupId,
                blog.MobileSkin.TemplateFolder.NullIfEmpty(),
                blog.MobileSkin.SkinStyleSheet.NullIfEmpty(),
                blog.OpenIdUrl,
                blog.CardSpaceHash,
                blog.OpenIdServer,
                blog.OpenIdDelegate);
        }

        public override bool CreateBlogAlias(BlogAlias alias)
        {
            int aliasId = _procedures.CreateDomainAlias(alias.BlogId, alias.Host, alias.Subfolder, alias.IsActive);
            alias.Id = aliasId;
            return true;
        }

        public override bool UpdateBlogAlias(BlogAlias alias)
        {
            return _procedures.UpdateDomainAlias(alias.Id, alias.BlogId, alias.Host, alias.Subfolder, alias.IsActive);
        }

        public override bool DeleteBlogAlias(BlogAlias alias)
        {
            return _procedures.DeleteDomainAlias(alias.Id);
        }

        /// <summary>
        /// Gets the blog group.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override BlogGroup GetBlogGroup(int id, bool activeOnly)
        {
            BlogGroup group;
            using (IDataReader reader = _procedures.GetBlogGroup(id, activeOnly))
            {
                if (!reader.Read())
                    return null;

                group = reader.ReadObject<BlogGroup>();
            }

            if (group != null)
            {
                //TODO: Make this more efficient.
                IPagedCollection<Blog> blogs =
                    this.GetBlogs(0, int.MaxValue, activeOnly ? ConfigurationFlags.IsActive : ConfigurationFlags.None);
                group.Blogs = new List<Blog>();
                foreach (Blog blog in blogs)
                {
                    if (blog.BlogGroupId == group.Id)
                        group.Blogs.Add(blog);
                }
            }
            return group;
        }

        /// <summary>
        /// Lists the blog groups.
        /// </summary>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override ICollection<BlogGroup> ListBlogGroups(bool activeOnly)
        {
            using (IDataReader reader = _procedures.ListBlogGroups(activeOnly))
            {
                return reader.ReadCollection<BlogGroup>();
            }
        }

        public override ICollection<EntrySummary> GetTopEntrySummaries(int blogId, int rowCount)
        {
            using (IDataReader reader = _procedures.GetTopEntries(blogId, rowCount))
            {
                return reader.ReadCollection<EntrySummary>();
            }
        }

        public override ICollection<EntrySummary> GetRelatedEntries(int blogId, int entryId, int rowCount)
        {
            using (IDataReader reader = _procedures.GetRelatedEntries(blogId, entryId, rowCount))
            {
                return reader.ReadCollection<EntrySummary>();
            }
        }
    }
}
