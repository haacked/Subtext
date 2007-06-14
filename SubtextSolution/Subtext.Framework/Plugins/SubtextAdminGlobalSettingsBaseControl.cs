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

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Base control which must be inherited by the plugin module that collects the
	/// general settings for a single blog
	/// </summary>
	public abstract class SubtextAdminGlobalSettingsBaseControl : System.Web.UI.UserControl
	{

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
		///		txbValue1.Text = CurrentPlugin.GetSetting("value1");
		///		if (!String.IsNullOrEmpty(GetSetting("check")))
		///		{
		///			bool checkOn = Boolean.Parse(GetSetting("check"));
		///			chkOption.Checked = checkOn;
		///		}
		///		txbValue2.Text = CurrentPlugin.GetSetting("value2");
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
		///		CurrentPlugin.SetSetting("value1",txbValue1.Text);
		///		CurrentPlugin.SetSetting("value2", txbValue2.Text);
		///		if (chkOption.Checked)
		///			CurrentPlugin.SetSetting("check", "true");
		///		else
		///			CurrentPlugin.SetSetting("check", "false");
		///	}
		/// 
		/// </example>
		public abstract void UpdateSettings();
	}
}
