using System;
using System.Configuration;
using System.Web;
using Subtext.Framework.Providers;

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
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public static BlogConfig GetConfig(string hostname, string application)
		{
			BlogConfig result = DTOProvider.Instance().GetConfig(hostname, application);
			if(result == null)
			{
				throw new BlogDoesNotExistException(
					String.Format("A blog matching the location you requested was not found. Host = [{0}], Application = [{1}]",
					hostname, 
					application));
			}
			return result;
		}

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public static bool AddInitialBlog(string userName, string password)
		{
			return DTOProvider.Instance().AddInitialBlogConfiguration(userName, password);
		}

		/// <summary>
		/// Updates the database with the configuration data within 
		/// the specified <see cref="BlogConfig"/> instance.
		/// </summary>
		/// <param name="config">Config.</param>
		/// <returns></returns>
		public static bool UpdateConfigData(BlogConfig config)
		{
			return DTOProvider.Instance().UpdateConfigData(config);
		}
	}
}
