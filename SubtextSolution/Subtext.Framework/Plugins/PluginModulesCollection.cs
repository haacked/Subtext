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
	/// This is the custom collection of the modules defined for each plugin
	/// </summary>

	[ConfigurationCollection(typeof(PluginModule))]
	public class PluginModulesCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new PluginModule();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PluginModule)element).Key;
		}

		public void Add(PluginModule element)
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

		public PluginModule this[int index]
		{
			get { return this[index]; }
		}
	}
}
