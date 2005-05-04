using System;
using System.Configuration;
using System.Web;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Summary description for Config.
	/// </summary>
	public class Config
	{
		public Config()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static BlogConfig CurrentBlog()
		{
			return CurrentBlog(HttpContext.Current);
		}

		public static BlogConfig CurrentBlog(HttpContext context)
		{
			return ConfigProvider.Instance().GetConfig(context);
		}

		public static bool UpdateConfigData(BlogConfig config)
		{
			return DTOProvider.Instance().UpdateConfigData(config);
		}

		public static BlogConfigurationSettings Settings
		{
			get
			{
				return ((BlogConfigurationSettings)ConfigurationSettings.GetConfig("BlogConfigurationSettings"));
			}
		}

		public static BlogConfig GetConfig(int BlogID)
		{
			return  DTOProvider.Instance().GetConfig(BlogID);
		}

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
	}
}
