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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using log4net;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;
using Subtext.Scripting;
using System.Web;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Static helper class used to access various configuration 
	/// settings.
	/// </summary>
	public static class Config
	{
		private readonly static ILog Log = new Log();

	    static UrlBasedBlogInfoProvider _configProvider;

		/// <summary>
		/// Returns an instance of <see cref="BlogConfigurationSettings"/> which 
		/// are configured within web.config as a custom config section.
		/// </summary>
		/// <value></value>
		public static BlogConfigurationSettings Settings
		{
			get
			{
				return ((BlogConfigurationSettings)ConfigurationManager.GetSection("BlogConfigurationSettings"));
			}
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
				if(connectionString == null)
				{
					string connectionStringName = ConfigurationManager.AppSettings["connectionStringName"];
					if (ConfigurationManager.ConnectionStrings[connectionStringName] == null)
						throw new ConfigurationErrorsException(String.Format("There is no connectionString entry associated with the connectionStringName '{0}'.", connectionStringName));
					string connectionStringText = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
					connectionString = ConnectionString.Parse(connectionStringText);
				}
				return connectionString;
			}
		}
		static ConnectionString connectionString;

		/// <summary>
		/// Gets the file not found page from web.config.
		/// </summary>
		/// <returns></returns>
		public static string GetFileNotFoundPage()
		{
			CustomErrorsSection errorsSection = WebConfigurationManager.GetWebApplicationSection("system.web/customErrors") as CustomErrorsSection;
			if (errorsSection != null)
			{
				CustomError fileNotFoundError = errorsSection.Errors["404"];
				if (fileNotFoundError != null)
				{
					return fileNotFoundError.Redirect;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
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
		public static BlogInfo CurrentBlog
		{
			get
			{
				if (HttpContext.Current == null)
					return null;

				if (InstallationManager.IsInHostAdminDirectory)
					return null;
				
				BlogInfo currentBlog = ConfigurationProvider.GetBlogInfo();
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
				IPagedCollection<BlogInfo> blogs = BlogInfo.GetBlogs(1, 1, ConfigurationFlag.IsActive);
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
				IPagedCollection blogs = BlogInfo.GetBlogs(1, 1, ConfigurationFlag.None);
				return blogs.MaxItems;
			}
		}

		/// <summary>
		/// Gets or sets the configuration provider.
		/// </summary>
		/// <value></value>
		public static UrlBasedBlogInfoProvider ConfigurationProvider
		{
			get
			{
				if (_configProvider == null)
				{
					_configProvider = UrlBasedBlogInfoProvider.Instance;
				}
				return _configProvider;
			}
			set
			{
				_configProvider = value;
			}
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="hostName">Hostname.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <returns></returns>
		public static BlogInfo GetBlogInfo(string hostName, string subfolder)
		{
			return GetBlogInfo(hostName, subfolder, false);
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <param name="hostName">Hostname.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the subfolder and hostname match.</param>
		/// <returns></returns>
		public static BlogInfo GetBlogInfo(string hostName, string subfolder, bool strict)
		{
			hostName = BlogInfo.StripPortFromHost(hostName);
			return ObjectProvider.Instance().GetBlogInfo(hostName, subfolder, strict);
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Domain Alias.
		/// </summary>
		/// <param name="domainAlias">Domain alias</param>
		/// <param name="subfolder">Sub Folder</param>
		/// <param name="strict">Strict</param>
		/// <returns></returns>
		public static BlogInfo GetBlogInfoFromDomainAlias(string domainAlias, string subfolder, bool strict)
		{
			domainAlias = BlogInfo.StripPortFromHost(domainAlias);
			return ObjectProvider.Instance().GetBlogByDomainAlias(domainAlias, subfolder, strict);
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
        public static bool CreateBlog(string title, string userName, string password, string host, string subfolder)
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
        public static bool CreateBlog(string title, string userName, string password, string host, string subfolder, int groupId)
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
        public static bool CreateBlog(string title, string userName, string password, string host, string subfolder, bool passwordAlreadyHashed)
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
        public static bool CreateBlog(string title, string userName, string password, string host, string subfolder, int blogGroupId, bool passwordAlreadyHashed)
		{
			if(subfolder != null && subfolder.EndsWith("."))
				throw new InvalidSubfolderNameException(subfolder);

			host = BlogInfo.StripPortFromHost(host);

			//Check for duplicate
			BlogInfo potentialDuplicate = GetBlogInfo(host, subfolder, true);
			if(potentialDuplicate != null)
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

		    //If the subfolder is null, this next check is redundant as it is 
		    //equivalent to the check we just made.
			if (subfolder != null && subfolder.Length > 0)
            {
                //Check to see if we're going to end up hiding another blog.
                BlogInfo potentialHidden = GetBlogInfo(host, string.Empty, true);
                if (potentialHidden != null)
                {
                    //We found a blog that would be hidden by this one.
                    throw new BlogHiddenException(potentialHidden);
                }
            }
			
			subfolder = UrlFormats.StripSurroundingSlashes(subfolder);
			Log.Debug(string.Format("Creating a blog with subfolder '{0}'", subfolder));

			if(subfolder == null || subfolder.Length == 0)
			{
				//Check to see if this blog requires a Subfolder value
				//This would occur if another blog has the same host already.
				int activeBlogWithHostCount = BlogInfo.GetBlogsByHost(host, 0, 1, ConfigurationFlag.IsActive).Count;
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
				password = SecurityHelper.HashPassword(password);

            return (ObjectProvider.Instance().CreateBlog(title, userName, password, host, subfolder, blogGroupId));
		}

		/// <summary>
		/// Updates the database with the configuration data within 
		/// the specified <see cref="BlogInfo"/> instance.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public static void UpdateConfigData(BlogInfo info)
		{
			//Check for duplicate
			BlogInfo potentialDuplicate = GetBlogInfo(info.Host, info.Subfolder, true);
			if (potentialDuplicate != null && !potentialDuplicate.Equals(info))
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

			//Check to see if we're going to end up hiding another blog.
			BlogInfo potentialHidden = GetBlogInfo(info.Host, string.Empty, true);
			if (potentialHidden != null && !potentialHidden.Equals(info) && potentialHidden.IsActive)
			{
				//We found a blog that would be hidden by this one.
				throw new BlogHiddenException(potentialHidden);
			}

			string subfolderName = info.Subfolder == null ? string.Empty : UrlFormats.StripSurroundingSlashes(info.Subfolder);

			if (subfolderName.Length == 0)
			{
				//Check to see if this blog requires a Subfolder value
				//This would occur if another blog has the same host already.
                IPagedCollection<BlogInfo> blogsWithHost = BlogInfo.GetBlogsByHost(info.Host, 0, 1, ConfigurationFlag.IsActive);
				if(blogsWithHost.Count > 0)
				{
					if (blogsWithHost.Count > 1 || !blogsWithHost[0].Equals(info))
					{
						throw new BlogRequiresSubfolderException(info.Host, blogsWithHost.Count);
					}
				}
			}
			else
			{
				if (!IsValidSubfolderName(subfolderName))
				{
					throw new InvalidSubfolderNameException(subfolderName);
				}
			}
			
			info.IsPasswordHashed = Settings.UseHashedPasswords;
			info.AllowServiceAccess = Settings.AllowServiceAccess;

			ObjectProvider.Instance().UpdateBlog(info);
		}

        //TODO: Is this the right place to put this list?
        private static readonly string[] _invalidSubfolders = { "Tags", "Admin", "bin", "ExternalDependencies", "HostAdmin", "Images", "Install", "Properties", "Providers", "Scripts", "Skins", "SystemMessages", "UI", "Modules", "Services", "Category", "Archive", "Archives", "Comments", "Articles", "Posts", "Story", "Stories", "Gallery", "aggbug", "Sitemap" };

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
				throw new ArgumentNullException("subfolder", "Subfolder cannot be null.");

			if (subfolder.EndsWith("."))
				return false;

			string invalidChars = @"{}[]/\ @!#$%:^&*()?+|""='<>;,";

			foreach(char c in invalidChars)
			{
				if(subfolder.IndexOf(c) > -1)
					return false;
			}

			foreach(string invalidSubFolder in _invalidSubfolders)
			{
				if (String.Equals(invalidSubFolder, subfolder, StringComparison.InvariantCultureIgnoreCase))
					return false;
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
		public static IList<BlogGroup> ListBlogGroups(bool activeOnly)
		{
			return ObjectProvider.Instance().ListBlogGroups(activeOnly);
		}
	}
}
