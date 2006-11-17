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

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Class used to collect the data stored inside the custom configuration section of the web.config<br />
	/// This is about the default sitewide settings defined for each plugin
	/// </summary>
	public class PluginDefaultSetting : ConfigurationElement
	{
		/// <summary>
		/// Key of the setting
		/// </summary>
		[ConfigurationProperty("key")]
		public string Key
		{
			get { return (string)base["key"]; }
			set { base["key"] = value; }
		}

		/// <summary>
		/// value for the setting
		/// </summary>
		[ConfigurationProperty("value")]
		public string Value
		{
			get { return (string)base["value"]; }
			set { base["value"] = value; }
		}
	}
}
