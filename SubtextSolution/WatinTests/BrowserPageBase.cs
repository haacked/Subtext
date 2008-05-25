using System;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace WatinTests
{
	public abstract class BrowserPageBase
	{
		private readonly Browser browser;
		
		public BrowserPageBase(Browser browser)
		{
			this.browser = browser;
		}

		abstract public string PageUrl {get;}

		public Browser Browser
		{
			get { return this.browser; }
		}

		public TextField ASPTextField(string id)
		{
			return browser.TextField(new Regex(".*" + id));
		}

		public Button ButtonByValue(string value)
		{
			return browser.Button(Find.ByValue(value));
		}

		public void GoToUrl(string relativeUrl)
		{
			browser.GoToUrl(relativeUrl);
		}

		public void ClickLink(string text)
		{
			browser.Link(Find.ByText(text)).Click();
		}

		protected void ClickNavLink<T>(T nav)
		{
			ClickLink(nav.ToString().Replace("_", " "));
		}

        public void ClickLinkNoWait(string text)
        {
            browser.Link(Find.ByText(text)).ClickNoWait();
        }

        public void ClickNavLinkNoWait<T>(T nav)
        {
            ClickLinkNoWait(nav.ToString().Replace("_", " "));
        }
	}
}
