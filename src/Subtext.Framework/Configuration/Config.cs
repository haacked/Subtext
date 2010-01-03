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
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;
using Subtext.Framework.Web;
using Subtext.Framework.Web.HttpModules;
using Subtext.Scripting;

namespace Subtext.Framework.Configuration
{
    /// <summary>
    /// Static helper class used to access various configuration 
    /// settings.
    /// </summary>
    public static class Config
    {
        private static readonly string[] InvalidSubfolders = {
                                                                  "Tags", "Admin", "aspx", "bin", "ExternalDependencies",
                                                                  "HostAdmin", "Images", "Install", "Properties",
                                                                  "Providers", "Pages", "Scripts", "Skins", 
                                                                  "SystemMessages", "UI", "Modules", "Services", 
                                                                  "Category", "Archive", "Archives", "Comments",
                                                                  "Articles", "Posts", "Story", "Stories", "Gallery",
                                                                  "aggbug", "Sitemap", "Account"
                                                              };

        static ConnectionString _connectionString;

        /// <summary>
        /// Returns an instance of <see cref="BlogConfigurationSettings"/> which 
        /// are configured within web.config as a custom config section.
        /// </summary>
        /// <value></value>
        public static BlogConfigurationSettings Settings
        {
            get { return ((BlogConfigurationSettings)ConfigurationManager.GetSection("BlogConfigurationSettings")); }
        }

        /// <summary>
        /// Returns the Subtext connection string.
        /// </summary>
        /// <remarks>
        /// The connectionStrings section may contain multiple connection strings. 
        /// The AppSetting "connectionStringName" points to which of those strings 
        /// is the one in use.
        /// </remarks>
        public static ConnectionString ConnectionString
        {
            get
            {
                if(_connectionString == null)
                {
                    string connectionStringName = ConfigurationManager.AppSettings["connectionStringName"];
                    if(ConfigurationManager.ConnectionStrings[connectionStringName] == null)
                    {
                        throw new ConfigurationErrorsException(String.Format(CultureInfo.InvariantCulture,
                                                                             Resources.
                                                                                 ConfigurationErrros_NoConnectionString,
                                                                             connectionStringName));
                    }
                    string connectionStringText =
                        ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                    _connectionString = ConnectionString.Parse(connectionStringText);
                }
                return _connectionString;
            }
        }

        /// <summary>
        /// Returns a <see cref="Blog"/> instance containing 
        /// the configuration settings for the current blog.
        /// </summary>
        /// <remarks>
        ///	<para>This property may throw an exception in a couple of cases. The reason for 
        /// this is that there are a couple different reasons why the Current Blog might 
        /// not exist and we handle those situations differently in the UI. Returning 
        /// NULL does not give us enough information.
        /// </para>
        /// </remarks>
        /// <exception type="BlogDoesNotExistException">Thrown if the blog does not exist</exception>
        /// <exception type="BlogInactiveException">Thrown if the blog is no longer active</exception>
        /// <returns>The current blog</returns>
        public static Blog CurrentBlog
        {
            get
            {
                if(HttpContext.Current == null)
                {
                    return null;
                }
                BlogRequest blogRequest = BlogRequest.Current;
                if(blogRequest == null || blogRequest.IsHostAdminRequest)
                {
                    return null;
                }

                Blog currentBlog = BlogRequest.Current.Blog;
                return currentBlog;
            }
        }

        /// <summary>
        /// Gets the count of active blogs.
        /// </summary>
        /// <value></value>
        public static int ActiveBlogCount
        {
            get
            {
                IPagedCollection<Blog> blogs = Blog.GetBlogs(1, 1, ConfigurationFlags.IsActive);
                return blogs.MaxItems;
            }
        }

        /// <summary>
        /// Gets the total blog count in the system, active or not.
        /// </summary>
        /// <value></value>
        public static int BlogCount
        {
            get
            {
                IPagedCollection<Blog> blogs = Blog.GetBlogs(1, 1, ConfigurationFlags.None);
                return blogs.MaxItems;
            }
        }

