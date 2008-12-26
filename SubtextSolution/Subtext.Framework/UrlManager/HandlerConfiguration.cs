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
using System.Collections.Generic;

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
		public static void SetControls(HttpContext context, IEnumerable<string> controls)
		{
			if(controls != null)
			{
				context.Items.Add("Subtext.Framework.UrlManager.ControlContext", controls);
			}
		}

		/// <summary>
		/// Gets the controls associated to the specified context.
		/// </summary>
		/// <param name="context">Context.</param>
		public static IEnumerable<string> GetControls(HttpContext context)
		{
			return (string[])context.Items["Subtext.Framework.UrlManager.ControlContext"];
		}

		/// <summary>
		/// Gets or sets the HTTP handlers configured in the HttpHandlers section.
		/// </summary>
		/// <value></value>
		[XmlArray("HttpHandlers")]
		public HttpHandler[] HttpHandlers
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the defualt page location.
		/// </summary>
		/// <value></value>
		[XmlAttribute("defaultPageLocation")]
		public string DefaultPageLocation
		{
			get;
			set;
		}

		private string _fullPageLocation;
		/// <summary>
		/// Gets the full page location.
		/// </summary>
		/// <value></value>
		public string FullPageLocation
		{
			get {
				if(this._fullPageLocation == null)
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
