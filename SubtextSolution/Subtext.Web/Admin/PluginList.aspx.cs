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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Subtext.Extensibility.Plugins;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using System.Web.UI;
using System.Web;

namespace Subtext.Web.Admin.Pages
{
	public partial class PluginListPage : AdminOptionsPage
	{
		private const string VSKEY_KEYWORDID = "PluginID";
		private const string VSKEY_PLUGINNAME = "PluginModuleName";

		private int _resultsPageNumber;
		private BlogInfo info;

		private SubtextAdminGlobalSettingsBaseControl settingModule;

		public string PluginID
		{
			get
			{
				if (ViewState[VSKEY_KEYWORDID] != null)
					return (string)ViewState[VSKEY_KEYWORDID];
				else
					return String.Empty;
			}
			set { ViewState[VSKEY_KEYWORDID] = value; }
		}

		protected override void Page_Load(object sender, EventArgs e)
		{
			info = Config.CurrentBlog;
			base.Page_Load(sender, e);
			ProcessPluginSettingsModule();
			if (!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				this.resultsPager.PageSize = Preferences.ListingItemCount;
				this.resultsPager.PageIndex = _resultsPageNumber;
				Results.Collapsible = false;

				BindList();
			}
		}

		//Get the plugin being edited so that its webcontrols are populated with the postback data
		private void ProcessPluginSettingsModule()
		{
			string completeModuleName = (string)ViewState[VSKEY_PLUGINNAME];
			if (!String.IsNullOrEmpty(completeModuleName))
			{
				PluginBase currentPlugin = SubtextApplication.Current.GetPluginByGuid(PluginID);
				if (currentPlugin != null)
				{
					settingModule = LoadControl(completeModuleName) as SubtextAdminGlobalSettingsBaseControl;
					settingModule.PluginGuid = currentPlugin.Id;
					settingModule.ID = "PluginSettingsEditControl";
					pluginSetting.Controls.Add(settingModule);
				}
			}
		}

		private void BindList()
		{
			View.Visible = false;
			Dictionary<Guid, PluginBase>.ValueCollection pluginList = SubtextApplication.Current.Plugins.Values;
			pluginListRpt.DataSource = pluginList;
			pluginListRpt.DataBind();
		}

		protected void pluginListRpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Label pluginName = (Label)e.Item.FindControl("pluginName");
				Label pluginVersion = (Label)e.Item.FindControl("pluginVersion");
				Label pluginCompany = (Label)e.Item.FindControl("pluginCompany");
				Label pluginDescription = (Label)e.Item.FindControl("pluginDescription");
				HtmlTableRow currentRow = (HtmlTableRow)e.Item.FindControl("currentRow");

				LinkButton lnkView = (LinkButton)e.Item.FindControl("lnkView");
				LinkButton lnkEnable = (LinkButton)e.Item.FindControl("lnkEnable");
				LinkButton lnkPluginSettings = (LinkButton)e.Item.FindControl("lnkPluginSettings");
				LinkButton lnkDisable = (LinkButton)e.Item.FindControl("lnkDisable");

				PluginBase currentPlugin = (PluginBase)e.Item.DataItem;

				currentRow.Attributes.Add("title", "Guid: " + currentPlugin.Id.ToString("B"));
				pluginName.Text = currentPlugin.Info.Name;
				pluginVersion.Text = currentPlugin.Info.Version.ToString();
				pluginCompany.Text = currentPlugin.Info.Company;
				pluginDescription.Text = currentPlugin.Info.Description;

				lnkView.CommandArgument = lnkEnable.CommandArgument = currentPlugin.Id.ToString();
				lnkPluginSettings.CommandArgument = lnkDisable.CommandArgument = currentPlugin.Id.ToString();

