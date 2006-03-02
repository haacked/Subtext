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

using System.Configuration;
using Subtext.Framework.Components;
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
	public sealed class Config
	{
		static UrlBasedBlogInfoProvider _configProvider = null;
		private Config() {}

		/// <summary>
		/// Returns an instance of <see cref="BlogConfigurationSettings"/> which 
		/// are configured within web.config as a custom config section.
		/// </summary>
		/// <value></value>
		public static BlogConfigurationSettings Settings
		{
			get
			{
				return ((BlogConfigurationSettings)ConfigurationSettings.GetConfig("BlogConfigurationSettings"));
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
				/* TASK - version 1.1
				 * We get a pageSize of 10 here - the number 10 is just a magic #, and 
				 * could be any number... possibly move down to 1 for next release.
				 */
				int notUsed;
				return BlogInfo.GetActiveBlogs(1, 10, true, out notUsed).Count;
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
				/* TASK - version 1.1
				 * We get a pageSize of 10 here - the number 10 is just a magic #, and 
				 * could be any number... possibly move down to 1 for next release.
				 * we only need to get one page of blogInfo objects b/c the SP that the 
				 * dataProvider calls will return the total number of blogs as a return value. 
				 * This value is then set as the MaxItems property of our BlogInfoCollection.
				 */
				int totalBlogCount;
				BlogInfo.GetActiveBlogs(1, 10, true, out totalBlogCount);
				return totalBlogCount;
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
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public static BlogInfo GetBlogInfo(string hostName, string application)
		{
			return GetBlogInfo(hostName, application, true);
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
		/// <param name="subfolder">Application.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the application and hostname match.</param>
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
				throw new InvalidApplicationNameException(subfolder);

			host = BlogInfo.NormalizeHostName(host);

			//Check for duplicate
			BlogInfo potentialDuplicate = Subtext.Framework.Configuration.Config.GetBlogInfo(host, subfolder);
			if(potentialDuplicate != null)
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

			//Check to see if we're going to end up hiding another blog.
			BlogInfo potentialHidden = Subtext.Framework.Configuration.Config.GetBlogInfo(host, string.Empty);
			if(potentialHidden != null)
			{
				//We found a blog that would be hidden by this one.
				throw new BlogHiddenException(potentialHidden);
			}
			
			subfolder = UrlFormats.StripSurroundingSlashes(subfolder);

			if(subfolder == null || subfolder.Length == 0)
			{
				//Check to see if this blog requires an Application value
				//This would occur if another blog has the same host already.
				int activeBlogWithHostCount = BlogInfo.GetActiveBlogsByHost(host).Count;
				if(activeBlogWithHostCount > 0)
				{
					throw new BlogRequiresApplicationException(host, activeBlogWithHostCount);
				}
			}
			else
			{
				if(!IsValidApplicationName(subfolder))
				{
					throw new InvalidApplicationNameException(subfolder);
				}
			}

			if(!passwordAlreadyHashed && Config.Settings.UseHashedPasswords)
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
			BlogInfo potentialDuplicate = Subtext.Framework.Configuration.Config.GetBlogInfo(info.Host, info.Application);
			if(potentialDuplicate != null && !potentialDuplicate.Equals(info))
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

			//Check to see if we're going to end up hiding another blog.
			BlogInfo potentialHidden = Subtext.Framework.Configuration.Config.GetBlogInfo(info.Host, string.Empty);
			if(potentialHidden != null && !potentialHidden.Equals(info) && potentialHidden.IsActive)
			{
				//We found a blog that would be hidden by this one.
				throw new BlogHiddenException(potentialHidden);
			}

			string application = info.Application == null ? string.Empty : UrlFormats.StripSurroundingSlashes(info.Application);

			if(application.Length == 0)
			{
				//Check to see if this blog requires an Application value
				//This would occur if another blog has the same host already.
				BlogInfoCollection blogsWithHost = BlogInfo.GetActiveBlogsByHost(info.Host);
				if(blogsWithHost.Count > 0)
				{
					if(blogsWithHost.Count > 1 || !blogsWithHost[0].Equals(info))
					{
						throw new BlogRequiresApplicationException(info.Host, blogsWithHost.Count);
					}
				}
			}
			else
			{
				if(!IsValidApplicationName(application))
				{
					throw new InvalidApplicationNameException(application);
				}
			}
			
			info.IsPasswordHashed = Config.Settings.UseHashedPasswords;
			info.AllowServiceAccess = Config.Settings.AllowServiceAccess;

			return ObjectProvider.Instance().UpdateBlog(info);
		}

		//TODO: Is this the right place to put this list?
		private static string[] _invalidApplications = {"Admin", "bin", "ExternalDependencies", "HostAdmin", "Images", "Install", "Modules", "Services", "Skins", "UI", "Category", "Archive", "Archives", "Comments", "Articles", "Posts", "Story", "Stories", "Gallery" };

		/// <summary>
		/// Returns true if the specified application name has a 
		/// valid format. It may not start, nor end with ".".  It 
		/// may not contain any of the following invalid characters 
		/// {}[]/\ @!#$%:^&*()?+|"='<>;,
		/// </summary>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public static bool IsValidApplicationName(string application)
		{
			if(application.StartsWith(".") || application.EndsWith("."))
				return false;

			string invalidChars = @"{}[]/\ @!#$%:^&*()?+|""='<>;,";

			foreach(char c in invalidChars)
			{
				if(application.IndexOf(c) > -1)
					return false;
			}

			foreach(string invalidApp in _invalidApplications)
			{
				if(StringHelper.AreEqualIgnoringCase(invalidApp, application))
					return false;
			}
			return true;
		}
	}
}