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
            using(IDataReader reader = _procedures.GetBlogById(blogId))
            {
                if(reader.Read())
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
            using(IDataReader reader = _procedures.GetConfig(hostname, subfolder ?? string.Empty))
            {
                Blog info = null;
                while(reader.Read())
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
            using(IDataReader reader = _procedures.GetDomainAliasById(aliasId))
            {
                if(reader.Read())
                {
                    alias = reader.ReadObject<BlogAlias>();
                }
                reader.Close();
            }
            return alias;
        }

        public override Blog GetBlogByDomainAlias(string host, string subfolder, bool strict)
        {
            using(IDataReader reader = _procedures.GetBlogByDomainAlias(host, subfolder, strict))
            {
                if(reader.Read())
                {
                    return reader.ReadBlog();
                }
            }
            return null;
        }

        public override BlogStatistics GetBlogStatistics(int blogId)
        {
            BlogStatistics stats = null;
            using(IDataReader reader = _procedures.GetBlogStats(blogId))
            {
                if(reader.Read())
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
            using(IDataReader reader = _procedures.GetPagedBlogs(host, pageIndex, pageSize, flags))
            {
                return reader.ReadPagedCollection(r => r.ReadBlog());
            }
        }

        public override IPagedCollection<BlogAlias> GetPagedBlogDomainAlias(Blog blog, int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetPageableDomainAliases(pageIndex, pageSize, blog.Id))
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
        /// <param name="host"></param>
        /// <param name="subfolder"></param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        public override int CreateBlog(string title, string userName, string password, string host, string subfolder)
        {
            return CreateBlog(title, userName, password, host, subfolder, 1 /* blogGroupId */);
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
        public override int CreateBlog(string title, string userName, string password, string host, string subfolder, int blogGroupId)
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

            return _procedures.UTILITYAddBlog(title, userName, password, string.Empty, host, subfolder ?? string.Empty, (int)flag, blogGroupId);
        }

        public override bool UpdateBlog(Blog info)
        {
            int? daysTillCommentsClose = null;
            if(info.DaysTillCommentsClose > -1 && info.DaysTillCommentsClose < int.MaxValue)
            {
                daysTillCommentsClose = info.DaysTillCommentsClose;
            }

            int? commentDelayInMinutes = null;
            if(info.CommentDelayInMinutes > 0 && info.CommentDelayInMinutes < int.MaxValue)
            {
                commentDelayInMinutes = info.CommentDelayInMinutes;
            }

            int? numberOfRecentComments = null;
            if(info.NumberOfRecentComments > 0 && info.NumberOfRecentComments < int.MaxValue)
            {
                numberOfRecentComments = info.NumberOfRecentComments;
            }

            int? recentCommentsLength = null;
            if(info.RecentCommentsLength > 0 && info.RecentCommentsLength < int.MaxValue)
            {
                recentCommentsLength = info.RecentCommentsLength;
            }

            return _procedures.UpdateConfig(info.UserName,
                info.Password,
                info.Email,
                info.Title,
                info.SubTitle,
                info.Skin.TemplateFolder,
                info.Subfolder,
                info.Host,
                info.Author,
                info.Language,
                info.TimeZoneId,
                info.ItemCount,
                info.CategoryListPostCount,
                info.News.NullIfEmpty(),
                info.TrackingCode.NullIfEmpty(),
                info.LastUpdated /*null*/,
                info.Skin.CustomCssText.NullIfEmpty(),
                info.Skin.SkinStyleSheet.NullIfEmpty(),
                (int)info.Flag,
                info.Id,
                info.LicenseUrl,
                daysTillCommentsClose,
                commentDelayInMinutes,
                numberOfRecentComments,
                recentCommentsLength,
                info.FeedbackSpamServiceKey.NullIfEmpty(),
                info.RssProxyUrl.NullIfEmpty(),
                info.BlogGroupId,
                info.MobileSkin.TemplateFolder.NullIfEmpty(),
                info.MobileSkin.SkinStyleSheet.NullIfEmpty(),
                info.OpenIdUrl,
                info.CardSpaceHash,
                info.OpenIdServer,
                info.OpenIdDelegate);
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
            using(IDataReader reader = _procedures.GetBlogGroup(id, activeOnly))
            {
                if(!reader.Read())
                    return null;

                group = reader.ReadObject<BlogGroup>();
            }

            if(group != null)
            {
                //TODO: Make this more efficient.
                IPagedCollection<Blog> blogs =
                    Blog.GetBlogs(0, int.MaxValue, activeOnly ? ConfigurationFlags.IsActive : ConfigurationFlags.None);
                group.Blogs = new List<Blog>();
                foreach(Blog blog in blogs)
                {
                    if(blog.BlogGroupId == group.Id)
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
            using(IDataReader reader = _procedures.ListBlogGroups(activeOnly))
            {
                return reader.ReadCollection<BlogGroup>();
            }
        }

        public override ICollection<EntrySummary> GetTopEntrySummaries(int blogId, int rowCount)
        {
            using(IDataReader reader = _procedures.GetTopEntries(blogId, rowCount))
            {
                return reader.ReadCollection<EntrySummary>();
            }
        }

        public override ICollection<EntrySummary> GetRelatedEntries(int blogId, int entryId, int rowCount)
        {
            using(IDataReader reader = _procedures.GetRelatedEntries(blogId, entryId, rowCount))
            {
                return reader.ReadCollection<EntrySummary>();
            }
        }
    }
}
