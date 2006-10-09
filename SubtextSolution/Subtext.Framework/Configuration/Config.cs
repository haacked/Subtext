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
using System.Configuration;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Static helper class used to access various configuration 
	/// settings.
	/// </summary>
	public static class Config
	{
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
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the current blog.
		/// </summary>
		/// <returns></returns>
		public static BlogInfo CurrentBlog
		{
			get
			{
				return ConfigurationProvider.GetBlogInfo();
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
				if(_configProvider == null)
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
			hostName = BlogInfo.NormalizeHostName(hostName);
			return ObjectProvider.Instance().GetBlogInfo(hostName, subfolder, strict);
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
			return CreateBlog(title, userName, password, host, subfolder, false);
		}

		/// <summary>
		/// Creates an initial blog.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <param name="subfolder"></param>
		/// <param name="host"></param>
		/// <param name="passwordAlreadyHashed">If true, the password has already been hashed.</param>
		/// <returns></returns>
		public static bool CreateBlog(string title, string userName, string password, string host, string subfolder, bool passwordAlreadyHashed)
		{
			if(subfolder != null && subfolder.StartsWith("."))
				throw new InvalidSubfolderNameException(subfolder);

			host = BlogInfo.NormalizeHostName(host);

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
			Console.WriteLine("Creating a blog with subfolder '" + subfolder + "'");

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
				password = Security.HashPassword(password);

            return (ObjectProvider.Instance().CreateBlog(title, userName, password, host, subfolder));
		}

		/// <summary>
		/// Updates the database with the configuration data within 
		/// the specified <see cref="BlogInfo"/> instance.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public static bool UpdateConfigData(BlogInfo info)
		{
			//Check for duplicate
			BlogInfo potentialDuplicate = GetBlogInfo(info.Host, info.Subfolder);
			if(potentialDuplicate != null && !potentialDuplicate.Equals(info))
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

			//Check to see if we're going to end up hiding another blog.
			BlogInfo potentialHidden = GetBlogInfo(info.Host, string.Empty);
			if(potentialHidden != null && !potentialHidden.Equals(info) && potentialHidden.IsActive)
			{
				//We found a blog that would be hidden by this one.
				throw new BlogHiddenException(potentialHidden);
			}

			string subfolderName = info.Subfolder == null ? string.Empty : UrlFormats.StripSurroundingSlashes(info.Subfolder);

			if(subfolderName.Length == 0)
			{
				//Check to see if this blog requires a Subfolder value
				//This would occur if another blog has the same host already.
                IPagedCollection<BlogInfo> blogsWithHost = BlogInfo.GetBlogsByHost(info.Host, 0, 1, ConfigurationFlag.IsActive);
				if(blogsWithHost.Count > 0)
				{
					if(blogsWithHost.Count > 1 || !blogsWithHost[0].Equals(info))
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

			return ObjectProvider.Instance().UpdateBlog(info);
		}

		//TODO: Is this the right place to put this list?
		private static string[] _invalidSubfolders = {"Admin", "bin", "ExternalDependencies", "HostAdmin", "Images", "Install", "Modules", "Services", "Skins", "UI", "Category", "Archive", "Archives", "Comments", "Articles", "Posts", "Story", "Stories", "Gallery", "Providers", "aggbug"};

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
				throw new ArgumentNullException("Subfolder cannot be null.");

			if(subfolder.StartsWith(".") || subfolder.EndsWith("."))
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
	}
}
