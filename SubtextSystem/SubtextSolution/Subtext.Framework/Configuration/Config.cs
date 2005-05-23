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
		/// Returns a <see cref="BlogConfig"/> instance containing 
		/// the configuration settings for the current blog.
		/// </summary>
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <returns></returns>
		public static BlogConfig CurrentBlog
		{
			get
			{
				return ConfigProvider.Instance().GetConfig(HttpContext.Current);
			}
		}

		/// <summary>
		/// Returns a <see cref="BlogConfig"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public static BlogConfig GetConfig(string hostname, string application)
		{
			return GetConfig(hostname, application, true);
		}

		/// <summary>
		/// Returns a <see cref="BlogConfig"/> instance containing 
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
		public static BlogConfig GetConfig(string hostname, string application, bool strict)
		{
			return DTOProvider.Instance().GetConfig(hostname, application, strict);
		}

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public static bool AddBlogConfiguration(string userName, string password, string host, string application)
		{
			//Check for duplicate
			BlogConfig potentialDuplicate = Subtext.Framework.Configuration.Config.GetConfig(host, application);
			if(potentialDuplicate != null)
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

			//Check to see if we're going to end up hiding another blog.
			BlogConfig potentialHidden = Subtext.Framework.Configuration.Config.GetConfig(host, string.Empty);
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
				BlogConfigCollection blogsWithHost = BlogConfig.GetBlogsByHost(host);
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

			return (DTOProvider.Instance().AddBlogConfiguration(userName, password, host, application));
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
		/// the specified <see cref="BlogConfig"/> instance.
		/// </summary>
		/// <param name="config">Config.</param>
		/// <returns></returns>
		public static bool UpdateConfigData(BlogConfig config)
		{
			//Check for duplicate
			BlogConfig potentialDuplicate = Subtext.Framework.Configuration.Config.GetConfig(config.Host, config.Application);
			if(potentialDuplicate != null && !potentialDuplicate.Equals(config))
			{
				//we found a duplicate!
				throw new BlogDuplicationException(potentialDuplicate);
			}

			//Check to see if we're going to end up hiding another blog.
			BlogConfig potentialHidden = Subtext.Framework.Configuration.Config.GetConfig(config.Host, string.Empty);
			if(potentialHidden != null && !potentialHidden.Equals(config))
			{
				//We found a blog that would be hidden by this one.
				throw new BlogHiddenException(potentialHidden);
			}

			string application = config.Application == null ? string.Empty : config.Application.Replace("/", string.Empty);

			if(application.Length == 0)
			{
				//Check to see if this blog requires an Application value
				//This would occur if another blog has the same host already.
				BlogConfigCollection blogsWithHost = BlogConfig.GetBlogsByHost(config.Host);
				if(blogsWithHost.Count > 0)
				{
					if(blogsWithHost.Count > 1 || !blogsWithHost[0].Equals(config))
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
			
			if(config.Application.Length > 0 && HttpContext.Current != null)
			{
				// You're going to hate this, but we just create a new stub 
				// We won't delete the old one because IIS will have a lock 
				// on it. It's just a harmless empty stub.
				if(!CreateApplicationStub(config.Application))
				{
					return false;
				}
			}

			return DTOProvider.Instance().UpdateConfigData(config);
		}

		//TODO: Is this the right place to put this list?
		private static string[] _invalidApplications = {"Admin", "bin", "ExternalDependencies", "HostAdmin", "Images", "Modules", "Services", "Skins", "UI", "Category", "Archive", "Archives", "Comments", "Articles", "Posts", "Story", "Stories", "Gallery" };

		static bool IsValidApplicationName(string application)
		{
			if(application.StartsWith(".") || application.EndsWith("."))
				return false;

			string invalidChars = @"/\\ @!#$%;^&*()?+|""=\'<>;";

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