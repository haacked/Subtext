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
using System.Web;
using System.Xml.Serialization;
using Subtext.Framework.Properties;

namespace Subtext.Framework.UrlManager
{
	/// <summary>
	/// Configuration class for the HandlerConfiguration section of 
	/// the web.config file.
	/// </summary>
	public class HandlerConfiguration
	{
		/// <summary>
		/// Sets the controls.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="controls">Controls.</param>
		public static void SetControls(HttpContext context, string[] controls)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
			}

			if (controls != null)
			{
				context.Items.Add("Subtext.Framework.UrlManager.ControlContext", controls);
			}
		}

		/// <summary>
		/// Gets the controls associated to the specified context.
		/// </summary>
		/// <param name="context">Context.</param>
		public static string[] GetControls(HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
			}

			return (string[])context.Items["Subtext.Framework.UrlManager.ControlContext"];
		}

		private HttpHandler[] _httpHandlers;
		/// <summary>
		/// Gets or sets the HTTP handlers configured in the HttpHandlers section.
		/// </summary>
		/// <value></value>
		[XmlArray("HttpHandlers")]
		public HttpHandler[] HttpHandlers
		{
			get { return this._httpHandlers; }
			set { this._httpHandlers = value; }
		}

		private string _defaultPageLocation;
		/// <summary>
		/// Gets or sets the defualt page location.
		/// </summary>
		/// <value></value>
		[XmlAttribute("defaultPageLocation")]
		public string DefaultPageLocation
		{
			get { return this._defaultPageLocation; }
			set { this._defaultPageLocation = value; }
		}

		private string _fullPageLocation;
		/// <summary>
		/// Gets the full page location.
		/// </summary>
		/// <value></value>
		public string FullPageLocation
		{
			get
			{
				if (this._fullPageLocation == null)
				{
					this._fullPageLocation = HttpContext.Current.Request.MapPath("~/" + DefaultPageLocation);
				}
				return this._fullPageLocation;
			}
		}

		/// <summary>
		/// returns an instance of the HandlerConfiguration from 
		/// the configuration settings.
		/// </summary>
		/// <returns></returns>
		public static HandlerConfiguration Instance()
		{
			return ((HandlerConfiguration)ConfigurationManager.GetSection("HandlerConfiguration"));
		}
	}
}