				if (IsPluginEnabled(currentPlugin))
				{
					lnkView.Visible = false;
					lnkPluginSettings.Visible = true;

					lnkEnable.Visible = false;
					lnkDisable.Visible = true;
				}
				else
				{
					lnkView.Visible = true;
					lnkPluginSettings.Visible = false;

					lnkEnable.Visible = true;
					lnkDisable.Visible = false;
				}
			}
		}

		private static bool IsPluginEnabled(PluginBase plugin)
		{
			return SubtextApplication.PluginEnabled(plugin);
		}

		protected void pluginListRpt_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			Guid pluginId;
			switch (e.CommandName.ToLower(System.Globalization.CultureInfo.InvariantCulture))
			{
				case "view":
					//Load the plugin info and display them on the page
					PluginID = (string)e.CommandArgument;
					BindViewLink();
					break;
				case "settings":
					ResetPostEdit(false);
					//Load the plugin info, and display the custom user control to edit global plugin settings
					PluginID = (string)e.CommandArgument;
					BindEditLink(true);
					break;
				case "enable":
					//Enable the plugin for the current blog
					pluginId = new Guid((string)e.CommandArgument);
					ConfirmToggle(pluginId, true);
					break;
				case "disable":
					//Disable the plugin for the current blog
					pluginId = new Guid((string)e.CommandArgument);
					ConfirmToggle(pluginId, false);
					break;
				default:
					break;
			}
		}

		private void BindEditLink(bool loadSetting)
		{
			PluginBase currentPlugin = SubtextApplication.Current.GetPluginByGuid(PluginID);
			if (currentPlugin != null)
			{
				string moduleFileName=SubtextApplication.Current.GetPluginModuleFileName(currentPlugin.Info.Name, "blogsettings");
				string completeModuleName = string.Empty;
				if (moduleFileName != null)
				{
					completeModuleName = "~/Modules/" + currentPlugin.Info.Name + "/" + moduleFileName;
					//Check to see if a plugin settings control is already in the page
					//and remove it
					if (pluginSetting.Controls.Contains(settingModule))
						pluginSetting.Controls.Remove(settingModule);
					try
					{
                        Control tmp = LoadControl(completeModuleName);
						settingModule = LoadControl(completeModuleName) as SubtextAdminGlobalSettingsBaseControl;
					}
					catch (HttpException ex)
					{
						Messages.ShowError("Cannot load general blog settings user control for Plugin " + currentPlugin.Info.Name + ":<br>" + ex.Message);
						return;
					}
					if (settingModule != null)
					{
						settingModule.PluginGuid = currentPlugin.Id;
						settingModule.ID = "PluginSettingsEditControl";
						pluginSetting.Controls.Add(settingModule);
						ViewState[VSKEY_PLUGINNAME] = completeModuleName;

						if (loadSetting)
							settingModule.LoadSettings();

						Results.Collapsed = true;
						Results.Collapsible = true;
						Edit.Visible = true;
						View.Visible = false;

						pluginEditName.Text = currentPlugin.Info.Name;

						if (AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
						{
							string title = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Plugin Settings: \"{0}\"", currentPlugin.Info.Name);

							AdminMasterPage.BreadCrumb.AddLastItem(title);
							AdminMasterPage.Title = title;
						}
					}
					else
					{
						Messages.ShowError("Cannot load general blog settings user control for Plugin " + currentPlugin.Info.Name + ":<br>" + moduleFileName + " doesn't inherit from Subtext.Extensibility.Plugins.STAdminGlobalSettingsBaseControl");
					}
				}
				else
				{
					Messages.ShowMessage("Plugin " + currentPlugin.Info.Name + " doesn't have a general blog settings manager.");
				}
			
			}
			else
				Messages.ShowMessage("Unable to find plugin with id" + PluginID);
		}

		private void ConfirmToggle(Guid pluginId, bool enable)
		{
			PluginBase currentPlugin = SubtextApplication.Current.GetPluginByGuid(pluginId);
			if (currentPlugin != null)
			{
				this.Command = new TogglePluginCommand(pluginId, currentPlugin.Info.Name, enable);
				this.Command.RedirectUrl = Request.Url.ToString();
				Server.Transfer(Constants.URL_CONFIRM);
			}
			else
				Messages.ShowMessage("Unable to find plugin with id" + PluginID);
		}

		private void BindViewLink()
		{
			PluginBase currentPlugin = SubtextApplication.Current.GetPluginByGuid(PluginID);
			if (currentPlugin != null)
			{
				Results.Collapsed = true;
				Results.Collapsible = true;
				Edit.Visible = false;
				View.Visible = true;

				pluginViewName.Text = currentPlugin.Info.Name;
				pluginViewGuid.Text = currentPlugin.Id.ToString();
				pluginViewAuthor.Text = currentPlugin.Info.Author;
				pluginViewCompany.Text = currentPlugin.Info.Company;
				pluginViewCopyright.Text = currentPlugin.Info.Copyright;
				pluginViewHomepage.Text = pluginViewHomepage.NavigateUrl = currentPlugin.Info.HomepageUrl.ToString();
				pluginViewVersion.Text = currentPlugin.Info.Version.ToString();
				pluginViewDescription.Text = currentPlugin.Info.Description;

				if (AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
				{
					string title = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Plugin Details: \"{0}\"", currentPlugin.Info.Name);

					AdminMasterPage.BreadCrumb.AddLastItem(title);
					AdminMasterPage.Title = title;
				}
			}
			else
				Messages.ShowMessage("Unable to find plugin with id" + PluginID);
		}


		private void ResetPostView(bool showEdit)
		{
			PluginID = String.Empty;

			Results.Collapsible = showEdit;
			Results.Collapsed = showEdit;
			View.Visible = showEdit;


			pluginViewName.Text = string.Empty;
			pluginViewGuid.Text = string.Empty;
			pluginViewAuthor.Text = string.Empty;
			pluginViewCompany.Text = string.Empty;
			pluginViewCopyright.Text = string.Empty;
			pluginViewHomepage.Text = string.Empty;
			pluginViewHomepage.NavigateUrl = string.Empty;
			pluginViewVersion.Text = string.Empty;
			pluginViewDescription.Text = string.Empty;

		}

		private void ResetPostEdit(bool showEdit)
		{
			PluginID = String.Empty;
            ViewState[VSKEY_PLUGINNAME] = string.Empty;

			Results.Collapsible = showEdit;
			Results.Collapsed = showEdit;
			Edit.Visible = showEdit;


			pluginEditName.Text = string.Empty;
			pluginSetting.Controls.Clear();
		}

		protected void lkbPost_Click(object sender, System.EventArgs e)
		{
			UpdatePluginSettings();
		}

		private void UpdatePluginSettings()
		{
			settingModule.UpdateSettings();
		}

		protected void lkbCancel_Click(object sender, System.EventArgs e)
		{
			ResetPostView(false);
		}

		protected void lkbEditCancel_Click(object sender, System.EventArgs e)
		{
			ResetPostEdit(false);
		}
	}
}
