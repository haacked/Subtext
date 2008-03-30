using System;
using WatiN.Core;

namespace WatinTests
{
	public class ConfigPage : BrowserPageBase
	{
		public ConfigPage(Browser browser) : base(browser)
		{
		}

		public override string PageUrl
		{
			get { return "/Admin/Configure.aspx"; }
		}

		public TextField TitleField
		{
			get
			{
				return ASPTextField("txbTitle");
			}
		}

		public TextField SubtitleField
		{
			get
			{
				return ASPTextField("txbSubtitle");
			}
		}

		public TextField UsernameField
		{
			get
			{
				return ASPTextField("txbUser");
			}
		}

		public TextField OwnerName
		{
			get
			{
				return ASPTextField("txbAuthor");
			}
		}

		public TextField OwnerEmail
		{
			get
			{
				return ASPTextField("txbAuthorEmail");
			}
		}

		public void ClickSave()
		{
			ButtonByValue("Save").Click();
		}

		public void ClickNavLink(ConfigNavigationLink link)
		{
			base.ClickNavLink(link);
		}
	}

	public enum ConfigNavigationLink
	{
		Configure,
		Customize,
		Preferences,
		Syndication,
		Comments,
		Key_Words,
		Passwords,
	}
}
