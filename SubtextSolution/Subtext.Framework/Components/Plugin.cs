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
using Subtext.Extensibility.Plugins;
using Subtext.Framework.Providers;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Globalization;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Helper class to retrieve plugin informations from the DB
	/// </summary>
	public class Plugin
	{
		private const string PluginPerBlogCacheNameFormat = "{0}_PLUGINLISTWITHBLOGSETTINGS"; //{0} is the blog id
		private const string PluginPerEntryCacheNameFormat = "{0}_PLUGINLISTWITHENTRYETTINGS"; //{0} is the entry id


		private PluginBase _initializedPlugin;

		/// <summary>
		/// An instance of the plugin, already initialized
		/// </summary>
		public PluginBase InitializedPlugin
		{
			get { return _initializedPlugin; }
			set { _initializedPlugin = value; }
		}

		private NameValueCollection _settings;

		public NameValueCollection Settings
		{
			get { return _settings; }
			set { _settings = value; }
		}

		//TODO: Make this back to internal once I get the strong name going.
		public Plugin(PluginBase plugin, NameValueCollection settings)
		{
			_initializedPlugin = plugin;
			_settings = settings;
		}	

		/// <summary>
		/// Get all enabled plugins for the current blog with its own settings from the ASP.NET Cache
		/// </summary>
		/// <returns>A collection with the GUIDs of all enabled plugins</returns>
		public static IDictionary<Guid, Plugin> GetEnabledPluginsWithBlogSettingsFromCache()
		{
			int blogId = Subtext.Framework.Configuration.Config.CurrentBlog.Id;
			IDictionary<Guid, Plugin> pluginList = null;

			//check to see if cache is not null (we are not running a unit test)
			if (HttpContext.Current.Cache != null)
			{
				//try to get the object from the cache
				object cachedPluginList = HttpContext.Current.Cache[String.Format(CultureInfo.InvariantCulture, PluginPerBlogCacheNameFormat, blogId)];
				pluginList = (IDictionary<Guid, Plugin>)cachedPluginList;
			}
			//if the pluginlist is still null (not found inside the cache) go and hit the DB
			//and then store the findings inside the cache
			if (pluginList == null)
			{
				ICollection<Guid> pluginGuids = GetEnabledPlugins();
				pluginList = new Dictionary<Guid, Plugin>(pluginGuids.Count);
				foreach (Guid guid in pluginGuids)
				{
					PluginBase currPlugin = SubtextApplication.Current.GetPluginByGuid(guid);
					pluginList.Add(guid, new Plugin(currPlugin, GetPluginBlogSettings(guid)));
				}
				StoreEnabledPluginsWithBlogSettingsToCache(pluginList);
			}

			return pluginList;
		}

		/// <summary>
		/// Get all enabled plugins for the current blog with its own settings from the ASP.NET Cache
		/// </summary>
		/// <returns>A collection with the GUIDs of all enabled plugins</returns>
		public static IDictionary<Guid, Plugin> GetEnabledPluginsWithEntrySettingsFromCache(int entryid)
		{
			IDictionary<Guid, Plugin> pluginList = null;

			//check to see if cache is not null (we are not running a unit test)
			if (HttpContext.Current.Cache != null)
			{
				//try to get the object from the cache
				object cachedPluginList = HttpContext.Current.Cache[String.Format(CultureInfo.InvariantCulture, PluginPerEntryCacheNameFormat, entryid)];
				pluginList = (IDictionary<Guid, Plugin>)cachedPluginList;
			}
			//if the pluginlist is still null (not found inside the cache) go and hit the DB
			//and then store the findings inside the cache
			if (pluginList == null)
			{
				ICollection<Guid> pluginGuids = GetEnabledPlugins();
				pluginList = new Dictionary<Guid, Plugin>(pluginGuids.Count);
				foreach (Guid guid in pluginGuids)
				{
					PluginBase currPlugin = SubtextApplication.Current.GetPluginByGuid(guid);
					pluginList.Add(guid, new Plugin(currPlugin, GetPluginEntrySettings(guid, entryid)));
				}
				StoreEnabledPluginsWithEntrySettingsToCache(pluginList, entryid);
			}

			return pluginList;
		}

		#region Enable/Disable Plugins

		/// <summary>
		/// Get all enabled plugins for the current blog
		/// </summary>
		/// <returns>A collection with the GUIDs of all enabled plugins</returns>
		private static ICollection<Guid> GetEnabledPlugins()
		{
			return ObjectProvider.Instance().GetEnabledPlugins();
		}

		/// <summary>
		/// Enables a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">GUID of the plugin to enable</param>
		/// <returns>True if the plugin has been enabled, false otherwise</returns>
		public static void EnablePlugin(Guid pluginId)
		{
			ObjectProvider.Instance().EnablePlugin(pluginId);

			//Removes the list of enabled plugins for the current blog from the cache
			//This way we don't have strange caching issues
			RemoveEnabledPluginsWithBlogSettingsFromCache();
		}

		/// <summary>
		/// Disables a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">GUID of the plugin to disable</param>
		/// <returns>True if the plugin has been disable, false otherwise</returns>
		public static void DisablePlugin(Guid pluginId)
		{
			ObjectProvider.Instance().DisablePlugin(pluginId);

			//Removes the list of enabled plugins for the current blog from the cache
			//This way we don't have strange caching issues
			RemoveEnabledPluginsWithBlogSettingsFromCache();
		}
		#endregion Enable/Disable Plugins


		#region Blog Plugin Settings
		/// <summary>
		/// Retrieves the blog plugin settings from the storage
		/// </summary>
		/// <param name="pluginId">GUID of the plugin</param>
		/// <returns>A NameValueCollection with all the settings</returns>
		public static NameValueCollection GetPluginBlogSettings(Guid pluginId)
		{
			return ObjectProvider.Instance().GetPluginBlogSettings(pluginId);
		}

		/// <summary>
		/// Inserts a new value in the blog settings list
		/// </summary>
		/// <param name="pluginGuid">GUID of the plugin</param>
		/// <param name="key">Setting name</param>
		/// <param name="value">Setting value</param>
		public static void InsertPluginBlogSettings(Guid pluginGuid, string key, string value)
		{
			ObjectProvider.Instance().InsertPluginBlogSettings(pluginGuid, key, value);

			//Removes the list of enabled plugins for the current blog from the cache
			//This way we don't have strange caching issues
			RemoveEnabledPluginsWithBlogSettingsFromCache();
		}

		/// <summary>
		/// Updates a setting in the blog settings list
		/// </summary>
		/// <param name="pluginGuid">GUID of the plugin</param>
		/// <param name="key">Setting name</param>
		/// <param name="value">Setting value</param>
		public static void UpdatePluginBlogSettings(Guid pluginGuid, string key, string value)
		{
			ObjectProvider.Instance().UpdatePluginBlogSettings(pluginGuid, key, value);

			//Removes the list of enabled plugins for the current blog from the cache
			//This way we don't have strange caching issues
			RemoveEnabledPluginsWithBlogSettingsFromCache();
		}

        #endregion Blog Plugin Settings

        #region Entry Plugin Settings

        /// <summary>
        /// Retrieves plugin settings for a specified entry from the storage
        /// </summary>
        /// <param name="pluginGuid">GUID of the plugin</param>
        /// <param name="entryId">Id of the blog entry</param>
        /// <returns>A NameValueCollection with all the settings</returns>
        public static NameValueCollection GetPluginEntrySettings(Guid pluginGuid, int entryId)
        {
            return ObjectProvider.Instance().GetPluginEntrySettings(pluginGuid, entryId);
        }

        /// <summary>
        /// Inserts a new value in the plugin settings list for a specified entry
        /// </summary>
        /// <param name="pluginGuid">GUID of the plugin</param>
        /// <param name="entryId">Id of the blog entry</param>
        /// <param name="key">Setting name</param>
        /// <param name="value">Setting value</param>
        public static void InsertPluginEntrySettings(Guid pluginGuid, int entryId, string key, string value)
        {
            ObjectProvider.Instance().InsertPluginEntrySettings(pluginGuid, entryId, key, value);

			//Removes the list of enabled plugins for the current entry from the cache
			//This way we don't have strange caching issues
			RemoveEnabledPluginsWithEntrySettingsFromCache(entryId);
        }

        /// <summary>
        /// Updates a plugin setting for a specified entry
        /// </summary>
        /// <param name="pluginGuid">GUID of the plugin</param>
        /// <param name="entryId">Id of the blog entry</param>
        /// <param name="key">Setting name</param>
        /// <param name="value">Setting value</param>
        public static void UpdatePluginEntrySettings(Guid pluginGuid, int entryId, string key, string value)
        {
            ObjectProvider.Instance().UpdatePluginEntrySettings(pluginGuid, entryId, key, value);

			//Removes the list of enabled plugins for the current entry from the cache
			//This way we don't have strange caching issues
			RemoveEnabledPluginsWithEntrySettingsFromCache(entryId);
        }

        #endregion Entry Plugin Settings


        #region Cache managing helper classes

		private static void StoreEnabledPluginsWithBlogSettingsToCache(IDictionary<Guid, Plugin> pluginList)
		{
			int blogId = Subtext.Framework.Configuration.Config.CurrentBlog.Id;
			HttpContext.Current.Cache.Insert(String.Format(CultureInfo.InvariantCulture, PluginPerBlogCacheNameFormat, blogId), pluginList, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), CacheItemPriority.NotRemovable, null);
		}

		private static void RemoveEnabledPluginsWithBlogSettingsFromCache()
		{
			int blogId = Subtext.Framework.Configuration.Config.CurrentBlog.Id;
			HttpContext.Current.Cache.Remove(String.Format(CultureInfo.InvariantCulture, PluginPerBlogCacheNameFormat, blogId));
			Subtext.Framework.Configuration.Config.CurrentBlog.ClearEnablePluginsCache();
		}

		private static void StoreEnabledPluginsWithEntrySettingsToCache(IDictionary<Guid, Plugin> pluginList, int entryId)
		{
			HttpContext.Current.Cache.Insert(String.Format(CultureInfo.InvariantCulture, PluginPerEntryCacheNameFormat, entryId), pluginList, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), CacheItemPriority.NotRemovable, null);
		}

		private static void RemoveEnabledPluginsWithEntrySettingsFromCache(int entryId)
		{
			HttpContext.Current.Cache.Remove(String.Format(CultureInfo.InvariantCulture, PluginPerEntryCacheNameFormat, entryId));
		}

		#endregion Cache managing helper classes

	}
}
