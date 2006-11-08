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
using System.Collections.Specialized;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Helper class to retrieve plugin informations from the DB
	/// </summary>
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

		/// <summary>
		/// Retrieves the blog plugin settings from the storage
		/// </summary>
		/// <param name="pluginId">GUID of the plugin</param>
		/// <returns>A NameValueCollection with all the settings</returns>
		public static NameValueCollection GetPluginGeneralSettings(Guid pluginId)
		{
			return ObjectProvider.Instance().GetPluginGeneralSettings(pluginId);
		}

		/// <summary>
		/// Inserts a new value in the blog settings list
		/// </summary>
		/// <param name="pluginGuid">GUID of the plugin</param>
		/// <param name="key">Setting name</param>
		/// <param name="value">Setting value</param>
		public static void InsertPluginGeneralSettings(Guid pluginGuid, string key, string value)
		{
			ObjectProvider.Instance().InsertPluginGeneralSettings(pluginGuid, key, value);
		}

		/// <summary>
		/// Updates a setting in the blog settings list
		/// </summary>
		/// <param name="pluginGuid">GUID of the plugin</param>
		/// <param name="key">Setting name</param>
		/// <param name="value">Setting value</param>
		public static void UpdatePluginGeneralSettings(Guid pluginGuid, string key, string value)
		{
			ObjectProvider.Instance().UpdatePluginGeneralSettings(pluginGuid, key, value);
		}
	}
}
