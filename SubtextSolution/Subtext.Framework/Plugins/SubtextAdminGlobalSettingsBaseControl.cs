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
using System.Web;
using System.Collections.Specialized;
using Subtext.Framework.Providers;
using Subtext.Framework.Components;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Base control which must be inherited by the plugin module that collects the
	/// general settings for a single blog
	/// </summary>
	public abstract class SubtextAdminGlobalSettingsBaseControl : System.Web.UI.UserControl
	{
		private NameValueCollection _blogSettings;
		
		//Helper properties used to cache the settings during the lifetime of the module
		private NameValueCollection BlogSettings
		{
			get
			{
				if (_blogSettings == null)
					_blogSettings = GetBlogSettings();
				return _blogSettings;
			}
		}

		/// <summary>
		/// Retrive a single Blog Setting
		/// </summary>
		/// <param name="key">Name of the setting</param>
		/// <returns>Value of the setting, as string. Must be deserialized if it represents something more complex than a string</returns>
		public string GetSetting(string key)
		{
			if (BlogSettings[key] != null)
				return BlogSettings[key];
			else
				return string.Empty;
		}

		/// <summary>
		/// Set a single blog setting, and persists it to the storage<br/>
		/// If the key already exists the value is updated, otherwise a new setting is created
		/// </summary>
		/// <param name="key">Name of the setting</param>
		/// <param name="value">Value of the setting</param>
		public void SetSetting(string key, string value)
		{

			if (BlogSettings[key] == null)
			{
				BlogSettings.Add(key, value);
				Plugin.InsertPluginGeneralSettings(_pluginGuid, key, value);
			}
			else
			{
				BlogSettings[key] = value;
				Plugin.UpdatePluginGeneralSettings(_pluginGuid, key, value);
			}
		}

		//Retrieve the settings from the storage
		private NameValueCollection GetBlogSettings()
		{
			if (_pluginGuid == Guid.Empty)
			{
				throw new InvalidOperationException("BlogSettings cannot be retrieved if no PluginGuid has been specified");
			}
			else
			{
				return Subtext.Framework.Configuration.Config.CurrentBlog.EnabledPlugins[_pluginGuid].Settings;
			}
		}

		private Guid _pluginGuid=Guid.Empty;
		/// <summary>
		/// Guid of the plugin being edited
		/// </summary>
		public Guid PluginGuid
		{
			get
			{
				return _pluginGuid;
			}

			set
			{
				_pluginGuid = value;
			}
		}

		public PluginBase CurrentPlugin
		{
			get
			{
				return SubtextApplication.Current.GetPluginByGuid(_pluginGuid);
			}
		}

		/// <summary>
		/// This method must be implemented by the page settings page.<br/>
		/// It should be responsible for binding the Plugin Blog settings to the custom UI
		/// </summary>
		/// <example>
		/// 
		///	public override void LoadSettings()
		///	{
		///		txbValue1.Text = GetSetting("value1");
		///		if (!String.IsNullOrEmpty(GetSetting("check")))
		///		{
		///			bool checkOn = Boolean.Parse(GetSetting("check"));
		///			chkOption.Checked = checkOn;
		///		}
		///		txbValue2.Text = GetSetting("value2");
		///	}
		/// 
		/// </example>
		public abstract void LoadSettings();
		/// <summary>
		/// This method must be implemented by the page settings page.<br/>
		/// It should be responsible for taking the data out of the custom UI and saving them the to Settings List
		/// </summary>
		/// <example>
		/// 
		/// public override void UpdateSettings()
		///	{
		///		SetSetting("value1",txbValue1.Text);
		///		SetSetting("value2", txbValue2.Text);
		///		if (chkOption.Checked)
		///			SetSetting("check", "true");
		///		else
		///			SetSetting("check", "false");
		///	}
		/// 
		/// </example>
		public abstract void UpdateSettings();
	}
}