        /// <summary>
        /// Gets the file not found page from web.config.
        /// </summary>
        /// <returns></returns>
        public static string GetFileNotFoundPage()
        {
            var errorsSection =
                WebConfigurationManager.GetWebApplicationSection("system.web/customErrors") as CustomErrorsSection;
            if(errorsSection != null)
            {
                CustomError fileNotFoundError = errorsSection.Errors["404"];
                if(fileNotFoundError != null)
                {
                    return fileNotFoundError.Redirect;
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
        public static Blog GetBlog(string hostName, string subfolder)
        {
            hostName = Blog.StripPortFromHost(hostName);
            return ObjectProvider.Instance().GetBlog(hostName, subfolder);
        }

        /// <summary>
        /// Creates an initial blog.  This is a convenience method for 
        /// allowing a user with a freshly installed blog to immediately gain access 
        /// to the admin section to edit the blog.
        /// </summary>
        /// <param name="title">Title of the blog</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password.</param>
        /// <param name="subfolder"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static int CreateBlog(string title, string userName, string password, string host, string subfolder)
        {
            return CreateBlog(title, userName, password, host, subfolder, 1, false);
        }

        /// <summary>
        /// Creates an initial blog.  This is a convenience method for 
        /// allowing a user with a freshly installed blog to immediately gain access 
        /// to the admin section to edit the blog.
        /// </summary>
        /// <param name="title">Title of the blog</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password.</param>
        /// <param name="subfolder"></param>
        /// <param name="groupId"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static int CreateBlog(string title, string userName, string password, string host, string subfolder,
                                     int groupId)
        {
            return CreateBlog(title, userName, password, host, subfolder, groupId, false);
        }


        /// <summary>
        /// Creates an initial blog.  This is a convenience method for 
        /// allowing a user with a freshly installed blog to immediately gain access 
        /// to the admin section to edit the blog.
        /// </summary>
        /// <param name="title">Title of the blog.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password.</param>
        /// <param name="subfolder"></param>
        /// <param name="host"></param>
        /// <param name="passwordAlreadyHashed">If true, the password has already been hashed.</param>
        /// <returns></returns>
        public static int CreateBlog(string title, string userName, string password, string host, string subfolder,
                                     bool passwordAlreadyHashed)
        {
            return CreateBlog(title, userName, password, host, subfolder, 1, passwordAlreadyHashed);
        }

        /// <summary>
        /// Creates an initial blog.  This is a convenience method for 
        /// allowing a user with a freshly installed blog to immediately gain access 
        /// to the admin section to edit the blog.
        /// </summary>
        /// <param name="title">Title of the blog.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password.</param>
        /// <param name="subfolder"></param>
        /// <param name="host"></param>
        /// <param name="blogGroupId"></param>
        /// <param name="passwordAlreadyHashed">If true, the password has already been hashed.</param>
        /// <returns></returns>
        public static int CreateBlog(string title, string userName, string password, string host, string subfolder,
                                     int blogGroupId, bool passwordAlreadyHashed)
        {
            if(subfolder != null && subfolder.EndsWith("."))
            {
                throw new InvalidSubfolderNameException(subfolder);
            }

            host = Blog.StripPortFromHost(host);

            //Check for duplicate
            Blog potentialDuplicate = GetBlog(host, subfolder);
            if(potentialDuplicate != null)
            {
                //we found a duplicate!
                throw new BlogDuplicationException(potentialDuplicate);
            }

            //If the subfolder is null, this next check is redundant as it is 
            //equivalent to the check we just made.
            if(!string.IsNullOrEmpty(subfolder))
            {
                //Check to see if we're going to end up hiding another blog.
                Blog potentialHidden = GetBlog(host, string.Empty);
                if(potentialHidden != null)
                {
                    //We found a blog that would be hidden by this one.
                    throw new BlogHiddenException(potentialHidden);
                }
            }

            subfolder = HttpHelper.StripSurroundingSlashes(subfolder);

            if(string.IsNullOrEmpty(subfolder))
            {
                //Check to see if this blog requires a Subfolder value
                //This would occur if another blog has the same host already.
                int activeBlogWithHostCount = Blog.GetBlogsByHost(host, 0, 1, ConfigurationFlags.IsActive).Count;
                if(activeBlogWithHostCount > 0)
                {
                    throw new BlogRequiresSubfolderException(host, activeBlogWithHostCount);
                }
            }
            else
            {
                if(!IsValidSubfolderName(subfolder))
                {
                    throw new InvalidSubfolderNameException(subfolder);
                }
            }

            if(!passwordAlreadyHashed && Settings.UseHashedPasswords)
            {
                password = SecurityHelper.HashPassword(password);
            }

            return (ObjectProvider.Instance().CreateBlog(title, userName, password, host, subfolder, blogGroupId));
        }

        /// <summary>
        /// Updates the database with the configuration data within 
        /// the specified <see cref="Blog"/> instance.
        /// </summary>
        public static void UpdateConfigData(this ObjectProvider repository, Blog info)
        {
            //Check for duplicate
            Blog potentialDuplicate = GetBlog(info.Host, info.Subfolder);
            if(potentialDuplicate != null && !potentialDuplicate.Equals(info))
            {
                //we found a duplicate!
                throw new BlogDuplicationException(potentialDuplicate);
            }

            //Check to see if we're going to end up hiding another blog.
            Blog potentialHidden = GetBlog(info.Host, string.Empty);
            if(potentialHidden != null && !potentialHidden.Equals(info) && potentialHidden.IsActive)
            {
                //We found a blog that would be hidden by this one.
                throw new BlogHiddenException(potentialHidden);
            }

            string subfolderName = info.Subfolder == null
                                       ? string.Empty
                                       : HttpHelper.StripSurroundingSlashes(info.Subfolder);

            if(subfolderName.Length == 0)
            {
                //Check to see if this blog requires a Subfolder value
                //This would occur if another blog has the same host already.
                IPagedCollection<Blog> blogsWithHost = Blog.GetBlogsByHost(info.Host, 0, 1, ConfigurationFlags.IsActive);
                if(blogsWithHost.Count > 0)
                {
                    if(blogsWithHost.Count > 1 || !blogsWithHost.First().Equals(info))
                    {
                        throw new BlogRequiresSubfolderException(info.Host, blogsWithHost.Count);
                    }
                }
            }
            else
            {
                if(!IsValidSubfolderName(subfolderName))
                {
                    throw new InvalidSubfolderNameException(subfolderName);
                }
            }

            info.IsPasswordHashed = Settings.UseHashedPasswords;
            info.AllowServiceAccess = Settings.AllowServiceAccess;

            repository.UpdateBlog(info);
        }

        /// <summary>
        /// Returns true if the specified subfolder name has a 
        /// valid format. It may not start, nor end with ".".  It 
        /// may not contain any of the following invalid characters 
        /// {}[]/\ @!#$%:^&*()?+|"='<>;,
        /// </summary>
        /// <param name="subfolder">subfolder.</param>
        /// <returns></returns>
        public static bool IsValidSubfolderName(string subfolder)
        {
            if(subfolder == null)
            {
                throw new ArgumentNullException("subfolder");
            }

            if(subfolder.EndsWith("."))
            {
                return false;
            }

            const string invalidChars = @"{}[]/\ @!#$%:^&*()?+|""='<>;,";

            foreach(char c in invalidChars)
            {
                if(subfolder.IndexOf(c) > -1)
                {
                    return false;
                }
            }

            foreach(string invalidSubFolder in InvalidSubfolders)
            {
                if(String.Equals(invalidSubFolder, subfolder, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Adds the blog alias to the system.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static bool AddBlogAlias(BlogAlias alias)
        {
            return ObjectProvider.Instance().CreateBlogAlias(alias);
        }

        /// <summary>
        /// Updates the blog alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static bool UpdateBlogAlias(BlogAlias alias)
        {
            return ObjectProvider.Instance().UpdateBlogAlias(alias);
        }

        /// <summary>
        /// Deletes the blog alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static bool DeleteBlogAlias(BlogAlias alias)
        {
            return ObjectProvider.Instance().DeleteBlogAlias(alias);
        }

        /// <summary>
        /// Gets the blog alias.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static BlogAlias GetBlogAlias(int id)
        {
            return ObjectProvider.Instance().GetBlogAliasById(id);
        }

        /// <summary>
        /// Gets the blog group by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public static BlogGroup GetBlogGroup(int id, bool activeOnly)
        {
            return ObjectProvider.Instance().GetBlogGroup(id, activeOnly);
        }

        /// <summary>
        /// Lists the blog groups in this installation.
        /// </summary>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public static ICollection<BlogGroup> ListBlogGroups(bool activeOnly)
        {
            return ObjectProvider.Instance().ListBlogGroups(activeOnly);
        }
    }
}