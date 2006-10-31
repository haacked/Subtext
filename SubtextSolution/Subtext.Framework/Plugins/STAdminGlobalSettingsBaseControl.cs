using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using Subtext.Framework.Providers;

namespace Subtext.Extensibility.Plugins
{
	public abstract class STAdminGlobalSettingsBaseControl : System.Web.UI.UserControl
	{

		private NameValueCollection _blogSettings = null;

		private NameValueCollection BlogSettings
		{
			get
			{
				if (_blogSettings == null)
					_blogSettings = GetBlogSettings();
				return _blogSettings;
			}
		}


		public string GetSetting(string key)
		{
			if (BlogSettings[key] != null)
				return BlogSettings[key];
			else
				return string.Empty;
		}

		public void SetSetting(string key, string value)
		{

			if (BlogSettings[key] == null)
			{
				BlogSettings.Add(key, value);
				ObjectProvider.Instance().InsertPluginGeneralSettings(_pluginGuid, key, value);
			}
			else
			{
				BlogSettings[key] = value;
				ObjectProvider.Instance().UpdatePluginGeneralSettings(_pluginGuid, key, value);
			}
		}


		private NameValueCollection GetBlogSettings()
		{
			if (_pluginGuid == Guid.Empty)
			{
				throw new InvalidOperationException("BlogSettings cannot be retrieved if no PluginGuid has been specified");
			}
			else
			{
				return ObjectProvider.Instance().GetPluginGeneralSettings(_pluginGuid);
			}
		}

		private Guid _pluginGuid=Guid.Empty;
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

		public abstract void LoadSettings();
		public abstract void UpdateSettings();
	}
}
