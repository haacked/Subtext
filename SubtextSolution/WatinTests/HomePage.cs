using System;
using WatiN.Core;

namespace WatinTests
{
	public class HomePage : BrowserPageBase
	{
		public HomePage(Browser browser) : base(browser)
		{
		}

		public override string PageUrl
		{
			get { return "/"; }
		}

		public Link GetTitleLinkByText(string text)
		{
			Link link = Browser.Link(Find.ByText(text));
			return link;
		}
	}
}
