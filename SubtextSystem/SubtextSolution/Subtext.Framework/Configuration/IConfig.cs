using System;
using System.Web;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Interface definition for Blog configuration classes.
	/// </summary>
	public interface IConfig
	{
		/// <summary>
		/// Gets the blog configuration based on the current http context.
		/// </summary>
		/// <returns></returns>
		BlogConfig GetConfig();
		
		/// <summary>
		/// Gets the configuration based on the specified <see cref="HttpContext"/>. 
		/// Must be implemented by configuration handlers.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns></returns>
		BlogConfig GetConfig(HttpContext context);


		/// <summary>
		/// Gets or sets the blog ID.
		/// </summary>
		/// <value></value>
		int BlogID
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the cache time.
		/// </summary>
		/// <value></value>
		int CacheTime
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value></value>
		string Host
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the application.
		/// </summary>
		/// <value></value>
		string Application
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the image directory.
		/// </summary>
		/// <value></value>
		string ImageDirectory
		{
			get;
			set;
		}
	}
}
