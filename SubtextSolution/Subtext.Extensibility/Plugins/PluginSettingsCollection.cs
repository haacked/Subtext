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
	[ConfigurationCollection(typeof(PluginSettings))]
	public class PluginSettingsCollection: ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new PluginSettings();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PluginSettings) element).Name;
		}

		public void Add(PluginSettings element)
		{
			this.BaseAdd(element);
		}

		public void Remove(string key)
		{
			this.BaseRemove(key);
		}

		public void Clear()
		{
			this.BaseClear();
		}

		public PluginSettings this[int idx]
		{
			get { return (PluginSettings)this[idx]; }
		}
	}
}
