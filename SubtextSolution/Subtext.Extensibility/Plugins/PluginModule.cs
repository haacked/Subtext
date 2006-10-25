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
	public class PluginModule : ConfigurationElement
	{
		[ConfigurationProperty("key")]
		public string Key
		{
			get { return (string)base["key"]; }
			set { base["key"] = value; }
		}

		[ConfigurationProperty("filename")]
		public string FileName
		{
			get { return (string)base["filename"]; }
			set { base["filename"] = value; }
		}
	}
}
