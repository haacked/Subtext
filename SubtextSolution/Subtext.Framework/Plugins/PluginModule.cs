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
	/// This is about the modules defined for each plugin
	/// </summary>
	public class PluginModule : ConfigurationElement
	{
		/// <summary>
		/// Key of the Module
		/// </summary>
		[ConfigurationProperty("key")]
		public string Key
		{
			get { return (string)base["key"]; }
			set { base["key"] = value; }
		}

		/// <summary>
		/// FileName for the module
		/// </summary>
		[ConfigurationProperty("filename")]
		public string FileName
		{
			get { return (string)base["filename"]; }
			set { base["filename"] = value; }
		}
	}
}
