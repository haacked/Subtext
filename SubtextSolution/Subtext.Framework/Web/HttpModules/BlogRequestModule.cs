using System;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.Properties;

namespace Subtext.Web.HttpModules
{
	/// <summary>
	/// Examines incoming http requests and adds Subtext specific as well as blog 
	/// specific information to the context.
	/// </summary>
	public class BlogRequestModule : IHttpModule
	{
		/// <summary>
		/// Initializes a module and prepares it to handle
		/// requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
            if (context == null)
            {
                throw new ArgumentNullException(Resources.ArgumentNull_Generic);
            }

			context.BeginRequest += MapUrlToBlogStatus;
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the
		/// module that implements <see langword="IHttpModule."/>
		/// </summary>
		public void Dispose()
		{
			//Do Nothing.
		}

		/// <summary>
		/// Maps the incoming URL to the corresponding blog. If no blog matches, then 
		/// makes sure any host application settings are still set.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private static void MapUrlToBlogStatus(object sender, EventArgs e)
		{
			string subfolder = UrlFormats.GetBlogSubfolderFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			if(!Config.IsValidSubfolderName(subfolder))
				subfolder = string.Empty;

			BlogRequest.Current = new BlogRequest(Host, subfolder, HttpContext.Current.Request.Url, HttpContext.Current.Request.IsLocal);
		}
		
		/// <summary>
		/// Gets the current host.
		/// </summary>
		/// <returns></returns>
		protected static string Host
		{
			get
			{
				string host = HttpContext.Current.Request.Params["HTTP_HOST"];
				if (String.IsNullOrEmpty(host))
					host = HttpContext.Current.Request.Url.Authority;

				return host;
			}
		}
	}
}
