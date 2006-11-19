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
	/// This is the custom collection of the default settings defined for each plugin
	/// </summary>

	[ConfigurationCollection(typeof(PluginDefaultSetting))]
	public class PluginDefaultSettingsCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new PluginDefaultSetting();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PluginDefaultSetting)element).Key;
		}

		public void Add(PluginDefaultSetting element)
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

		public PluginDefaultSetting this[int index]
		{
			get { return (PluginDefaultSetting)this[index]; }
		}

		public new string this[string key]
		{
			get
			{
				foreach (PluginDefaultSetting conf in this)
				{
					if (conf.Key.Equals(key))
						return conf.Value;
				}
				return null;
			}
		}
	}
}
