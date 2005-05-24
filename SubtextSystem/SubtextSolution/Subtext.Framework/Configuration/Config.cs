using System;
using System.Configuration;
using System.IO;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Exceptions;
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
				return ConfigurationProvider.GetBlogInfo(HttpContext.Current);
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
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public static BlogInfo GetBlogInfo(string hostname, string application)
		{
			return GetBlogInfo(hostname, application, true);
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
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the application and hostname match.</param>
		/// <returns></returns>
		public static BlogInfo GetBlogInfo(string hostname, string application, bool strict)
		{
			return DTOProvider.Instance().GetBlogInfo(hostname, application, strict);
		}

		/// <summary>
		/// Creates an initial blog.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public static bool CreateBlog(string title, string userName, string password, string host, string application)
		{
			//Check for duplicate
			BlogInfo potentialDuplicate = Subtext.Framework.Configuration.Config.GetBlogInfo(host, application);
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
			
			application = application.Replace("/", string.Empty);

			if(application == null || application.Length == 0)
			{
				//Check to see if this blog requires an Application value
				//This would occur if another blog has the same host already.
				BlogInfoCollection blogsWithHost = BlogInfo.GetBlogsByHost(host);
				if(blogsWithHost.Count > 0)
				{
					throw new BlogRequiresApplicationException(blogsWithHost.Count);
				}
			}
			else
			{
				if(!IsValidApplicationName(application))
				{
					throw new InvalidApplicationNameException(application);
				}
			}

			if(application.Length > 0 && HttpContext.Current != null)
			{
				if(!CreateApplicationStub(application))
				{
					return false;
				}
			}

			if(Config.Settings.UseHashedPasswords)
			{
				password = Security.HashPassword(password);
			}

			return (DTOProvider.Instance().CreateBlog(title, userName, password, host, application));
		}

		private static bool CreateApplicationStub(string application)
		{
			if(!IsValidApplicationName(application))
			{
				throw new InvalidApplicationNameException(application);
			}

			string blogDirectoryPath = GetBlogPhysicalPath(application);

			if(!Directory.Exists(blogDirectoryPath))
			{
				try
				{
					Directory.CreateDirectory(blogDirectoryPath);
					using(StreamWriter writer = File.CreateText(Path.Combine(blogDirectoryPath, "Default.aspx")))
					{
						writer.Close(); //Empty stub file.
					}
					return true;
				}
				catch(System.IO.IOException exception)
				{
					throw new BlogApplicationDirectoryCreateException(exception);
				}
			}
			return true;
		}

		static string GetBlogPhysicalPath(string application)
		{
			string applicationPhysicalPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath);
			return Path.Combine(applicationPhysicalPath, application);
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
			if(potentialHidden != null && !potentialHidden.Equals(info))
			{
				//We found a blog that would be hidden by this one.
				throw new BlogHiddenException(potentialHidden);
			}

			string application = info.Application == null ? string.Empty : info.Application.Replace("/", string.Empty);

			if(application.Length == 0)
			{
				//Check to see if this blog requires an Application value
				//This would occur if another blog has the same host already.
				BlogInfoCollection blogsWithHost = BlogInfo.GetBlogsByHost(info.Host);
				if(blogsWithHost.Count > 0)
				{
					if(blogsWithHost.Count > 1 || !blogsWithHost[0].Equals(info))
					{
						throw new BlogRequiresApplicationException(blogsWithHost.Count);
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
			
			if(info.Application.Length > 0 && HttpContext.Current != null)
			{
				// You're going to hate this, but we just create a new stub 
				// We won't delete the old one because IIS will have a lock 
				// on it. It's just a harmless empty stub.
				if(!CreateApplicationStub(info.Application))
				{
					return false;
				}
			}

			if(Config.Settings.UseHashedPasswords)
			{
				info.Password = Security.HashPassword(info.Password);
			}

			return DTOProvider.Instance().UpdateBlog(info);
		}

		//TODO: Is this the right place to put this list?
		private static string[] _invalidApplications = {"Admin", "bin", "ExternalDependencies", "HostAdmin", "Images", "Modules", "Services", "Skins", "UI", "Category", "Archive", "Archives", "Comments", "Articles", "Posts", "Story", "Stories", "Gallery" };

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