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
using System.Web;
using System.Web.Caching;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Helper class to retrieve plugin informations from the DB
	/// </summary>
	public class Plugin
	{
		private const string PLUGINCACHENAMEFORMAT = "{0}_PLUGINLIST"; //{0} is the blog id

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

		/// <summary>
		/// Get all enabled plugins for the current blog from the ASP.NET Cache
		/// </summary>
		/// <returns>A collection with the GUIDs of all enabled plugins</returns>
		public static ICollection<Guid> GetEnabledPluginsFromCache()
		{
			int blogId = Subtext.Framework.Configuration.Config.CurrentBlog.Id;
			ICollection<Guid> pluginList = null;

			//check to see if cache is not null (we are not running a unit test)
			if (HttpContext.Current.Cache != null)
			{
				//try to get the object from the cache
				object cachedPluginList = HttpContext.Current.Cache[String.Format(PLUGINCACHENAMEFORMAT,blogId)];
				pluginList = (ICollection<Guid>)cachedPluginList;
			}
			//if the pluginlist is still null (not found inside the cache) go and hit the DB
			//and then store the findings inside the cache
			if (pluginList == null)
			{
				pluginList = Plugin.GetEnabledPlugins();
				StorePluginListToCache(blogId, pluginList);
			}

			return pluginList;
		}


		#region Cache managing helper classes

		private static void StorePluginListToCache(int blogId, ICollection<Guid> pluginList)
		{
			HttpContext.Current.Cache.Insert(String.Format(PLUGINCACHENAMEFORMAT, blogId), pluginList, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), CacheItemPriority.NotRemovable, null);
		}

		private static void RemovePluginListFromCache(int blogId)
		{
			HttpContext.Current.Cache.Remove(String.Format(PLUGINCACHENAMEFORMAT, blogId));
		}

		#endregion Cache managing helper classes

	}
}
