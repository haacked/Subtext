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
using Subtext.Extensibility.Attributes;
using System.Collections.Specialized;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using System.Diagnostics;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Base class for all Subtext Plugins.
	/// </summary>
	public abstract class PluginBase
	{
		/// <summary>
		/// Initialize the plugin.<br />
		/// This is the only method that must be overridden since all actions are performed inside Event Handlers<br />
		/// The implementation of this method should only subscribe to the events raised by the SubtextApplication
		/// </summary>
		public abstract void Init(SubtextApplication application);

		private Guid _guid;

		/// <summary>
		/// Identifier of the plugin. This value has to be unique, so this interface provides a Guid.
		/// <remarks>It retrieves the value from the Identifier attribute</remarks>
		/// </summary>
		public Guid Id
		{
			get
			{
				if (_guid == Guid.Empty)
				{
					_guid=GetGuidFromAttribute(this.GetType());
				}
				return _guid;
			}
		}

		private PluginImplementationInfo _info;

		/// <summary>
		/// Provides information on the plugin: the author, the company, an url, a description, etc...
		/// <remarks>It retrieves the value from the Description attribute</remarks>
		/// </summary>
		public PluginImplementationInfo Info
		{
			get
			{
				if (_info == null)
				{
					_info = GetInfoFromAttribute(this.GetType());
				}
				return _info;
			}
        }

        #region Default Settings

        private PluginDefaultSettingsCollection _defaultSettings;

		/// <summary>
		/// Sitewide settings for the plugin
		/// </summary>
        internal PluginDefaultSettingsCollection DefaultSettings
		{
			get { return _defaultSettings; }
			set { _defaultSettings = value; }
        }

        /// <summary>
        /// Retrive a single Default Setting
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <returns>Value of the setting, as string. Must be deserialized if it represents something more complex than a string</returns>
        public string GetDefaultSetting(string key)
        {
            if (DefaultSettings[key] != null)
                return DefaultSettings[key];
            else
                return string.Empty;
        }

        #endregion Default Settings

        #region General Blog Settings

        /// <summary>
        /// Retrive a single Blog Setting
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <returns>Value of the setting, as string. Must be deserialized if it represents something more complex than a string</returns>
        public string GetBlogSetting(string key)
        {
			if (GetBlogSettings()[key] != null)
				return GetBlogSettings()[key];
            else
                return string.Empty;
        }

        /// <summary>
        /// Set a single blog setting, and persists it to the storage<br/>
        /// If the key already exists the value is updated, otherwise a new setting is created
        /// </summary>
        /// <param name="key">Name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public void SetBlogSetting(string key, string value)
        {
			if (GetBlogSettings()[key] == null)
            {
				Plugin.InsertPluginBlogSettings(Id, key, value);
            }
            else
            {
				Plugin.UpdatePluginBlogSettings(Id, key, value);
            }
        }

        //Retrieve the settings from the storage
        private NameValueCollection GetBlogSettings()
        {
			if (Id == Guid.Empty)
            {
                throw new InvalidOperationException("BlogSettings cannot be retrieved if a Plugin Id has not been specified");
            }
            else
            {
				Plugin plugin = Config.CurrentBlog.EnabledPlugins[Id];
				Debug.Assert(plugin != null, "Plugin is null, something must be wrong");
				return plugin.Settings;
            }
        }

        #endregion General Blog Settings


		//Retrieve the settings from the storage
		private NameValueCollection GetEntrySettings(Entry entry)
		{
			if (Id == Guid.Empty)
			{
				throw new InvalidOperationException("EntrySettings cannot be retrieved if no PluginGuid has been specified");
			}
			else
			{
				Plugin plugin = entry.EnabledPlugins[Id];
				Debug.Assert(plugin != null, "Plugin is null, something must be wrong");
				return plugin.Settings;
			}
		}

		//TODO: check if the entry is part of the current blog
		public string GetEntrySetting(Entry entry, string key)
		{
			string value = GetEntrySettings(entry)[key];
			if (value != null)
				return value;
			else
				return string.Empty;
		}

		//TODO: check if the entry is part of the current blog
		public void SetEntrySetting(Entry entry, string key, string value)
		{
			if (GetEntrySettings(entry)[key] == null)
			{
				Plugin.InsertPluginEntrySettings(Id, entry.Id, key, value);
			}
			else
			{
				Plugin.UpdatePluginEntrySettings(Id, entry.Id, key, value);
			}
			if (entry.EnabledPlugins[Id] != null)
				entry.EnabledPlugins[Id].Settings[key] = value;
		}

        #region Attribute Accessor Helpers
        private static PluginImplementationInfo GetInfoFromAttribute(Type type)
		{
			Attribute[] attrs = Attribute.GetCustomAttributes(type, typeof(DescriptionAttribute));
			foreach (Attribute attr in attrs)
			{
                DescriptionAttribute descAttr = attr as DescriptionAttribute;
                if (descAttr != null)
				{
					PluginImplementationInfo info = new PluginImplementationInfo();

					info._name = descAttr.Name;
					info._author = descAttr.Author;
					info._company = descAttr.Company;
					info._copyright = descAttr.Copyright;
					info._description = descAttr.Description;
					info._homepageUrl = new Uri(descAttr.HomePageUrl);
					info._version = new Version(descAttr.Version);
					return info;
				}
			}
			return null;
		}

		private static Guid GetGuidFromAttribute(Type type)
		{
			Attribute[] attrs = Attribute.GetCustomAttributes(type, typeof(IdentifierAttribute));
			foreach (Attribute attr in attrs)
			{
				IdentifierAttribute idAttr = attr as IdentifierAttribute;
				if (idAttr != null)
				{
					return idAttr.Guid;
				}
			}
			return Guid.Empty;
		} 
		#endregion

		public class PluginImplementationInfo
		{
			internal string _name;
			public string Name
			{
				get { return _name; }
			}

			internal string _author;
			public string Author
			{
				get { return _author; }
			}

			internal string _company;
			public string Company
			{
				get { return _company; }
			}

			internal string _copyright;
			public string Copyright
			{
				get { return _copyright; }
			}

			internal string _description;
			public string Description
			{
				get { return _description; }
			}

			internal Uri _homepageUrl;
			public Uri HomepageUrl
			{
				get { return _homepageUrl; }
			}

			internal Version _version;
			public Version Version
			{
				get { return _version; }
			}
		}

	}


}
