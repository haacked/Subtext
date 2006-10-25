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
using System.Collections.Generic;
using System.Text;
using Subtext.Extensibility;
using Subtext.Extensibility.Plugins;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Components
{
	public class Plugin
	{
		/// <summary>
		/// Get all enabled plugins for the current blog
		/// </summary>
		/// <returns>A collection with the GUIDs of all enabled plugins</returns>
		public static ICollection<Guid> GetEnabledPlugins()
		{
			return ObjectProvider.Instance().GetEnabledPlugins();
		}

		/// <summary>
		/// Enables a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">GUID of the plugin to enable</param>
		/// <returns>True if the plugin has been enabled, falso otherwise</returns>
		public static bool EnablePlugin(Guid pluginId)
		{
			return ObjectProvider.Instance().EnablePlugin(pluginId);
		}

		/// <summary>
		/// Disables a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">GUID of the plugin to disable</param>
		/// <returns>True if the plugin has been disable, falso otherwise</returns>
		public static bool DisablePlugin(Guid pluginId)
		{
			return ObjectProvider.Instance().DisablePlugin(pluginId);
		}
	}
}
